using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Administrator;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.AdministratorRepository
{
    public class AdministratorRepository : IAdministratorRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public AdministratorRepository(DatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Administrator> GetAdministrator()
        {
            return this.context.Administratori.ToList();
        }

        public Administrator GetAdministratorById(Guid Id_administrator)
        {
            return context.Administratori.FirstOrDefault(e => e.Id_administrator == Id_administrator);
        }

        public Administrator CreateAdministrator(AdministratorCreationDto administrator)
        {
            Administrator administratorEntity = mapper.Map<Administrator>(administrator);
            administratorEntity.Id_administrator = Guid.NewGuid();
            // lozinka 
            var createdAdministrator = context.Administratori.Add(administratorEntity);
            return mapper.Map<Administrator>(createdAdministrator.Entity);
        }

        public void UpdateAdministrator(Administrator administrator)
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeleteAdministrator(Guid Id_administrator)
        {
            var deletedAdministrator = GetAdministratorById(Id_administrator);
            context.Remove(deletedAdministrator);
        }
    }
}
