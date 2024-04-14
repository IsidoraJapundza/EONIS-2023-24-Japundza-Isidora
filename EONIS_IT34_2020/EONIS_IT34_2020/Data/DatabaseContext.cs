using EONIS_IT34_2020.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EONIS_IT34_2020.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<Administrator> Administratori { get; set; }
        public DbSet<Dogadjaj> Dogadjaji { get; set; }
        public DbSet<KontigentKarata> KontigentiKarata { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Porudzbina> Porudzbine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("Popust"));
        }
    }
}