using EONIS_IT34_2020.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EONIS_IT34_2020.Data
{
    public class DatabaseContextDB : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly static int iterations = 1000;

        public DatabaseContextDB(DbContextOptions<DatabaseContextDB> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Dogadjaj> Dogadjaj { get; set; }
        public DbSet<KontigentKarata> KontigentKarata { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Porudzbina> Porudzbina { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik", "EONIS_IT34_2020");
            modelBuilder.Entity<Administrator>().ToTable("Administrator", "EONIS_IT34_2020");
            modelBuilder.Entity<Dogadjaj>().ToTable("Dogadjaj", "EONIS_IT34_2020");
            modelBuilder.Entity<KontigentKarata>().ToTable("KontigentKarata", "EONIS_IT34_2020");
            modelBuilder.Entity<Porudzbina>().ToTable("Porudzbina", "EONIS_IT34_2020");

            modelBuilder.Entity<Porudzbina>().HasKey(pk => new { pk.Id_korisnik, pk.Id_kontigentKarata });

            /*modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("Popust"));*/
        }
    }
}