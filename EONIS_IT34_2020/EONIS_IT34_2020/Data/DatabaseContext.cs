using EONIS_IT34_2020.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EONIS_IT34_2020.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly static int iterations = 1000;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
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
            modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("Popust"));
        }
    }
}