# TaskFlow

<a href="https://ibb.co/bMtSrdTL"><img src="https://i.ibb.co/4nk6jNzM/task1.jpg" alt="task1" border="0"></a>
<a href="https://ibb.co/7xr9qF1q"><img src="https://i.ibb.co/spsBZrVZ/task2.jpg" alt="task2" border="0"></a>
<a href="https://ibb.co/XnpqXbx"><img src="https://i.ibb.co/Mzpmnsk/task3.jpg" alt="task3" border="0"></a>
<a href="https://ibb.co/7trQKZQ6"><img src="https://i.ibb.co/HTGCqbCk/task4.jpg" alt="task4" border="0"></a>
<a href="https://ibb.co/tTBzqtT4"><img src="https://i.ibb.co/67Y8Hx7m/task5.jpg" alt="task5" border="0"></a>


TaskFlow, ekipler için görev ve proje yönetimi sağlayan modern bir web uygulamasıdır. Proje, .NET 8 tabanlı bir backend API ve React tabanlı bir frontend içerir.

## Özellikler

- Proje oluşturma, güncelleme, silme
- Görev yönetimi (ekleme, atama, durum takibi)
- Takım üyeleriyle işbirliği
- JWT ile kimlik doğrulama
- Modern ve kullanıcı dostu arayüz

## Proje Yapısı

```
src/
  Core/
    TaskFlow.Application/   # Uygulama katmanı (CQRS, DTO, servisler)
    TaskFlow.Domain/        # Domain modelleri
  Infrastructure/
    TaskFlow.Infrastructure/ # Veri erişim ve altyapı
  Presentation/
    TaskFlow.API/           # .NET Web API
  client/
    frontend/               # React tabanlı frontend
```

## Kurulum

### Backend (.NET API)

1. Gerekli bağımlılıkları yükleyin:
   ```sh
   cd src/Presentation/TaskFlow.API
   dotnet restore
   ```

2. Veritabanını oluşturun ve migrasyonları uygulayın:
   ```sh
   dotnet ef database update
   ```

3. API'yi başlatın:
   ```sh
   dotnet run
   ```
   API varsayılan olarak `https://localhost:5001` adresinde çalışır.

### Frontend (React)

1. Gerekli bağımlılıkları yükleyin:
   ```sh
   cd src/client/frontend
   npm install
   ```

2. Frontend'i başlatın:
   ```sh
   npm run dev
   ```
   Uygulama varsayılan olarak `http://localhost:8080` adresinde çalışır.

## Ortam Değişkenleri

- Backend için veritabanı bağlantı ayarlarını `appsettings.json` dosyasında yapılandırabilirsiniz.
- Frontend için API adresini `.env` dosyasında ayarlayabilirsiniz.

## Geliştirici Notları

- Swagger dokümantasyonu için API çalışırken `/swagger` adresini ziyaret edebilirsiniz.
- JWT ile kimlik doğrulama gereklidir. Kayıt ve giriş işlemleri frontend üzerinden yapılabilir.

## Katkıda Bulunma

Katkıda bulunmak için lütfen bir fork oluşturun ve pull request gönderin.

## Lisans

MIT

---

> TaskFlow ile ekip çalışmasını kolaylaştırın!
