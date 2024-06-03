using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.PorudzbinaRepository
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbina();
        Porudzbina GetExactPorudzbina(Guid Id_porudzbina);
        Porudzbina GetPorudzbinaById(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata);
        List<Porudzbina> GetPorudzbinaByKorisnik(Guid Id_korisnik);
        Porudzbina CreatePorudzbina(Porudzbina porudzbina);
        Porudzbina UpdatePorudzbina(Porudzbina porudzbina);
        void DeletePorudzbina(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata);
        bool SaveChanges();
    }
}
