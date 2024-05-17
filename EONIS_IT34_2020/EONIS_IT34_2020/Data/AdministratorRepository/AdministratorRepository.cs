using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Administrator;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace EONIS_IT34_2020.Data.AdministratorRepository
{
    public class AdministratorRepository : IAdministratorRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;
        private readonly static int iterations = 1000;

        public AdministratorRepository(DatabaseContextDB context, IMapper mapper)
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

        public Administrator GetAdministratorByKorisnickoIme(string korisnickoIme)
        {
            return context.Administrator.FirstOrDefault(e => e.KorisnickoImeAdministratora == korisnickoIme);
        }

        public Administrator CreateAdministrator(AdministratorCreationDto administrator)
        {
            Administrator administratorEntity = mapper.Map<Administrator>(administrator);
            administratorEntity.Id_administrator = Guid.NewGuid();
            // lozinka 
            var lozinkaAdministratoraHashed = HashPassword(administrator.LozinkaAdministratora);
            administratorEntity.LozinkaAdministratoraHashed = Convert.FromBase64String(lozinkaAdministratoraHashed.Item1);
            //administratorEntity.saltAdministratora = Convert.FromBase64String(lozinkaAdministratoraHashed.Item2);
            
            var createdAdministrator = this.context.Administrator.Add(administratorEntity);
            this.context.SaveChanges();
            return mapper.Map<Administrator>(createdAdministrator.Entity);
        }

        public Administrator UpdateAdministrator(AdministratorUpdateDto administrator)  
        {
            try
            {
                var existingAdministrator = this.context.Administrator.FirstOrDefault(e => e.Id_administrator == administrator.Id_administrator);

                if (existingAdministrator != null)
                {
                    existingAdministrator.ImeAdministratora = administrator.ImeAdministratora;
                    existingAdministrator.PrezimeAdministratora = administrator.PrezimeAdministratora;
                    existingAdministrator.KorisnickoImeAdministratora = administrator.KorisnickoImeAdministratora;
                    existingAdministrator.MejlAdministratora = administrator.MejlAdministratora;
                    existingAdministrator.KontaktAdministratora = administrator.KontaktAdministratora;
                    existingAdministrator.StatusAktivnosti = administrator.StatusAktivnosti;
                    existingAdministrator.Privilegije = administrator.Privilegije;
                   
                    var novaLozinkaHashed = HashPassword(administrator.LozinkaAdministratora);
                    existingAdministrator.LozinkaAdministratoraHashed = Convert.FromBase64String(novaLozinkaHashed.Item1);
                    //existingAdministrator.saltAdministratora = Convert.FromBase64String(novaLozinkaHashed.Item2);

                    this.context.SaveChanges();

                    return existingAdministrator;
                }
                else
                {
                    throw new KeyNotFoundException($"Administrator with ID {administrator.Id_administrator} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Administrator.", ex);
            }
        }

        public void DeleteAdministrator(Guid Id_administrator)
        {
            var deletedAdministrator = GetAdministratorById(Id_administrator);
            this.context.Remove(deletedAdministrator);
            this.context.SaveChanges();
        }

        public void DeleteAdministrator(string korisnickoIme)
        {
            try
            {
                var administrator = context.Administrator.FirstOrDefault(e => e.KorisnickoImeAdministratora == korisnickoIme);

                if(administrator != null)
                {
                    context.Administrator.Remove(administrator);
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Administrator with username {korisnickoIme} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Administrator!", ex);
            }
        }

        public bool AdministratorWithCredentialsExists(string korisnickoIme, string lozinka)
        {
            Administrator administrator = context.Administrator.FirstOrDefault(e => e.KorisnickoImeAdministratora == korisnickoIme);

            if(administrator == null)
            {
                return false;
            }
            if(VerifyPassword(lozinka, Convert.ToBase64String(administrator.LozinkaAdministratoraHashed)))//, administrator.saltAdministratora))
            {
                return true;
            }
            return false;
        }

        // helpers
        public bool VerifyPassword(string lozinka, string lozinkaHashed)//, byte[] salt)
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
