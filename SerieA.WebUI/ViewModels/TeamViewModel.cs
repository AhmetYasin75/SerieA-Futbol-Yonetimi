namespace SerieA.WebUI.ViewModels
{
    // API'den gelen takım bilgisini karşılayan sınıf (formlardaki takım listesi için)
    // Alan adları API'deki TeamDto ile aynı olmalı, yoksa JSON doğru dolmaz
    public class TeamViewModel
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

        // Takımın stadyumu (formda ev sahibi seçilince stadyum bundan otomatik dolacak)
        public string Stadium { get; set; } = string.Empty;
    }
}