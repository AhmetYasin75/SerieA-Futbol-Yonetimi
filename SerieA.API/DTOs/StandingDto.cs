namespace SerieA.API.DTOs
{
    // Puan durumu tablosunun tek bir satırı (bir takımın istatistikleri)
    // Bu bilgiler veritabanında SAKLANMAZ, tamamlanan maçlardan anlık hesaplanır
    public class StandingDto
    {
        // Tablodaki sırası (1, 2, 3 ...)
        public int Position { get; set; }

        // Takım bilgileri
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        // Oynanan maç sayısı (O)
        public int Played { get; set; }

        // Galibiyet (G), Beraberlik (B), Mağlubiyet (M)
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }

        // Atılan gol (AG) ve yenilen gol (YG)
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        // Averaj = Atılan Gol - Yenilen Gol (case'in kuralı)
        public int GoalDifference { get; set; }

        // Puan: galibiyet 3, beraberlik 1, mağlubiyet 0
        public int Points { get; set; }
    }
}