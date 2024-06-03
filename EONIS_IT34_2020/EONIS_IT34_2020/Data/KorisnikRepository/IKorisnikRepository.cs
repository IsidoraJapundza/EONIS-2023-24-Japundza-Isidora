using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KorisnikRepository
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnik();
        Korisnik GetKorisnikById(Guid Id_korisnik);
        Korisnik GetKorisnikByKorisnickoIme(string korisnickoIme);
        Korisnik CreateKorisnik(KorisnikCreationDto korisnik);
        Korisnik UpdateKorisnik(KorisnikUpdateDto korisnik);
        void DeleteKorisnik(Guid Id_korisnik);
        void DeleteKorisnik(string korisnickoIme); 
        bool SaveChanges();
        bool KorisnikWithCredentialsExists(string korisnickoIme, string lozinka);
    }
}
