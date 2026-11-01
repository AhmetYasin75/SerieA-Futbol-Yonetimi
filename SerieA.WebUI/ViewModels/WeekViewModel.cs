namespace SerieA.WebUI.ViewModels
{
    // API'den gelen hafta bilgisini karşılayan sınıf (hafta seçici için)
    // Alan adları API'deki WeekDto ile aynı olmalı, yoksa JSON doğru dolmaz
    public class WeekViewModel
    {
        // Hafta numarası
        public int Week { get; set; }

        // Haftanın ilk ve son maç tarihi
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Haftadaki toplam maç ve biten maç sayısı
        public int MatchCount { get; set; }
        public int FinishedCount { get; set; }
    }
}