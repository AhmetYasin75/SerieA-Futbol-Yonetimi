namespace SerieA.API.DTOs
{
    // API'nin dışarıya verdiği takım bilgisi
    // Entity'den farkı: ilişkili listeler yok, sadece ekranın ihtiyacı olan alanlar var
    public class TeamDto
    {
        public int Id { get; set; }

        // Takım adı (örn: Inter)
        public string Name { get; set; } = string.Empty;

        // 3 harfli kısa kod (örn: INT)
        public string ShortName { get; set; } = string.Empty;

        // Logo adresi
        public string LogoUrl { get; set; } = string.Empty;

        // Takımın şehri
        public string City { get; set; } = string.Empty;

        // Takımın stadyumu
        public string Stadium { get; set; } = string.Empty;
    }
}