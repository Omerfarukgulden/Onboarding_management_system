# Çalışan Onboarding Yönetim Sistemi

ASP.NET Core Web API ile geliştirilmiş, yeni çalışanların işe giriş (onboarding) süreçlerini şablonlar üzerinden otomatik oluşturup takip eden bir sistem.

---

## Gereksinimler

| Araç | Sürüm |
|---|---|
| .NET SDK | **10.0.203** |
| Docker Desktop | Güncel sürüm |
| Git | Güncel sürüm |
| IDE | Visual Studio 2022 Community veya JetBrains Rider |

> SQL Server, Docker container olarak çalıştığı için ayrıca kurulum gerekmez.

---

## Kullanılan Teknolojiler ve Paketler

| Paket | Sürüm | Amaç |
|---|---|---|
| ASP.NET Core Web API | 10.0 | Web API çatısı |
| AutoMapper | 16.2.0 | Entity ↔ DTO dönüşümü |
| BCrypt.Net-Next | 4.2.0 | Şifre hash'leme |
| FluentValidation.AspNetCore | 11.3.1 | DTO doğrulama |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.10 | JWT kimlik doğrulama |
| Microsoft.AspNetCore.OpenApi | 10.0.7 | OpenAPI desteği |
| Microsoft.EntityFrameworkCore | 10.0.10 | ORM |
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.10 | SQL Server sağlayıcısı |
| Microsoft.EntityFrameworkCore.Design | 10.0.10 | Migration araçları |
| Microsoft.EntityFrameworkCore.Tools | 10.0.10 | EF CLI araçları |
| Swashbuckle.AspNetCore | 10.2.3 | Swagger UI |

**Veritabanı:** Microsoft SQL Server 2022 (16.0.4265.3) — Docker üzerinde Linux (Ubuntu 22.04) container olarak çalışır.

---

## Kurulum Adımları

### 1) Projeyi indir

```bash
git clone <https://github.com/Omerfarukgulden/Onboarding_management_system/tree/main>
cd Onboard_management_system/Onboard_management_system
```

