using Microsoft.AspNetCore.Mvc;
using SerieA.WebUI.ViewModels;

namespace SerieA.WebUI.Controllers
{
    // Maç detay sayfasının controller'ı. Adres: /Match/Detail/5
    public class MatchController : Controller
    {
        // Program.cs'te tanıttığımız HttpClient'ları üreten fabrika
        private readonly IHttpClientFactory _httpClientFactory;

        public MatchController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Adresteki sayı (/Match/Detail/5) buradaki "id" parametresine gelir
        public async Task<IActionResult> Detail(int id)
        {
            // API'ye bağlanacak client (adresi Program.cs'ten hazır gelir)
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // API'den maçın tüm detayını iste
            var response = await client.GetAsync($"api/matches/{id}");

            // API 404 döndüyse (böyle bir maç yok) sayfada da "bulunamadı" göster
            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // Gelen JSON'u MatchDetailViewModel'e çevir
            var match = await response.Content.ReadFromJsonAsync<MatchDetailViewModel>();

            // Çeviri başarısız olursa (null) yine "bulunamadı" göster
            if (match == null)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // Maçı ekrana (View) gönder
            return View(match);
        }
    }
}