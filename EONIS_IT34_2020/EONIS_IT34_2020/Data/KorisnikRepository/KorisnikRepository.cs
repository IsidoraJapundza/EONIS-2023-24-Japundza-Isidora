using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;
using System.Net.WebSockets;

namespace EONIS_IT34_2020.Data.KorisnikRepository
{
    public class KorisnikRepository : IKorisnikRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

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
            return context.Korisnik.FirstOrDefault(e => e.Id_korisnik == Id_korisnik);
        }

        public Korisnik GetKorisnikByKorisnickoIme(string korisnickoImeKorisnika)
        {
            return context.Korisnik.FirstOrDefault(e => e.KorisnickoImeKorisnika == korisnickoImeKorisnika);
        }

        public Korisnik CreateKorisnik(KorisnikCreationDto korisnik)
        {
            Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);
            korisnikEntity.Id_korisnik = Guid.NewGuid();
            /* lozinka 
            var lozinkaKorisnikaHashed = HashPassword(administrator.LozinkaKorisnika);
            korisnikEntity.lozinkaKorisnikaHashed = Convert.FromBase64String(lozinkaKorisnikaHashed.Item1);
            korisnikEntity.saltKorisnikaa = Convert.FromBase64String(lozinkaKorisnikaHashed.Item2);
            */
            var createdKorisnik = this.context.Korisnik.Add(korisnikEntity);
            return mapper.Map<Korisnik>(createdKorisnik.Entity);
        }

        public void UpdateKorisnik(Korisnik korisnik)
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeleteKorisnik(Guid Id_korisnik)
        {
            var deletedKorisnik = GetKorisnikById(Id_korisnik);
            this.context.Remove(deletedKorisnik);
        }
    }
}
