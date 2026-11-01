using System.ComponentModel.DataAnnotations;
using SerieA.API.Entities;

namespace SerieA.API.DTOs
{
    // Maç EKLERKEN (POST) ve DÜZENLERKEN (PUT) dışarıdan gelen veri paketi
    // Id yok: eklemede Id'yi veritabanı verir, düzenlemede Id adresten (/api/matches/5) gelir
    public class MatchSaveDto
    {
        // Ev sahibi takımın Id'si
        [Required]
        public int HomeTeamId { get; set; }

        // Deplasman takımının Id'si
        [Required]
        public int AwayTeamId { get; set; }

        // Hafta numarası. Range: 1 ile 38 arası değer kabul eder, dışındakini otomatik reddeder
        [Range(1, 38, ErrorMessage = "Hafta 1 ile 38 arasında olmalıdır.")]
        public int Week { get; set; }

        // Maçın tarihi ve saati
        [Required]
        public DateTime MatchDate { get; set; }

        // Maçın durumu (Başlamadı / Canlı / Tamamlandı / Ertelendi)
        public MatchStatus Status { get; set; }

        // Maçın oynandığı stadyum
        public string Stadium { get; set; } = string.Empty;

        // Skorlar. Oynanmamış maçta boş (null) bırakılabilir
        // Range: negatif skor girilmesini engeller
        [Range(0, 99, ErrorMessage = "Skor 0 ile 99 arasında olmalıdır.")]
        public int? HomeScore { get; set; }

        [Range(0, 99, ErrorMessage = "Skor 0 ile 99 arasında olmalıdır.")]
        public int? AwayScore { get; set; }
    }
}