namespace SerieA.API.DTOs
{
    // Hafta seçicide gösterilecek tek bir haftanın özet bilgisi
    public class WeekDto
    {
        // Hafta numarası (1, 2, 3 ...)
        public int Week { get; set; }

        // O haftanın ilk maçının tarihi (hafta aralığının başı)
        public DateTime StartDate { get; set; }

        // O haftanın son maçının tarihi (hafta aralığının sonu)
        public DateTime EndDate { get; set; }

        // O haftadaki toplam maç sayısı
        public int MatchCount { get; set; }

        // O haftadaki tamamlanan maç sayısı (güncel haftayı bulmak için kullanacağız)
        public int FinishedCount { get; set; }
    }
}