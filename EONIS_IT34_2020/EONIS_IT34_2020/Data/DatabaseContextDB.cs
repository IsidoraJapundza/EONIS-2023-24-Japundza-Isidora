using EONIS_IT34_2020.Models.Entities;
using EONIS_IT34_2020.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Cryptography;

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
        public DbSet<KontingentKarata> KontingentKarata { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Porudzbina> Porudzbina { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik", "EONIS_IT34_2020");
            modelBuilder.Entity<Administrator>().ToTable("Administrator", "EONIS_IT34_2020");
            modelBuilder.Entity<Dogadjaj>().ToTable("Dogadjaj", "EONIS_IT34_2020");
            modelBuilder.Entity<KontingentKarata>().ToTable("KontingentKarata", "EONIS_IT34_2020");
            modelBuilder.Entity<Porudzbina>().ToTable("Porudzbina", "EONIS_IT34_2020");

            modelBuilder.Entity<KontingentKarata>().HasKey(pk => new { pk.Id_dogadjaj, pk.Id_administrator });

            /* ispraviti
             * modelBuilder.Entity<KontingentKarata>()
                .HasOne(a => a.Administrator)
                .HasForeignKey(d =>  d.Id_administrator)
                .OnDelete(DeleteBehavior.SetNull); // Postavlja vrednost na null umesto brisanja*/

            modelBuilder.Entity<Porudzbina>().HasKey(pk => new { pk.Id_korisnik, pk.Id_kontingentKarata });

            modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("ProveriDostupnostKarata"));

            modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("ProveriDatumRodjenja"));
        }

        //ispraviti
        /*private Tuple<string, string> HashPassword(string password)
        {
            var sBytes = new byte[password.Length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);

            var derivedBytes = new Rfc2898DeriveBytes(password, sBytes, iterations);

            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }*/
    }
}