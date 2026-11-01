using SerieA.API.Entities;

namespace SerieA.API.DTOs
{
    // Maç detay ekranı için dışarıya verilen tam paket
    public class MatchDetailDto
    {
        public int Id { get; set; }
        public int Week { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
        public string Stadium { get; set; } = string.Empty;

        // Ev sahibi takım
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string HomeTeamShortName { get; set; } = string.Empty;

        // Deplasman takımı
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public string AwayTeamShortName { get; set; } = string.Empty;

        // Skorlar (oynanmamış maçta boş olabilir)
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        // Maçın golleri, kartları ve oyuncu değişiklikleri (dakikaya göre sıralı gelecek)
        public List<MatchGoalDto> Goals { get; set; } = new();
        public List<MatchCardDto> Cards { get; set; } = new();
        public List<SubstitutionDto> Substitutions { get; set; } = new();
    }

    // Tek bir gol
    public class MatchGoalDto
    {
        // Golü atan takımın Id'si. Ekranda golün sol mu sağ mı tarafta görüneceğini buradan anlayacağız
        public int TeamId { get; set; }

        // Golü atan oyuncunun adı
        public string PlayerName { get; set; } = string.Empty;

        // Golün dakikası
        public int Minute { get; set; }
    }

    // Tek bir kart
    public class MatchCardDto
    {
        public int TeamId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Minute { get; set; }

        // Kartın türü (Sarı / Kırmızı)
        public CardType CardType { get; set; }
    }

    // Tek bir oyuncu değişikliği
    public class SubstitutionDto
    {
        public int TeamId { get; set; }

        // Oyuna giren oyuncu
        public string PlayerIn { get; set; } = string.Empty;

        // Oyundan çıkan oyuncu
        public string PlayerOut { get; set; } = string.Empty;

        public int Minute { get; set; }
    }
}