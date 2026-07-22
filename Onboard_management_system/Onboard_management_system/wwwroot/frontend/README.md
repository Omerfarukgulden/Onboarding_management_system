# Onboarding Yönetim Sistemi - Basit Frontend

Bu klasör, backend'i (ASP.NET Core Web API, .NET) test etmek için hazırlanmış
sade bir HTML/CSS/JS arayüzdür. Herhangi bir framework veya build adımı yoktur.

## Dosyalar
- `login.html` - giriş ekranı (JWT token alınır)
- `dashboard.html` - role göre menü ve tüm ekranlar
- `js/api.js` - API çağrıları için ortak fetch fonksiyonu
- `js/auth.js` - token/rol saklama (localStorage)
- `js/app.js` - dashboard içindeki tüm bölümlerin mantığı
- `css/style.css` - basit stil

## 1) Backend'de CORS ayarı (ÖNEMLİ)

Backend projesinde şu anda CORS ayarı yok. Bu frontend farklı bir origin'den
(örn. Live Server, `http://localhost:5500` veya `file://`) API'ye istek attığı
için backend'in bu isteklere izin vermesi lazım, yoksa tarayıcı isteği bloklar.

`Program.cs` dosyasına şu satırları ekle:

```csharp
// builder.Services.AddAuthorization(); satırından önce:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

ve `app.UseAuthentication();` satırından ÖNCE:

```csharp
app.UseCors("AllowAll");
```

(Bu ayar sadece geliştirme/test amaçlıdır, canlıya alırken origin'i daraltmalısın.)

## 2) API adresi

`js/api.js` dosyasının en üstünde:

```js
const API_BASE = "http://localhost:5010/api";
```

Backend'i farklı bir portta çalıştırıyorsan (launchSettings.json'daki
`applicationUrl` değerine bak) burayı ona göre güncelle.

## 3) Çalıştırma

Bu dosyaları doğrudan çift tıklayıp tarayıcıda açabilirsin ama bazı
tarayıcılar `file://` üzerinden fetch isteklerini kısıtlayabiliyor. En
sorunsuz yöntem basit bir local server açmak:

```bash
cd frontend
python -m http.server 5500
```

sonra tarayıcıdan `http://localhost:5500/login.html` adresine git.

Backend'i de aynı anda `dotnet run` ile ayağa kaldırmayı unutma.

## 4) Roller ve görebildikleri ekranlar

- **Admin** → Departmanlar, Pozisyonlar, Kullanıcılar
- **Ik** (İK) → Çalışanlar, Onboarding Şablonları, Onboarding Süreçleri, Görevler
- **DepartmentUser** → sadece Görevler

Giriş yaptığın kullanıcının rolüne göre sol menü otomatik değişir.

## Not

Basit tutulması istendiği için: dropdown yerine bazı yerlerde (örn. Çalışan
formundaki Departman/Pozisyon Id, Süreç başlatmadaki Çalışan/Şablon Id) direkt
sayısal Id girişi var. İlgili Id'leri Admin panelinden (Departmanlar,
Pozisyonlar) veya Çalışanlar/Şablonlar listesinden görebilirsin.
