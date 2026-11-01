using Microsoft.AspNetCore.Mvc;
using SerieA.WebUI.ViewModels;

namespace SerieA.WebUI.Controllers
{
    // Haftalık fikstür sayfasının controller'ı. Adres: /Fixtures veya /Fixtures?week=2
    public class FixturesController : Controller
    {
        // Program.cs'te tanıttığımız HttpClient'ları üreten fabrika
        private readonly IHttpClientFactory _httpClientFactory;

        public FixturesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // "int? week": adreste ?week=2 yazıyorsa 2 gelir, yazmıyorsa null gelir
        public async Task<IActionResult> Index(int? week)
        {
            // API'ye bağlanacak client (adresi Program.cs'ten hazır gelir)
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // 1) Hafta seçici için tüm haftaları çek
            var weeks = await client.GetFromJsonAsync<List<WeekViewModel>>("api/matches/weeks")
                        ?? new List<WeekViewModel>();

            // 2) Güncel haftayı bul: en az bir maçı bitmiş olan en son hafta
            // Hiçbir maç bitmemişse ilk hafta, hiç hafta yoksa 1 kabul edilir
            var currentWeek = weeks.Any(w => w.FinishedCount > 0)
                ? weeks.Where(w => w.FinishedCount > 0).Max(w => w.Week)
                : (weeks.Count > 0 ? weeks.Min(w => w.Week) : 1);

            // 3) Gösterilecek hafta: adreste yazıyorsa o, yazmıyorsa güncel hafta
            var selectedWeek = week ?? currentWeek;

            // 4) Seçili haftanın maçlarını çek
            var matches = new List<MatchListViewModel>();
            var response = await client.GetAsync($"api/matches/week/{selectedWeek}");

            // Sadece başarılıysa (200) listeyi oku. 404 gelirse (o hafta maç yok) liste boş kalır
            if (response.IsSuccessStatusCode)
            {
                matches = await response.Content.ReadFromJsonAsync<List<MatchListViewModel>>()
                          ?? new List<MatchListViewModel>();
            }

            // 5) Hepsini tek pakette ekrana (View) gönder
            var model = new FixturesViewModel
            {
                SelectedWeek = selectedWeek,
                CurrentWeek = currentWeek,
                Weeks = weeks,
                Matches = matches
            };

            return View(model);
        }
    }
}