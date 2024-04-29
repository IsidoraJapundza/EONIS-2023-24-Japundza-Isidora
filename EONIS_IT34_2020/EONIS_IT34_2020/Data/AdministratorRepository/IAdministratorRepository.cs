using EONIS_IT34_2020.Models.DTOs.Administrator;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.AdministratorRepository
{
    public interface IAdministratorRepository
    {
        List<Administrator> GetAdministrator();
        Administrator GetAdministratorById(Guid Id_administrator);
        //Administrator GetAdministratorByKorisnickoIme(string korisnickoIme);
        Administrator CreateAdministrator(AdministratorCreationDto administrator);
        void UpdateAdministrator(Administrator administrator); // izmeniti
        void DeleteAdministrator(Guid Id_administrator);
        void DeleteAdministrator(string korisnickoIme);
        bool SaveChanges();
        bool AdministratorWithCredentialsExists(string korisnickoIme, string lozinka);
    }
}