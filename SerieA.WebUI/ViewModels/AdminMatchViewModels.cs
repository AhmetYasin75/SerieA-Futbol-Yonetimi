namespace SerieA.WebUI.ViewModels
{
    // Admin maç LİSTESİ ekranının verisi: tablo + filtre değerleri
    public class AdminMatchesViewModel
    {
        // Filtrelenmiş maçlar (tabloda gösterilecek olanlar)
        public List<MatchListViewModel> Matches { get; set; } = new();

        // Filtre uygulanmadan önceki toplam maç sayısı ("30 kayıttan 10 tanesi gösteriliyor" için)
        public int TotalCount { get; set; }

        // "Hafta Seç" listesinde gösterilecek hafta numaraları
        public List<int> Weeks { get; set; } = new();

        // Kullanıcının seçtiği filtreler (sayfa yenilenince kutularda kalsın diye geri gönderiyoruz)
        public string? Team { get; set; }
        public int? Week { get; set; }
        public MatchStatus? Status { get; set; }
    }

    // Admin maç FORMUNUN verisi: hem "Yeni Maç Ekle" hem "Maçı Düzenle" için kullanılır
    public class MatchFormViewModel
    {
        // Düzenlemede maçın Id'si. Eklemede 0 kalır
        public int Id { get; set; }

        // Formdaki alanlar (API'deki MatchSaveDto ile aynı bilgiler)
        public int Week { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        // Tarih ve saat tek alanda tutulur (tarayıcıdaki "datetime-local" kutusu)
        public DateTime MatchDate { get; set; } = DateTime.Today.AddHours(20);

        public MatchStatus Status { get; set; }

        // "?" = boş bırakılabilir. Boşsa ev sahibi takımın stadyumu otomatik yazılır
        public string? Stadium { get; set; }

        // Skorlar sadece Canlı ve Tamamlandı durumlarında girilir
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        // Aşağıdakiler formdan GELMEZ, formu çizmek için Controller tarafından doldurulur:

        // Ev sahibi ve deplasman açılır listelerindeki takımlar
        public List<TeamViewModel> Teams { get; set; } = new();

        // API'nin döndürdüğü hata mesajı (örn: "Ev sahibi ve deplasman takımı aynı olamaz.")
        public string? ErrorMessage { get; set; }
    }
}