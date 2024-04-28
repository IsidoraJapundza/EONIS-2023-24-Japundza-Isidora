using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontigentKarataRepository
{
    public class KontigentKarataRepository : IKontigentKarataRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

        public KontigentKarataRepository(DatabaseContextDB context, IMapper mapper)
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
            
            return this.context.KontigentKarata.ToList();
        }

        public KontigentKarata GetKontigentKarataById(Guid Id_kontigentKarata)
        {
            return this.context.KontigentKarata.FirstOrDefault(e => e.Id_kontigentKarata == Id_kontigentKarata);
        }

        public KontigentKarata CreateKontigentKarata(KontigentKarata kontigentKarata)
        {
            var createdKontigentKarata = this.context.KontigentKarata.Add(kontigentKarata);
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
            this.context.Remove(deletedKontigentKarata);
            this.context.SaveChanges();
        }
    }
}