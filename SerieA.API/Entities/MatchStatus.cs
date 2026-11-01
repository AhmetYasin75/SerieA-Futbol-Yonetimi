namespace SerieA.API.Entities
{
    // Maçın durumunu tutan enum. Sadece aşağıdaki 4 değerden biri olabilir.
    // Veritabanında sayı olarak saklanır: 0, 1, 2, 3
    public enum MatchStatus
    {
        // Maç henüz oynanmadı (0)
        NotStarted = 0,

        // Maç şu an oynanıyor (1)
        Live = 1,

        // Maç tamamlandı (2). Puan durumuna SADECE bu durumdaki maçlar girer
        Finished = 2,

        // Maç ertelendi (3)
        Postponed = 3
    }
}