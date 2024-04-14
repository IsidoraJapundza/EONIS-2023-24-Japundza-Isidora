using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontigentKarataRepository
{
    public class KontigentKarataRepository : IKontigentKarataRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public KontigentKarataRepository(DatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<KontigentKarata> GetKontigentKarata()
        {
            return this.context.KontigentiKarata.ToList();
        }

        public KontigentKarata GetKontigentKarataById(Guid Id_kontigentKarata)
        {
            return context.KontigentiKarata.FirstOrDefault(e => e.Id_kontigentKarata == Id_kontigentKarata);
        }

        public KontigentKarata CreateKontigentKarata(KontigentKarata kontigentKarata)
        {
            var createdKontigentKarata = context.KontigentiKarata.Add(kontigentKarata);
            this.context.SaveChanges();
            return mapper.Map<KontigentKarata>(createdKontigentKarata.Entity);
        }

        public void UpdateKontigentKarata(KontigentKarata kontigentKarata)
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeleteKontigentKarata(Guid Id_kontigentKarata)
        {
            var deletedKontigentKarata = GetKontigentKarataById(Id_kontigentKarata);
            context.Remove(deletedKontigentKarata);
        }
    }
}