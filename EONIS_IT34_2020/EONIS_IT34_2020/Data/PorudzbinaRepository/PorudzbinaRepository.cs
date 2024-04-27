using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.PorudzbinaRepository
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public PorudzbinaRepository(DatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Porudzbina> GetPorudzbina()
        {
            return this.context.Porudzbina.ToList();
        }

        public Porudzbina GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontigentKarata)
        {
            return context.Porudzbina.FirstOrDefault(e => (e.Id_korisnik == Id_korisnik && e.Id_kontigentKarata == Id_kontigentKarata));
        }



        public Porudzbina CreatePorudzbina(Porudzbina porudzbina)
        {
            var createdPorudzbina = this.context.Porudzbina.Add(porudzbina);
            this.context.SaveChanges();
            return mapper.Map<Porudzbina>(createdPorudzbina.Entity);
        }

        public void UpdatePorudzbina(Porudzbina porudzbina)
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeletePorudzbina(Guid Id_korisnik, Guid Id_kontigentKarata)
        {
            var deletedPorudzbina = GetPorudzbinaById(Id_korisnik, Id_kontigentKarata);
            context.Remove(deletedPorudzbina);
            this.context.SaveChanges();
        }
    }
}
