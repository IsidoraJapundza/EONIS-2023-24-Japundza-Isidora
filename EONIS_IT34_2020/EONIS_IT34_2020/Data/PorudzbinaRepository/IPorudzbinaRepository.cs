using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.PorudzbinaRepository
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbina();
        Porudzbina GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontingentKarata);
        Porudzbina CreatePorudzbina(Porudzbina porudzbina);
        Porudzbina UpdatePorudzbina(Porudzbina porudzbina);
        void DeletePorudzbina(Guid Id_korisnik, Guid Id_kontingentKarata);
        bool SaveChanges();
    }
}
