using Microsoft.EntityFrameworkCore;
using SerieA.API.Context;

var builder = WebApplication.CreateBuilder(args);

// SerieAContext'i uygulamaya tanýtýyoruz (Dependency Injection)
// appsettings.json içindeki "SqlConnection" adresini kullanarak SQL Server'a baðlanýr
builder.Services.AddDbContext<SerieAContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Controller sýnýflarýný kullanabilmemizi saðlar
builder.Services.AddControllers();

// Swagger: API'yi tarayýcýdan denememizi saðlayan test ekraný
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geliþtirme sýrasýnda Swagger ekranýný aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP isteklerini otomatik HTTPS'e yönlendirir
app.UseHttpsRedirection();

app.UseAuthorization();

// Gelen istekleri ilgili Controller'a yönlendirir
app.MapControllers();

app.Run();