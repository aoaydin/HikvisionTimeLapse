using System;
using System.Linq;
using System.Threading.Tasks;
using HikvisionTimeLapse.Core.Models;
using Quartz;
using Quartz.Impl;

namespace HikvisionTimeLapse.Core.Services;

public interface ISchedulerService
{
    Task StartAsync(AppSettings settings, Func<Task> onCapture); // legacy single camera
    Task StartAsync(AppSettings settings, Func<string, Task> onCapturePerCamera); // per-camera callback with cameraId
    Task StopAsync();
    bool IsRunning { get; }
}

public sealed class SchedulerService : ISchedulerService
{
    private IScheduler? _scheduler;
    public bool IsRunning => _scheduler?.IsStarted ?? false;

    public async Task StartAsync(AppSettings settings, Func<Task> onCapture)
    {
        if (_scheduler != null && _scheduler.IsStarted) return;

        var factory = new StdSchedulerFactory();
        _scheduler = await factory.GetScheduler();

        var jobKey = new JobKey("CaptureJob", "default");
        var job = JobBuilder.Create<CaptureJob>()
            .WithIdentity(jobKey)
            .StoreDurably(true) // allow multiple triggers to reference same job
            .Build();

        job.JobDataMap["onCapture"] = onCapture;
        // Add or replace the job definition once
        await _scheduler.AddJob(job, replace: true, storeNonDurableWhileAwaitingScheduling: true);

        foreach (var time in settings.DailyCaptureTimes)
        {
            if (!TimeSpan.TryParse(time, out var ts))
                continue;

            var cron = $"0 {ts.Minutes} {ts.Hours} ? * *"; // seconds minutes hours day-of-month month day-of-week
            var triggerKey = new TriggerKey($"CaptureTrigger_{time}", "default");
            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey)
                .WithCronSchedule(cron)
                .Build();

            if (await _scheduler.CheckExists(triggerKey))
            {
                await _scheduler.RescheduleJob(triggerKey, trigger);
            }
            else
            {
                await _scheduler.ScheduleJob(trigger);
            }
        }

        await _scheduler.Start();
    }

    public async Task StartAsync(AppSettings settings, Func<string, Task> onCapturePerCamera)
    {
        if (_scheduler != null && _scheduler.IsStarted) return;

        var factory = new StdSchedulerFactory();
        _scheduler = await factory.GetScheduler();

        foreach (var camera in settings.Cameras.Where(c => c.Enabled))
        {
            var jobKey = new JobKey($"CaptureJob_{camera.Id}", "default");
            var job = JobBuilder.Create<CameraCaptureJob>()
                .WithIdentity(jobKey)
                .StoreDurably(true)
                .Build();
            job.JobDataMap["cameraId"] = camera.Id;
            job.JobDataMap["onCapturePerCamera"] = onCapturePerCamera;
            await _scheduler.AddJob(job, replace: true, storeNonDurableWhileAwaitingScheduling: true);

            foreach (var time in settings.DailyCaptureTimes)
            {
                if (!TimeSpan.TryParse(time, out var ts))
                    continue;
                var cron = $"0 {ts.Minutes} {ts.Hours} ? * *";
                var triggerKey = new TriggerKey($"CaptureTrigger_{camera.Id}_{time}", "default");
                var trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerKey)
                    .ForJob(jobKey)
                    .WithCronSchedule(cron)
                    .Build();
                if (await _scheduler.CheckExists(triggerKey))
                {
                    await _scheduler.RescheduleJob(triggerKey, trigger);
                }
                else
                {
                    await _scheduler.ScheduleJob(trigger);
                }
            }
        }

        await _scheduler.Start();
    }

    public async Task StopAsync()
    {
        if (_scheduler != null)
        {
            await _scheduler.Shutdown(waitForJobsToComplete: false);
            _scheduler = null;
        }
    }

    private sealed class CaptureJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.JobDetail.JobDataMap["onCapture"] is Func<Task> onCapture)
            {
                await onCapture();
            }
        }
    }

    private sealed class CameraCaptureJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.JobDetail.JobDataMap["onCapturePerCamera"] is Func<string, Task> onCapturePerCamera)
            {
                var cameraId = context.JobDetail.JobDataMap.GetString("cameraId") ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(cameraId))
                {
                    await onCapturePerCamera(cameraId);
                }
            }
        }
    }
}


