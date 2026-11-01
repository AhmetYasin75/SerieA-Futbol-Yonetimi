namespace SerieA.WebUI.ViewModels
{
    // API'deki MatchStatus enum'unun WebUI'daki kopyası
    // Sıra ve sayı değerleri API'dekiyle BİREBİR aynı olmalı, yoksa durumlar karışır
    public enum MatchStatus
    {
        // Maç henüz oynanmadı (0)
        NotStarted = 0,

        // Maç şu an oynanıyor (1)
        Live = 1,

        // Maç tamamlandı (2)
        Finished = 2,

        // Maç ertelendi (3)
        Postponed = 3
    }
}