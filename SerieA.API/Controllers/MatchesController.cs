using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerieA.API.Entities;
using SerieA.API.Context;
using SerieA.API.DTOs;

namespace SerieA.API.Controllers
{
    [ApiController]
    // Adres: /api/matches
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        // Veritabanı köprümüz
        private readonly SerieAContext _context;

        // SerieAContext buraya otomatik gelir (Dependency Injection)
        public MatchesController(SerieAContext context)
        {
            _context = context;
        }

        // GET /api/matches  → tüm maçları listeler (admin paneli için)
        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _context.Matches
                .OrderBy(m => m.Week)                // Önce hafta sırasına göre
                .ThenBy(m => m.MatchDate)            // Aynı haftada tarihe göre
                .ThenBy(m => m.Id)                   // Tarih de aynıysa Id'ye göre
                .Select(m => new MatchListDto        // Entity'yi DTO'ya çevir
                {
                    Id = m.Id,
                    Week = m.Week,
                    MatchDate = m.MatchDate,
                    Status = m.Status,
                    Stadium = m.Stadium,
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam.Name,
                    HomeTeamShortName = m.HomeTeam.ShortName,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam.Name,
                    AwayTeamShortName = m.AwayTeam.ShortName,
                    HomeScore = m.HomeScore,
                    AwayScore = m.AwayScore
                })
                .ToListAsync();

