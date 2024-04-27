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
        //private readonly static int iterations = 1000;

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
            return this.context.Administrator.ToList();
        }

        public Administrator GetAdministratorById(Guid Id_administrator)
        {
            return context.Administrator.FirstOrDefault(e => e.Id_administrator == Id_administrator);
        }

        public Administrator GetAdministratorByKorisnickoIme(string korisnickoImeAdministratora)
        {
            return context.Administrator.FirstOrDefault(e => e.KorisnickoImeAdministratora == korisnickoImeAdministratora);
        }

        public Administrator CreateAdministrator(AdministratorCreationDto administrator)
        {
            Administrator administratorEntity = mapper.Map<Administrator>(administrator);
            administratorEntity.Id_administrator = Guid.NewGuid();
            /* lozinka 
            var lozinkaAdministratoraHashed = HashPassword(administrator.LozinkaAdministratora);
            administratorEntity.lozinkaAdministratoraHashed = Convert.FromBase64String(lozinkaAdministratoraHashed.Item1);
            administratorEntity.saltAdministratora = Convert.FromBase64String(lozinkaAdministratoraHashed.Item2);
            */
            var createdAdministrator = this.context.Administrator.Add(administratorEntity);
            return mapper.Map<Administrator>(createdAdministrator.Entity);
        }

        public void UpdateAdministrator(Administrator administrator)  //???
        {
            /*
               Nije potrebna implementacija jer EF core prati entitet koji smo izvukli iz baze
               i kada promenimo taj objekat i odradimo SaveChanges sve izmene će biti perzistirane.
            */
        }

        public void DeleteAdministrator(Guid Id_administrator)
        {
            var deletedAdministrator = GetAdministratorById(Id_administrator);
            this.context.Remove(deletedAdministrator);
        }

        public void DeleteAdministrator(string korisnickoImeAdministratora)
        {
            try
            {
                var administrator = context.Administrator.FirstOrDefault(e => e.KorisnickoImeAdministratora == korisnickoImeAdministratora);

                if(administrator != null)
                {
                    context.Administrator.Remove(administrator);
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Administrator with username {korisnickoImeAdministratora} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Administrator!", ex);
            }
        }
    }
}
