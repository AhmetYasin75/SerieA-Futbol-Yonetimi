using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerieA.API.Context;
using SerieA.API.DTOs;

namespace SerieA.API.Controllers
{
    // Bu sınıfın bir API controller'ı olduğunu belirtir
    [ApiController]
    // Adres: /api/teams  ("[controller]" kelimesi "Teams" olarak okunur)
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        // Veritabanı köprümüz
        private readonly SerieAContext _context;

        // Program.cs'te tanıttığımız SerieAContext, buraya otomatik gelir (Dependency Injection)
        public TeamsController(SerieAContext context)
        {
            _context = context;
        }

        // GET /api/teams  → tüm takımları listeler
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams
                .OrderBy(t => t.Name)              // Takımları ada göre A'dan Z'ye sırala
                .Select(t => new TeamDto           // Entity'yi DTO'ya çevir
                {
                    Id = t.Id,
                    Name = t.Name,
                    ShortName = t.ShortName,
                    LogoUrl = t.LogoUrl,
                    City = t.City,
                    Stadium = t.Stadium
                })
                .ToListAsync();                    // Veritabanından listeyi çek

            // 200 OK ile birlikte takım listesini döndür
            return Ok(teams);
        }
    }
}