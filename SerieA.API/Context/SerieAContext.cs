using Microsoft.EntityFrameworkCore;
using SerieA.API.Entities;

namespace SerieA.API.Context
{
    // Veritabanı köprümüz. DbContext sınıfından miras alır
    public class SerieAContext : DbContext
    {
        // Bağlantı ayarlarını (connection string) Program.cs'ten alır
        public SerieAContext(DbContextOptions<SerieAContext> options) : base(options)
        {
        }

        // Her DbSet, veritabanında bir tablo olur
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchGoal> MatchGoals { get; set; }
        public DbSet<MatchCard> MatchCards { get; set; }
        public DbSet<Substitution> Substitutions { get; set; }

        // Tablolar arası ilişkilerin ayrıntılı ayarları burada yapılır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Match tablosunun Teams tablosuna İKİ bağlantısı var (ev sahibi ve deplasman)
            // Restrict: Maçı olan bir takım silinmeye çalışılırsa silmeye izin verme
            // (SQL Server'da iki bağlantı + otomatik silme "multiple cascade paths" hatası verir)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gol, kart ve değişiklik kayıtları takıma bağlı ama takım silinince
            // maç olayları sessizce silinmesin diye burada da Restrict kullanıyoruz
            modelBuilder.Entity<MatchGoal>()
                .HasOne(g => g.Team)
                .WithMany()
                .HasForeignKey(g => g.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchCard>()
                .HasOne(c => c.Team)
                .WithMany()
                .HasForeignKey(c => c.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Substitution>()
                .HasOne(s => s.Team)
                .WithMany()
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Maç silinirse o maçın golleri, kartları ve değişiklikleri de silinsin
            // (Match -> Goals/Cards/Substitutions ilişkisi varsayılan olarak Cascade'dir,
            // yani ayrıca bir şey yazmamıza gerek yok)
        }
    }
}