namespace SerieA.API.Entities
{
    // Veritabanındaki "MatchCards" tablosunun C# karşılığı
    // Her satır, bir maçta gösterilen tek bir kartı temsil eder
    public class MatchCard
    {
        // Birincil anahtar
        public int Id { get; set; }

        // Kartın gösterildiği maçın Id'si (Matches tablosuna yabancı anahtar)
        public int MatchId { get; set; }

        // Kartı gören oyuncunun takımının Id'si (Teams tablosuna yabancı anahtar)
        public int TeamId { get; set; }

        // Kartı gören oyuncunun adı (oyuncu tablosu yok, isim yazı olarak tutulur)
        public string PlayerName { get; set; } = string.Empty;

        // Kartın gösterildiği dakika
        public int Minute { get; set; }

        // Kartın türü (Sarı / Kırmızı)
        public CardType CardType { get; set; }

        // Navigation property'ler: maça ve takıma doğrudan ulaşmamızı sağlar
        public Match Match { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}