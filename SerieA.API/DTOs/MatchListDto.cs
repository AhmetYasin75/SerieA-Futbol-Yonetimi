using SerieA.API.Entities;

namespace SerieA.API.DTOs
{
    // Fikstür listesinde her maç satırı için dışarıya verilen bilgi
    // Entity'deki Id'ler yerine ekranda gösterilecek takım adları da burada hazır gelir
    public class MatchListDto
    {
        public int Id { get; set; }

        // Maçın haftası
        public int Week { get; set; }

        // Maçın tarihi ve saati
        public DateTime MatchDate { get; set; }

        // Maçın durumu (Başlamadı / Canlı / Tamamlandı / Ertelendi)
        public MatchStatus Status { get; set; }

        // Maçın oynandığı stadyum
        public string Stadium { get; set; } = string.Empty;

        // Ev sahibi takım bilgileri
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string HomeTeamShortName { get; set; } = string.Empty;

        // Deplasman takımı bilgileri
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public string AwayTeamShortName { get; set; } = string.Empty;

        // Skorlar. "int?" = oynanmamış maçta boş (null) olabilir
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}