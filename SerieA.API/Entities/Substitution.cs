namespace SerieA.API.Entities
{
    // Veritabanındaki "Substitutions" tablosunun C# karşılığı
    // Her satır, bir maçta yapılan tek bir oyuncu değişikliğini temsil eder
    public class Substitution
    {
        // Birincil anahtar
        public int Id { get; set; }

        // Değişikliğin yapıldığı maçın Id'si (Matches tablosuna yabancı anahtar)
        public int MatchId { get; set; }

        // Değişikliği yapan takımın Id'si (Teams tablosuna yabancı anahtar)
        public int TeamId { get; set; }

        // Oyuna giren oyuncunun adı
        public string PlayerIn { get; set; } = string.Empty;

        // Oyundan çıkan oyuncunun adı
        public string PlayerOut { get; set; } = string.Empty;

        // Değişikliğin yapıldığı dakika
        public int Minute { get; set; }

        // Navigation property'ler: maça ve takıma doğrudan ulaşmamızı sağlar
        public Match Match { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}