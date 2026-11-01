namespace SerieA.WebUI.ViewModels
{
    // Haftalık fikstür sayfasının tüm verisini tek pakette toplayan sınıf
    public class FixturesViewModel
    {
        // Şu an ekranda gösterilen hafta
        public int SelectedWeek { get; set; }

        // Güncel hafta (hafta seçicide "Güncel" etiketi bunda görünecek)
        public int CurrentWeek { get; set; }

        // Hafta seçicideki tüm haftalar
        public List<WeekViewModel> Weeks { get; set; } = new();

        // Seçili haftanın maçları
        public List<MatchListViewModel> Matches { get; set; } = new();
    }
}