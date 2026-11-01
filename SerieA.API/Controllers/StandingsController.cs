using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerieA.API.Context;
using SerieA.API.DTOs;
using SerieA.API.Entities;

namespace SerieA.API.Controllers
{
    [ApiController]
    // Adres: /api/standings
    [Route("api/[controller]")]
    public class StandingsController : ControllerBase
    {
        // Veritabanı köprümüz
        private readonly SerieAContext _context;

        public StandingsController(SerieAContext context)
        {
            _context = context;
        }

        // GET /api/standings  → puan durumunu anlık hesaplayıp verir
        [HttpGet]
        public async Task<IActionResult> GetStandings()
        {
            // 1) Tüm takımları çek
            var teams = await _context.Teams.ToListAsync();

            // 2) Sadece TAMAMLANMIŞ maçları çek (case'in kuralı)
            // Skorları boş olan maçı da dışarıda bırakıyoruz, hesaplamada hata çıkmasın
            var matches = await _context.Matches
                .Where(m => m.Status == MatchStatus.Finished
                         && m.HomeScore != null
                         && m.AwayScore != null)
                .ToListAsync();

            // 3) Her takım için sıfırlanmış bir tablo satırı hazırla
            var standings = teams.Select(t => new StandingDto
            {
                TeamId = t.Id,
                TeamName = t.Name,
                ShortName = t.ShortName
            }).ToList();

            // 4) Maçları tek tek gez ve iki takımın satırını güncelle
            foreach (var match in matches)
            {
                // Bu maçtaki ev sahibi ve deplasman takımının tablo satırlarını bul
                var home = standings.First(s => s.TeamId == match.HomeTeamId);
                var away = standings.First(s => s.TeamId == match.AwayTeamId);

                // Skorlar int? (boş olabilir) tipinde, ".Value" ile gerçek sayıyı alıyoruz
                int homeGoals = match.HomeScore!.Value;
                int awayGoals = match.AwayScore!.Value;

                // İki takım da bir maç oynadı
                home.Played++;
                away.Played++;

                // Atılan ve yenilen golleri ekle
                home.GoalsFor += homeGoals;
                home.GoalsAgainst += awayGoals;
                away.GoalsFor += awayGoals;
                away.GoalsAgainst += homeGoals;

                if (homeGoals > awayGoals)
                {
                    // Ev sahibi kazandı: 3 puan
                    home.Won++;
                    home.Points += 3;
                    away.Lost++;
                }
                else if (homeGoals < awayGoals)
                {
                    // Deplasman kazandı: 3 puan
                    away.Won++;
                    away.Points += 3;
                    home.Lost++;
                }
                else
                {
                    // Beraberlik: ikisine de 1 puan
                    home.Drawn++;
                    away.Drawn++;
                    home.Points += 1;
                    away.Points += 1;
                }
            }

            // 5) Averaj = Atılan Gol - Yenilen Gol
            foreach (var s in standings)
            {
                s.GoalDifference = s.GoalsFor - s.GoalsAgainst;
            }

            // 6) Sıralama: önce Puan, eşitse Averaj, o da eşitse Atılan Gol
            // Hepsi eşitse takım adına göre sıralıyoruz ki sıralama her seferinde aynı kalsın
            var sorted = standings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenByDescending(s => s.GoalsFor)
                .ThenBy(s => s.TeamName)
                .ToList();

            // 7) Sıra numaralarını ver (1, 2, 3 ...)
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Position = i + 1;
            }

            return Ok(sorted);
        }
    }
}