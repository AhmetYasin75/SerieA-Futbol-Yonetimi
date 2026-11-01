namespace SerieA.WebUI.ViewModels
{
    // API'den gelen puan durumu satırını karşılayan sınıf
    // Alan adları API'deki StandingDto ile aynı olmalı, yoksa JSON doğru dolmaz
    public class StandingViewModel
    {
        // Tablodaki sırası
        public int Position { get; set; }

        // Takım bilgileri
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        // Oynanan, galibiyet, beraberlik, mağlubiyet
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }

        // Atılan gol, yenilen gol, averaj
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }

        // Puan
        public int Points { get; set; }
    }
}