### 2) SQL Server Docker container'ını başlat

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name mssql-onboarding -d mcr.microsoft.com/mssql/server:2022-latest
```

> Apple Silicon (M1/M2/M3) Mac kullanıyorsan `--platform linux/amd64` ekle:
> ```bash
> docker run --platform linux/amd64 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
>   -p 1433:1433 --name mssql-onboarding -d mcr.microsoft.com/mssql/server:2022-latest
> ```

Container'ın ayağa kalkması için ~15-20 saniye bekle.

### 3) Connection string'i ayarla

`appsettings.json` dosyasındaki `DefaultConnection` değerini, Docker'ı başlatırken kullandığın şifreyle güncelle:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=OnboardingDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

### 4) Paketleri yükle

```bash
dotnet restore
```

### 5) Migration'ı uygula ve veritabanını oluştur

```bash
dotnet ef database update
```

Bu komut:
- `OnboardingDb` veritabanını otomatik oluşturur
- Tüm tabloları oluşturur

> Uygulama **ilk çalıştığında** seed data otomatik yüklenir — departmanlar, pozisyonlar, kullanıcılar ve örnek şablon.

### 6) Projeyi başlat

**Terminal ile:**
```bash
dotnet run --launch-profile http
```

**IDE ile:**
- Visual Studio 2022: `http` profilini seçip ▶️ Run
- JetBrains Rider: Run Configuration'dan `http` profilini seçip ▶️ Run

---

## Swagger Adresi

```
http://localhost:5010/swagger
```

---

## Sisteme Giriş

Uygulama ilk çalıştığında aşağıdaki örnek kullanıcılar otomatik oluşturulur:

| Kullanıcı Adı | Şifre | Rol | Açıklama |
|---|---|---|---|
| `admin` | `Admin123!` | Admin | Kullanıcı, departman, pozisyon, şablon yönetimi |
| `ik.sorumlu` | `Ik1234!` | İK (HR) | Çalışan kaydı, onboarding süreci başlatma |
| `yazilim.sorumlu` | `Dept123!` | DepartmentUser | Kendine/departmanına atanan görevleri yönetme |

### Swagger'da token ile istek atmak

1. `POST /api/Auth/login` endpoint'ini çalıştır, kullanıcı adı ve şifre gir
2. Dönen `token` değerini kopyala
3. Swagger sayfasının sağ üstündeki **Authorize 🔓** butonuna tıkla
4. Token değerini yapıştır, **Authorize** butonuna bas
5. Artık yetkili endpoint'lere istek atabilirsin

---

## Kullanıcı Rolleri ve Yetkileri

| Rol | Yetkiler |
|---|---|
| **Admin** | Departman, pozisyon, kullanıcı ve onboarding şablonu yönetimi |
| **İK (HR)** | Çalışan kaydı oluşturma, onboarding süreci başlatma, tüm süreçleri takip etme |
| **DepartmentUser** | Kendisine veya departmanına atanan görevleri görüntüleme ve güncelleme |

---

## Temel Özellikler

- JWT Authentication + rol bazlı yetkilendirme
- BCrypt ile güvenli şifre hash'leme
- Onboarding şablonu üzerinden otomatik görev oluşturma
- Zorunlu görevler tamamlanınca süreç otomatik "Tamamlandı" olur
- Her görev durum değişikliği geçmişe kaydedilir
- Pasif departmana çalışan eklenemez
- Soft delete (fiziksel silme yerine pasife alma)
- Filtreleme ve sayfalama desteği
- Geciken görevler endpoint'i
- Global exception handling middleware
- FluentValidation ile DTO doğrulama
- Seed data ile otomatik başlangıç verisi
- Önemli işlemler için loglama

---

## Endpoint Özeti

| Grup | Endpoint | Rol |
|---|---|---|
| Auth | `POST /api/Auth/login` | Herkes |
| Departments | `GET/POST/PUT/DELETE /api/Departments` | Admin |
| Positions | `GET/POST/PUT/DELETE /api/Positions` | Admin |
| Users | `GET/POST/PUT/DELETE /api/Users` | Admin |
| OnboardingTemplates | `GET/POST/PUT/DELETE /api/OnboardingTemplates` | Admin |
| OnboardingTemplates | `POST /api/OnboardingTemplates/{id}/tasks` | Admin |
| OnboardingTemplates | `GET /api/OnboardingTemplates/{id}/tasks` | Admin |
| Employees | `GET/POST/PUT/DELETE /api/Employees` | İK |
| OnboardingProcesses | `GET/POST /api/OnboardingProcesses` | İK |
| OnboardingTasks | `GET /api/OnboardingTasks` | İK, DepartmentUser |
| OnboardingTasks | `GET /api/OnboardingTasks/overdue` | İK, DepartmentUser |
| OnboardingTasks | `PUT /api/OnboardingTasks/{id}/status` | İK, DepartmentUser |
| OnboardingTasks | `PUT /api/OnboardingTasks/{id}/note` | İK, DepartmentUser |
| OnboardingTasks | `GET /api/OnboardingTasks/{id}/history` | İK, DepartmentUser |

---

## Proje Yapısı

```
src/
├── OnboardingDomain/
│   ├── Entities/          (9 entity: Employee, Department, Position, User,
│   │                        OnboardingTemplate, OnboardingTemplateTask,
│   │                        OnboardingProcess, OnboardingTask, TaskStatusHistory)
│   └── Enums/             (UserRole, OnboardingStatus, TaskStatus)
├── OnboardingApplication/
│   ├── Common/            (PagedResult, PaginationParams, ApiErrorResponse)
│   ├── Dtos/              (tüm DTO'lar)
│   ├── Interfaces/        (servis arayüzleri)
│   ├── Services/          (iş mantığı)
│   ├── Mapping/           (AutoMapper profilleri)
│   └── Validators/        (FluentValidation)
├── OnboardingInfrastructure/
│   ├── Context/           (OnboardingDbContext)
│   ├── Configurations/    (EF entity konfigürasyonları)
│   ├── Migrations/        (EF migration dosyaları)
│   └── Seed/              (SeedData)
└── OnboardingAPI/
    ├── Controller/        (tüm controller'lar)
    └── Middleware/        (GlobalExceptionMiddleware)
```

---

## Sık Karşılaşılan Sorunlar

**Docker container başlamıyor:**
```bash
docker start mssql-onboarding
```

**"SQL Server'a bağlantı kurulamadı" hatası:**
- Docker Desktop'ın açık olduğundan emin ol
- `docker ps` ile container'ın çalıştığını doğrula
- `appsettings.json`'daki şifrenin Docker'ı başlatırken kullandığınla aynı olduğunu kontrol et

**Migration hatası / veritabanını sıfırlamak:**
```bash
dotnet ef database drop --force
dotnet ef database update
```

**dotnet-ef aracı bulunamadı:**
```bash
dotnet tool install --global dotnet-ef
```

