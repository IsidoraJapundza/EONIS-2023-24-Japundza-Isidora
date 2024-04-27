using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.DogadjajRepository
{
    public class DogadjajRepository : IDogadjajRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public DogadjajRepository(DatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Dogadjaj> GetDogadjaj()
        {
            return this.context.Dogadjaj.ToList();
        }

        public Dogadjaj GetDogadjajById(Guid Id_dogadjaj)
        {
            return context.Dogadjaj.FirstOrDefault(e => e.Id_dogadjaj == Id_dogadjaj);
        }

        public Dogadjaj CreateDogadjaj(Dogadjaj dogadjaj)
        {
            var createdDogadjaj = this.context.Dogadjaj.Add(dogadjaj);
            this.context.SaveChanges();
            return mapper.Map<Dogadjaj>(createdDogadjaj.Entity);
        }

        public void UpdateDogadjaj(Dogadjaj dogadjaj)
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeleteDogadjaj(Guid Id_dogadjaj)
        {
            var deletedDogadjaj = GetDogadjajById(Id_dogadjaj);
            this.context.Remove(deletedDogadjaj);
            this.context.SaveChanges();
        }
    }
}