            // Hiç maç olmasa bile hata değil, boş liste (200 OK) döner
            return Ok(matches);
        }

        // GET /api/matches/week/1  → 1. haftanın maçlarını listeler
        [HttpGet("week/{week}")]
        public async Task<IActionResult> GetMatchesByWeek(int week)
        {
            var matches = await _context.Matches
                .Where(m => m.Week == week)          // Sadece istenen haftanın maçları
                .OrderBy(m => m.MatchDate)           // Tarihe göre erkenden geçe doğru sırala
                .Select(m => new MatchListDto        // Entity'yi DTO'ya çevir
                {
                    Id = m.Id,
                    Week = m.Week,
                    MatchDate = m.MatchDate,
                    Status = m.Status,
                    Stadium = m.Stadium,
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam.Name,
                    HomeTeamShortName = m.HomeTeam.ShortName,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam.Name,
                    AwayTeamShortName = m.AwayTeam.ShortName,
                    HomeScore = m.HomeScore,
                    AwayScore = m.AwayScore
                })
                .ToListAsync();

            // O haftada hiç maç yoksa 404 Not Found döndür
            if (matches.Count == 0)
            {
                return NotFound($"{week}. hafta için maç bulunamadı.");
            }

            // Maç varsa 200 OK ile listeyi döndür
            return Ok(matches);
        }

        // GET /api/matches/weeks  → hafta seçici için haftaların listesini verir
        [HttpGet("weeks")]
        public async Task<IActionResult> GetWeeks()
        {
            var weeks = await _context.Matches
                .GroupBy(m => m.Week)                     // Maçları hafta numarasına göre grupla
                .OrderBy(g => g.Key)                      // Hafta numarasına göre sırala (g.Key = hafta no)
                .Select(g => new WeekDto
                {
                    Week = g.Key,
                    StartDate = g.Min(m => m.MatchDate),  // Haftanın en erken maçı
                    EndDate = g.Max(m => m.MatchDate),    // Haftanın en geç maçı
                    MatchCount = g.Count(),               // Haftadaki toplam maç
                    FinishedCount = g.Count(m => m.Status == MatchStatus.Finished) // Biten maç sayısı
                })
                .ToListAsync();

            return Ok(weeks);
        }

        // GET /api/matches/1  → tek bir maçın tüm detayını verir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _context.Matches
                .Where(m => m.Id == id)              // Sadece istenen maç
                .Select(m => new MatchDetailDto      // Entity'yi DTO'ya çevir
                {
                    Id = m.Id,
                    Week = m.Week,
                    MatchDate = m.MatchDate,
                    Status = m.Status,
                    Stadium = m.Stadium,
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam.Name,
                    HomeTeamShortName = m.HomeTeam.ShortName,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam.Name,
                    AwayTeamShortName = m.AwayTeam.ShortName,
                    HomeScore = m.HomeScore,
                    AwayScore = m.AwayScore,

                    // Goller dakikaya göre sıralı gelir
                    Goals = m.Goals
                        .OrderBy(g => g.Minute)
                        .Select(g => new MatchGoalDto
                        {
                            TeamId = g.TeamId,
                            PlayerName = g.PlayerName,
                            Minute = g.Minute
                        }).ToList(),

                    // Kartlar dakikaya göre sıralı gelir
                    Cards = m.Cards
                        .OrderBy(c => c.Minute)
                        .Select(c => new MatchCardDto
                        {
                            TeamId = c.TeamId,
                            PlayerName = c.PlayerName,
                            Minute = c.Minute,
                            CardType = c.CardType
                        }).ToList(),

                    // Oyuncu değişiklikleri dakikaya göre sıralı gelir
                    Substitutions = m.Substitutions
                        .OrderBy(s => s.Minute)
                        .Select(s => new SubstitutionDto
                        {
                            TeamId = s.TeamId,
                            PlayerIn = s.PlayerIn,
                            PlayerOut = s.PlayerOut,
                            Minute = s.Minute
                        }).ToList()
                })
                .FirstOrDefaultAsync();              // İlk kaydı al, yoksa null döner

            // Maç bulunamadıysa 404 Not Found döndür
            if (match == null)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // Maç varsa 200 OK ile detayı döndür
            return Ok(match);
        }

        // POST /api/matches  → yeni maç ekler
        [HttpPost]
        public async Task<IActionResult> CreateMatch(MatchSaveDto dto)
        {
            // Önce iş kurallarını kontrol et (400 veya 409 dönebilir)
            var error = await ValidateMatchAsync(dto);
            if (error != null)
            {
                return error;
            }

            // Kurallar tamamsa DTO'dan yeni bir Match entity'si oluştur
            var match = new Match
            {
                HomeTeamId = dto.HomeTeamId,
                AwayTeamId = dto.AwayTeamId,
                Week = dto.Week,
                MatchDate = dto.MatchDate,
                Status = dto.Status,
                Stadium = dto.Stadium,
                HomeScore = dto.HomeScore,
                AwayScore = dto.AwayScore
            };

            // Veritabanına ekle ve kaydet (Id'yi veritabanı verir)
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // 201 Created döndür. Yeni maçın detayına giden adresi de cevaba ekler
            return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, new { id = match.Id });
        }

        // Maç eklerken ve düzenlerken kullanılacak iş kuralı kontrolü
        // Sorun yoksa null döner, sorun varsa uygun HTTP cevabını (400/409) döner
        // excludeMatchId: düzenlemede maçın KENDİSİNİ "başka maç" saymamak için kullanılır
        private async Task<IActionResult?> ValidateMatchAsync(MatchSaveDto dto, int excludeMatchId = 0)
        {
            // Kural 1: Ev sahibi ve deplasman aynı takım olamaz
            if (dto.HomeTeamId == dto.AwayTeamId)
            {
                return BadRequest("Ev sahibi ve deplasman takımı aynı olamaz.");
            }

            // Kural 2: İki takım da veritabanında var mı?
            var teamCount = await _context.Teams
                .CountAsync(t => t.Id == dto.HomeTeamId || t.Id == dto.AwayTeamId);

            if (teamCount != 2)
            {
                return BadRequest("Ev sahibi veya deplasman takımı bulunamadı.");
            }

            // Kural 3: Bu takımlardan biri aynı haftada başka bir maçta var mı?
            var teamAlreadyPlays = await _context.Matches.AnyAsync(m =>
                m.Week == dto.Week
                && m.Id != excludeMatchId
                && (m.HomeTeamId == dto.HomeTeamId || m.AwayTeamId == dto.HomeTeamId
                    || m.HomeTeamId == dto.AwayTeamId || m.AwayTeamId == dto.AwayTeamId));

            if (teamAlreadyPlays)
            {
                return Conflict($"{dto.Week}. haftada takımlardan biri zaten başka bir maçta yer alıyor.");
            }

            // Kural 4: Tamamlandı durumundaki maçın skoru boş olamaz
            if (dto.Status == MatchStatus.Finished && (dto.HomeScore == null || dto.AwayScore == null))
            {
                return BadRequest("Tamamlandı durumundaki maçın skoru girilmelidir.");
            }

            // Hiçbir kural bozulmadı
            return null;
        }

        // PUT /api/matches/5  → var olan maçı günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, MatchSaveDto dto)
        {
            // Güncellenecek maçı veritabanında bul
            var match = await _context.Matches.FindAsync(id);

            // Maç yoksa 404 Not Found döndür
            if (match == null)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // İş kurallarını kontrol et. Maçın kendisini "başka maç" saymamak için id'yi veriyoruz
            var error = await ValidateMatchAsync(dto, id);
            if (error != null)
            {
                return error;
            }

            // Mevcut maçın alanlarını yeni değerlerle güncelle
            match.HomeTeamId = dto.HomeTeamId;
            match.AwayTeamId = dto.AwayTeamId;
            match.Week = dto.Week;
            match.MatchDate = dto.MatchDate;
            match.Status = dto.Status;
            match.Stadium = dto.Stadium;
            match.HomeScore = dto.HomeScore;
            match.AwayScore = dto.AwayScore;

            // Değişiklikleri veritabanına kaydet
            await _context.SaveChangesAsync();

            // 204 No Content: işlem başarılı, geri döndürecek veri yok
            return NoContent();
        }

        // DELETE /api/matches/5  → maçı siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            // Silinecek maçı veritabanında bul
            var match = await _context.Matches.FindAsync(id);

            // Maç yoksa (hiç olmadı ya da daha önce silindi) 404 Not Found döndür
            if (match == null)
            {
                return NotFound($"{id} numaralı maç bulunamadı.");
            }

            // Maçı sil. Maça bağlı gol, kart ve oyuncu değişiklikleri de otomatik silinir (Cascade)
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            // 204 No Content: işlem başarılı, geri döndürecek veri yok
            return NoContent();
        }
    }
}