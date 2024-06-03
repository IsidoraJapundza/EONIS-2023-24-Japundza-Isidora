using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;
using System.Data.SqlTypes;
using System.Net.WebSockets;
using System.Security.Cryptography;

namespace EONIS_IT34_2020.Data.KorisnikRepository
{
    public class KorisnikRepository : IKorisnikRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;
        private readonly static int iterations = 1000;

        public KorisnikRepository(DatabaseContextDB context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Korisnik> GetKorisnik()
        {
            return this.context.Korisnik.ToList();
        }

        public Korisnik GetKorisnikById(Guid Id_korisnik)
        {
            return this.context.Korisnik.FirstOrDefault(e => e.Id_korisnik == Id_korisnik);
        }

        public Korisnik GetKorisnikByKorisnickoIme(string korisnickoIme)
        {
            return context.Korisnik.FirstOrDefault(e => e.KorisnickoImeKorisnika == korisnickoIme);
        }

        public Korisnik CreateKorisnik(KorisnikCreationDto korisnik)
        {
            Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);
            korisnikEntity.Id_korisnik = Guid.NewGuid();
            korisnikEntity.DatumRegistracijeKorisnika = DateOnly.FromDateTime(DateTime.Now);
            // lozinka 
            var lozinkaKorisnikaHashed = HashPassword(korisnik.LozinkaKorisnika);
            korisnikEntity.LozinkaKorisnikaHashed = Convert.FromBase64String(lozinkaKorisnikaHashed.Item1);
            //korisnikEntity.saltKorisnika = Convert.FromBase64String(lozinkaKorisnikaHashed.Item2);

            var createdKorisnik = this.context.Korisnik.Add(korisnikEntity);
            this.context.SaveChanges();
            return mapper.Map<Korisnik>(createdKorisnik.Entity);
        }

        public Korisnik UpdateKorisnik(KorisnikUpdateDto korisnik)
        {
            try
            {
                var existingKorisnik = this.context.Korisnik.FirstOrDefault(e => e.Id_korisnik == korisnik.Id_korisnik);

                if (existingKorisnik != null)
                {
                    existingKorisnik.ImeKorisnika = korisnik.ImeKorisnika;
                    existingKorisnik.PrezimeKorisnika = korisnik.PrezimeKorisnika;
                    existingKorisnik.KorisnickoImeKorisnika = korisnik.KorisnickoImeKorisnika;
                    existingKorisnik.MejlKorisnika = korisnik.MejlKorisnika;
                    existingKorisnik.KontaktKorisnika = korisnik.KontaktKorisnika;
                    existingKorisnik.AdresaKorisnika = korisnik.AdresaKorisnika;
                    existingKorisnik.PrebivalisteKorisnika = korisnik.PrebivalisteKorisnika;
                    existingKorisnik.PostanskiBroj = korisnik.PostanskiBroj;
                    existingKorisnik.DatumRodjenjaKorisnika = korisnik.DatumRodjenjaKorisnika; //DateOnly.Parse(korisnik.DatumRodjenjaKorisnika);

                    var novaLozinkaHashed = HashPassword(korisnik.LozinkaKorisnika);
                    existingKorisnik.LozinkaKorisnikaHashed = Convert.FromBase64String(novaLozinkaHashed.Item1);
                    //existingKorisnik.saltKorisnika = Convert.FromBase64String(novaLozinkaHashed.Item2);

                    this.context.SaveChanges();

                    return existingKorisnik;
                }
                else
                {
                    throw new KeyNotFoundException($"Korisnik with ID {korisnik.Id_korisnik} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Korisnik.", ex);
            }
        }

        public void DeleteKorisnik(Guid Id_korisnik)
        {
            var deletedKorisnik = GetKorisnikById(Id_korisnik);
            this.context.Remove(deletedKorisnik);
            this.context.SaveChanges();
        }

        public void DeleteKorisnik(string korisnickoIme)
        {
            try
            {
                var korisnik = context.Korisnik.FirstOrDefault(e => e.KorisnickoImeKorisnika == korisnickoIme);

                if (korisnik != null)
                {
                    context.Korisnik.Remove(korisnik);
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Korisnik with username {korisnickoIme} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Korisnik!", ex);
            }
        }

        public bool KorisnikWithCredentialsExists(string korisnickoIme, string lozinka)
        {
            Korisnik korisnik = context.Korisnik.FirstOrDefault(k => k.KorisnickoImeKorisnika== korisnickoIme);

            if (korisnik == null)
            {
                return false;
            }
            if (VerifyPassword(lozinka, Convert.ToBase64String(korisnik.LozinkaKorisnikaHashed)))//, korisnik.saltKorisnika))
            {
                return true;
            }
            return false;
        }

        // helpers
        public bool VerifyPassword(string lozinka, string lozinkaHashed) //, byte[] salt)
        {
            //var saltBytes = salt;
            //var rfc2898DeriveBytes = new Rfc2898DeriveBytes(lozinka, saltBytes, iterations);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(lozinka, iterations);
            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == lozinkaHashed)
            {
                return true;
            }
            return false;
        }
        private Tuple<string, string> HashPassword(string lozinka)
        {
            var sBytes = new byte[lozinka.Length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);

            var derivedBytes = new Rfc2898DeriveBytes(lozinka, sBytes, iterations);

            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }
    }
}
