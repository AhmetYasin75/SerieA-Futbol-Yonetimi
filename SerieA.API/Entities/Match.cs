namespace SerieA.API.Entities
{
    // Veritabanındaki "Matches" tablosunun C# karşılığı
    public class Match
    {
        // Birincil anahtar
        public int Id { get; set; }

        // Ev sahibi takımın Id'si (Teams tablosuna yabancı anahtar / Foreign Key)
        public int HomeTeamId { get; set; }

        // Deplasman takımının Id'si (Teams tablosuna yabancı anahtar / Foreign Key)
        public int AwayTeamId { get; set; }

        // Maçın oynandığı hafta (1, 2, 3 ...)
        public int Week { get; set; }

        // Maçın tarihi ve saati (ikisi tek alanda tutulur)
        public DateTime MatchDate { get; set; }

        // Ev sahibi golü. "int?" = boş (null) olabilir, çünkü oynanmamış maçın skoru yoktur
        public int? HomeScore { get; set; }

        // Deplasman golü. Aynı şekilde oynanmamış maçta boş olabilir
        public int? AwayScore { get; set; }

        // Maçın durumu (Başlamadı / Canlı / Tamamlandı / Ertelendi)
        public MatchStatus Status { get; set; }

        // Maçın oynandığı stadyum
        public string Stadium { get; set; } = string.Empty;

        // Navigation property: Id yerine takımın kendisine ulaşmamızı sağlar
        // Örn: match.HomeTeam.Name  → "Inter"
        public Team HomeTeam { get; set; } = null!;
        public Team AwayTeam { get; set; } = null!;

        // Bir maçın BİRDEN ÇOK golü, kartı ve oyuncu değişikliği olabilir (bire-çok ilişki)
        // Bu listeler sayesinde match.Goals diyerek o maçın tüm gollerine ulaşabiliriz
        public List<MatchGoal> Goals { get; set; } = new();
        public List<MatchCard> Cards { get; set; } = new();
        public List<Substitution> Substitutions { get; set; } = new();
    }
}