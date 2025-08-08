## Hikvision Time-Lapse Kamera ve Video Otomasyonu

Modern, Windows Forms tabanlı bir uygulama ile Hikvision (veya RTSP uyumlu) IP kameralardan belirli saatlerde otomatik fotoğraf çekerek bunlardan time‑lapse videolar üreten bir çözüm.

### Özellikler (TR)
- Çoklu kamera desteği (her kamera için ayrı RTSP, kullanıcı/şifre, aktiflik).
- Günlük çekim saatlerini UI üzerinden belirleme; saatler normalize/tekilleştirilir.
- Planlayıcı (Quartz) ile güvenilir zamanlanmış çekimler; trigger çakışmalarına dayanıklı.
- Tek seferlik manuel testler: Kamera bazında bağlantı testi ve görüntü önizleme (snapshot).
- Fotoğrafların dosya hiyerarşisi: `photos/{cameraId}/YYYY-MM-DD/HH-mm-ss.jpg`.
- Time‑lapse üretimi:
  - Tek kamera: `photos/` kökünden sırayla birleştirme.
  - Çoklu kamera: Seçili/aktif kameraların tüm kareleri zaman sırasına göre tek videoda birleştirme (interleave).
  - FPS, genişlik, yükseklik UI’dan ayarlanabilir (varsayılan 25 FPS, 1920x1080).
  - Codec fallback: H264 → MP4V → XVID.
- Loglama: NLog ile `logs/app.log` + UI log kutusu.
- Tray davranışı: Yalnızca minimize edilince tepsiye iner; X ile uygulama kapanır.
- “Hakkında” ileti kutusu: Uygulama adı, geliştirici (aoaydin), sürüm.

### Features (EN)
- Multi‑camera support (per‑camera RTSP, username/password, enabled flag).
- Configure daily capture times in UI (normalized, de‑duplicated HH:mm).
- Scheduler (Quartz) for reliable captures; resilient to trigger clashes (reschedule).
- Per‑camera quick tests: connection test and snapshot preview.
- Photo storage layout: `photos/{cameraId}/YYYY-MM-DD/HH-mm-ss.jpg`.
- Timelapse generation:
  - Single camera: straight merge from `photos/`.
  - Multi‑camera: interleave frames by timestamp across selected cameras into one video.
  - FPS/Width/Height configurable (default 25 FPS, 1920×1080).
  - Codec fallback: H264 → MP4V → XVID.
- Logging with NLog to `logs/app.log` + UI log box.
- Tray behavior: minimize to tray only; close exits the app.
- About dialog with app/developer/version info.

---

## Kurulum (TR)
### Gereksinimler
- Windows 10/11
- .NET 8 SDK
- RTSP akışı veren IP kamera (Hikvision veya muadili)

### Çalıştırma
1) Çözümü derleyin:
   - `dotnet build HikvisionTimeLapse.sln -c Release`
2) Uygulamayı çalıştırın:
   - `dotnet run --project HikvisionTimeLapse/HikvisionTimeLapse.csproj -c Release`
   - veya `HikvisionTimeLapse\bin\Release\net8.0-windows\HikvisionTimeLapse.exe`

### İlk Yapılandırma
- `Kameralar...` butonundan kamera ekleyin:
  - Ad, RTSP, kullanıcı, şifre girin
  - Bağlantı testi yapın ve dilerse önizleme alın
  - Kaydedin
- Sağdaki liste ile günlük saatlerinizi ekleyip kaydedin.

### Kullanım
- Başlat: Planlayıcıyı aktifleştirir; belirlenen saatlerde çekim yapılır.
- Durdur: Planlayıcıyı kapatır.
- Çoklu kamera (sırala/birleştir): İşaretliyse çoklu kameralar tek videoda birleştirilir.
- Time‑lapse Oluştur: Mevcut fotoğraflardan MP4 üretir (UI’daki FPS/çözünürlükle).
- Minimize → tray; X → kapanış.

### Yapılandırma Dosyaları
- `settings.json` uygulama çalışma dizinine kopyalanır (projede tanımlıdır).
- Örnek:
  ```json
  {
    "Cameras": [
      {
        "Name": "Cam 1",
        "RtspUrl": "rtsp://IP:554/Streaming/Channels/101",
        "Username": "admin",
        "Password": "pass",
        "Enabled": true
      }
    ],
    "DailyCaptureTimes": ["10:00", "13:00", "16:30"],
    "PhotosRootDirectory": "photos",
    "OutputDirectory": "output",
    "DefaultFps": 25,
    "DefaultWidth": 1920,
    "DefaultHeight": 1080
  }
  ```

### NuGet Paketleri
- OpenCvSharp4, OpenCvSharp4.runtime.win
- Quartz
- Newtonsoft.Json
- NLog
- System.Drawing.Common

### Sorun Giderme
- RTSP bağlantı başarısız:
  - Aynı ağda mısınız? `ping <kamera-ip>`
  - RTSP yolunu modelinize göre kontrol edin (ör. `.../Channels/101?transportmode=unicast`).
  - TCP zorlaması parametresi eklenir (`rtsp_transport=tcp`).
  - Güvenlik duvarı ve port 554 erişimine izin verin.
- Video yazma hatası: H264 decoder/encoder yoksa MP4V/XVID fallback çalışır. Yine de başarısızsa OpenCV codec desteğini kurun.
- Quartz tetikleyici çakışması: Rescheduling logic aktif; saatler UI’da tekilleştirilir.

---

## Setup (EN)
See Turkish section for details. In short: install .NET 8 SDK, build the solution, add cameras via UI, configure times, start scheduler, and create timelapse videos. Use per‑camera Test/Preview to validate connectivity.

---

## Proje Yapısı / Project Structure
- `HikvisionTimeLapse` (WinForms UI)
  - `Form1` (main)
  - `CamerasForm` (camera list + times)
  - `CameraEditForm` (add/edit + test + preview)
  - `NLog.config`, `settings.json`
- `HikvisionTimeLapse.Core`
  - `Models` (`AppSettings`, `CameraConfig`)
  - `Services` (`SettingsService`, `LoggingService`, `CaptureService`, `TimelapseService`, `SchedulerService`)
- `HikvisionTimeLapse.Tests` (MSTest)

---

## Lisans
Bu repo örnek amaçlı hazırlanmıştır. Kurum içi/kişisel kullanım için serbestçe uyarlayabilirsiniz.
