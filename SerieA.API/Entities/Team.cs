namespace SerieA.API.Entities
{
    // Veritabanındaki "Teams" tablosunun C# karşılığı
    public class Team
    {
        // Birincil anahtar (Primary Key). EF Core "Id" adını görünce otomatik anahtar yapar
        public int Id { get; set; }

        // Takım adı (örn: Inter)
        public string Name { get; set; } = string.Empty;

        // Puan durumunda gösterilecek 3 harfli kısa kod (örn: INT)
        public string ShortName { get; set; } = string.Empty;

        // Takım logosunun adresi (internetteki resim linki)
        public string LogoUrl { get; set; } = string.Empty;

        // Takımın şehri (örn: Milano)
        public string City { get; set; } = string.Empty;

        // Takımın stadyumu (örn: San Siro)
        public string Stadium { get; set; } = string.Empty;
    }
}