using Microsoft.AspNetCore.Mvc;
using SerieA.WebUI.ViewModels;

namespace SerieA.WebUI.Controllers
{
    // Admin panelinin controller'ı. Bu controller'ın tüm adresleri "/Admin" ile başlar
    [Route("Admin")]
    public class AdminController : Controller
    {
        // Program.cs'te tanıttığımız HttpClient'ları üreten fabrika
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Adres: /Admin  → doğrudan maç listesine yönlendirir
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Matches");
        }

        // Adres: /Admin/Matches  veya  /Admin/Matches?team=inter&week=2&status=Finished
        // Filtre parametreleri boş gelebilir ("?" işaretli olanlar), o zaman o filtre uygulanmaz
        [HttpGet("Matches")]
        public async Task<IActionResult> Matches(string? team, int? week, MatchStatus? status)
        {
            // API'ye bağlanacak client
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // 1) API'den tüm maçları çek
            var allMatches = await client.GetFromJsonAsync<List<MatchListViewModel>>("api/matches")
                             ?? new List<MatchListViewModel>();

            // 2) Filtreleri sırayla uygula (hepsi seçilmişse hepsi birlikte uygulanır)
            var filtered = allMatches.AsEnumerable();

            // Takım adı: ev sahibi ya da deplasman adında geçiyorsa (büyük/küçük harf fark etmez)
            if (!string.IsNullOrWhiteSpace(team))
            {
                var search = team.Trim();
                filtered = filtered.Where(m =>
                    m.HomeTeamName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    m.AwayTeamName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Hafta
            if (week.HasValue)
            {
                filtered = filtered.Where(m => m.Week == week.Value);
            }

            // Maç durumu
            if (status.HasValue)
            {
                filtered = filtered.Where(m => m.Status == status.Value);
            }

            // 3) Ekrana gidecek paketi hazırla
            var model = new AdminMatchesViewModel
            {
                Matches = filtered.ToList(),
                TotalCount = allMatches.Count,

                // "Hafta Seç" listesi: maçı olan haftalar, küçükten büyüğe
                Weeks = allMatches.Select(m => m.Week).Distinct().OrderBy(w => w).ToList(),

                // Seçilen filtreleri geri gönder, sayfa yenilenince kutularda kalsın
                Team = team,
                Week = week,
                Status = status
            };

            return View(model);
        }

        // Adres: /Admin/Matches/Create  → boş "Yeni Maç Ekle" formunu açar
        [HttpGet("Matches/Create")]
        public async Task<IActionResult> Create()
        {
            // Formdaki açılır listeler için takımları çek
            var model = new MatchFormViewModel
            {
                Teams = await GetTeamsAsync()
            };

            // Ekleme ve düzenleme aynı form ekranını (MatchForm) kullanır
            return View("MatchForm", model);
        }

        // Form gönderilince çalışır (butona basınca)
        [HttpPost("Matches/Create")]
        public async Task<IActionResult> Create(MatchFormViewModel model)
        {
            // Form hata ile geri dönerse açılır listeler boş kalmasın diye takımları tekrar çek
            model.Teams = await GetTeamsAsync();

            // 1) Önce kendi kontrollerimiz (hafta, takım seçimi, skor)
            var error = ValidateForm(model);
            if (error != null)
            {
                model.ErrorMessage = error;
                return View("MatchForm", model);
            }

            // 2) API'ye POST isteği at (API kendi iş kurallarını da kontrol eder)
            var client = _httpClientFactory.CreateClient("SerieAApi");
            var response = await client.PostAsJsonAsync("api/matches", BuildPayload(model));

            // 3) API hata döndürdüyse (400, 409) mesajı formda göster
            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = await ReadApiErrorAsync(response);
                return View("MatchForm", model);
            }

            // 4) Başarılı: bildirim mesajı bırak ve maç listesine dön
            TempData["Success"] = "Maç başarıyla eklendi.";
            return RedirectToAction("Matches");
        }

        // Adres: /Admin/Matches/Edit/5  → 5 numaralı maçın bilgileriyle dolu formu açar
        [HttpGet("Matches/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // Maçın mevcut bilgilerini API'den çek
            var response = await client.GetAsync($"api/matches/{id}");

            // Böyle bir maç yoksa (ya da silinmişse) 404 göster
            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            var match = await response.Content.ReadFromJsonAsync<MatchDetailViewModel>();
            if (match == null)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // API'den gelen bilgileri form modeline aktar (form bu değerlerle dolu açılır)
            var model = new MatchFormViewModel
            {
                Id = match.Id,
                Week = match.Week,
                HomeTeamId = match.HomeTeamId,
                AwayTeamId = match.AwayTeamId,
                MatchDate = match.MatchDate,
                Status = match.Status,
                Stadium = match.Stadium,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                Teams = await GetTeamsAsync()
            };

            return View("MatchForm", model);
        }

        // Düzenleme formu gönderilince çalışır ("Güncelle" butonuna basınca)
        [HttpPost("Matches/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, MatchFormViewModel model)
        {
            // Adresteki Id her zaman doğrudur, modele onu yaz
            model.Id = id;

            // Form hata ile geri dönerse açılır listeler boş kalmasın diye takımları tekrar çek
            model.Teams = await GetTeamsAsync();

            // 1) Önce kendi kontrollerimiz (hafta, takım seçimi, skor)
            var error = ValidateForm(model);
            if (error != null)
            {
                model.ErrorMessage = error;
                return View("MatchForm", model);
            }

            // 2) API'ye PUT isteği at (API kendi iş kurallarını da kontrol eder)
            var client = _httpClientFactory.CreateClient("SerieAApi");
            var response = await client.PutAsJsonAsync($"api/matches/{id}", BuildPayload(model));

            // 3) Maç bu arada silinmişse API 404 döner
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // 4) API başka bir hata döndürdüyse (400, 409) mesajı formda göster
            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = await ReadApiErrorAsync(response);
                return View("MatchForm", model);
            }

            // 5) Başarılı: bildirim mesajı bırak ve maç listesine dön
            TempData["Success"] = "Maç başarıyla güncellendi.";
            return RedirectToAction("Matches");
        }

        // Onay penceresindeki "Sil" butonuna basınca çalışır. Adres: /Admin/Matches/Delete/5
        [HttpPost("Matches/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("SerieAApi");

            // API'ye DELETE isteği at
            var response = await client.DeleteAsync($"api/matches/{id}");

            // Maç yoksa (ya da daha önce silinmişse) 404 göster
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // Başka bir sorun çıktıysa genel hata göster
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Maç silinirken bir hata oluştu.");
            }

            // Başarılı: bildirim mesajı bırak ve maç listesine dön
            TempData["Success"] = "Maç silindi.";
            return RedirectToAction("Matches");
        }

        // ===== Yardımcı metotlar (ekleme ve düzenleme formları bunları ortak kullanır) =====

        // API'den tüm takımları çeker (formdaki Ev Sahibi / Deplasman açılır listeleri için)
        private async Task<List<TeamViewModel>> GetTeamsAsync()
        {
            var client = _httpClientFactory.CreateClient("SerieAApi");
            return await client.GetFromJsonAsync<List<TeamViewModel>>("api/teams")
                   ?? new List<TeamViewModel>();
        }

        // Formu API'ye göndermeden önce kontrol eder. Sorun varsa hata mesajı döner, yoksa null döner
        private static string? ValidateForm(MatchFormViewModel model)
        {
            // Hafta seçilmemişse (0) ya da 1-38 dışındaysa
            if (model.Week < 1 || model.Week > 38)
            {
                return "Hafta 1 ile 38 arasında seçilmelidir.";
            }

            // Takımlardan biri seçilmemişse (0)
            if (model.HomeTeamId == 0 || model.AwayTeamId == 0)
            {
                return "Ev sahibi ve deplasman takımı seçilmelidir.";
            }

            // Canlı veya Tamamlandı maçta iki skor da girilmiş olmalı
            var needsScore = model.Status == MatchStatus.Live || model.Status == MatchStatus.Finished;
            if (needsScore && (model.HomeScore == null || model.AwayScore == null))
            {
                return "Canlı veya Tamamlandı durumunda iki skor da girilmelidir.";
            }

            return null;
        }

        // Formdaki bilgilerden API'ye gönderilecek paketi hazırlar (API'deki MatchSaveDto ile aynı alanlar)
        private static object BuildPayload(MatchFormViewModel model)
        {
            // Skorlar sadece Canlı ve Tamamlandı maçlarda geçerli, diğer durumlarda boş gider
            var needsScore = model.Status == MatchStatus.Live || model.Status == MatchStatus.Finished;

            // Stadyum boş bırakıldıysa ev sahibi takımın stadyumunu yaz
            var stadium = model.Stadium;
            if (string.IsNullOrWhiteSpace(stadium))
            {
                stadium = model.Teams.FirstOrDefault(t => t.Id == model.HomeTeamId)?.Stadium ?? "";
            }

            // Alan adları küçük harfle başlar (JSON kuralı), API bunları tanır
            return new
            {
                homeTeamId = model.HomeTeamId,
                awayTeamId = model.AwayTeamId,
                week = model.Week,
                matchDate = model.MatchDate,
                status = model.Status,
                stadium = stadium,
                homeScore = needsScore ? model.HomeScore : null,
                awayScore = needsScore ? model.AwayScore : null
            };
        }

        // API hata döndürdüğünde (400, 409) ekranda gösterilecek mesajı hazırlar
        private static async Task<string> ReadApiErrorAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            // Bizim yazdığımız kural mesajları düz yazıdır. "{" ile başlıyorsa API'nin otomatik
            // doğrulama hatasıdır (JSON), onu okunur bir mesajla değiştiriyoruz
            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
            {
                return "Girilen değerler geçersiz. Lütfen alanları kontrol edin.";
            }

            // Düz yazı mesajını olduğu gibi göster (varsa tırnakları kırp)
            return body.Trim().Trim('"');
        }
    }
}