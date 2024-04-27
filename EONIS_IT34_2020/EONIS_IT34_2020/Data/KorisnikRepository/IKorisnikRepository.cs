using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KorisnikRepository
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnik();
        Korisnik GetKorisnikById(Guid Id_korisnik);
        Korisnik GetKorisnikByKorisnickoIme(String korisnickoImeKorisnika);
        Korisnik CreateKorisnik(KorisnikCreationDto korisnik);
        void UpdateKorisnik(Korisnik korisnik); // izmeniti
        void DeleteKorisnik(Guid Id_korisnik);
        bool SaveChanges();
        // dodati
    }
}
