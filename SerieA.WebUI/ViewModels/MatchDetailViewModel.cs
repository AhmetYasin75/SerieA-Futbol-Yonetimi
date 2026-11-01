namespace SerieA.WebUI.ViewModels
{
    // Maç detay sayfasının tüm verisini karşılayan sınıf
    // Alan adları API'deki MatchDetailDto ile aynı olmalı, yoksa JSON doğru dolmaz
    public class MatchDetailViewModel
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

        // Maçın golleri, kartları ve oyuncu değişiklikleri (API dakikaya göre sıralı verir)
        public List<MatchGoalViewModel> Goals { get; set; } = new();
        public List<MatchCardViewModel> Cards { get; set; } = new();
        public List<SubstitutionViewModel> Substitutions { get; set; } = new();
    }

    // Tek bir gol
    public class MatchGoalViewModel
    {
        // Golü atan takımın Id'si (golün hangi takıma ait olduğunu buradan anlarız)
        public int TeamId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Minute { get; set; }
    }

    // Tek bir kart
    public class MatchCardViewModel
    {
        public int TeamId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Minute { get; set; }

        // Kartın türü (Sarı / Kırmızı)
        public CardType CardType { get; set; }
    }

    // Tek bir oyuncu değişikliği
    public class SubstitutionViewModel
    {
        public int TeamId { get; set; }

        // Oyuna giren ve oyundan çıkan oyuncu
        public string PlayerIn { get; set; } = string.Empty;
        public string PlayerOut { get; set; } = string.Empty;
        public int Minute { get; set; }
    }

    // API'deki CardType enum'unun WebUI'daki kopyası (sıra ve değerler aynı olmalı)
    public enum CardType
    {
        // Sarı kart (0)
        Yellow = 0,

        // Kırmızı kart (1)
        Red = 1
    }
}