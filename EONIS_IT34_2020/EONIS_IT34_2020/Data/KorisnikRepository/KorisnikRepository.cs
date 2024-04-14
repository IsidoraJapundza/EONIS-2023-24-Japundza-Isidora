using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;
using System.Net.WebSockets;

namespace EONIS_IT34_2020.Data.KorisnikRepository
{
    public class KorisnikRepository : IKorisnikRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public KorisnikRepository(DatabaseContext context, IMapper mapper)
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
            return this.context.Korisnici.ToList();
        }

        public Korisnik GetKorisnikById(Guid Id_korisnik)
        {
            return context.Korisnici.FirstOrDefault(e => e.Id_korisnik == Id_korisnik);
        }

        public Korisnik CreateKorisnik(KorisnikCreationDto korisnik)
        {
            Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);
            korisnikEntity.Id_korisnik = Guid.NewGuid();
            // lozinka 
            var createdKorisnik = context.Korisnici.Add(korisnikEntity);
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
            context.Remove(deletedKorisnik);
        }
    }
}
