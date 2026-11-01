using Microsoft.AspNetCore.Mvc;
using SerieA.WebUI.ViewModels;

namespace SerieA.WebUI.Controllers
{
    // Puan durumu sayfasının controller'ı. Adres: /Standings
    public class StandingsController : Controller
    {
        // Program.cs'te tanıttığımız HttpClient'ları üreten fabrika
        private readonly IHttpClientFactory _httpClientFactory;

        // IHttpClientFactory buraya otomatik gelir (Dependency Injection)
        public StandingsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Sayfa açılınca çalışan metot
        public async Task<IActionResult> Index()
        {
            // Program.cs'te "SerieAApi" adıyla tanıttığımız client'ı al (adresi hazır gelir)
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // API'ye GET isteği at ve gelen JSON'u StandingViewModel listesine çevir
            var standings = await client.GetFromJsonAsync<List<StandingViewModel>>("api/standings");

            // Veri gelmezse (null) boş liste gönder, sayfa hata vermesin
            return View(standings ?? new List<StandingViewModel>());
        }
    }
}