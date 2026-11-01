var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// API ile konuþacak HttpClient'ý tanýtýyoruz. Ýsmi "SerieAApi"
// Controller'larda bu isimle çaðýracaðýz, adresi her yerde tekrar yazmamýza gerek kalmaz
builder.Services.AddHttpClient("SerieAApi", client =>
{
    // API'nin adresi (Swagger'da gördüðümüz adres)
    client.BaseAddress = new Uri("https://localhost:7011/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ana adres (/) açýlýnca varsayýlan olarak puan durumu sayfasý gelir
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Standings}/{action=Index}/{id?}");

app.Run();