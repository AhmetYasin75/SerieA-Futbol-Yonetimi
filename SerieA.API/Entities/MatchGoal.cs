namespace SerieA.API.Entities
{
    // Veritabanındaki "MatchGoals" tablosunun C# karşılığı
    // Her satır, bir maçta atılan tek bir golü temsil eder
    public class MatchGoal
    {
        // Birincil anahtar
        public int Id { get; set; }

        // Golün atıldığı maçın Id'si (Matches tablosuna yabancı anahtar)
        public int MatchId { get; set; }

        // Golü atan takımın Id'si (Teams tablosuna yabancı anahtar)
        public int TeamId { get; set; }

        // Golü atan oyuncunun adı (case'te oyuncu tablosu yok, isim yazı olarak tutulur)
        public string PlayerName { get; set; } = string.Empty;

        // Golün atıldığı dakika (örn: 34)
        public int Minute { get; set; }

        // Navigation property'ler: Id yerine maça ve takıma doğrudan ulaşmamızı sağlar
        public Match Match { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}