using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.PorudzbinaRepository
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbina();
        Porudzbina GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontigentKarata);
        Porudzbina CreatePorudzbina(Porudzbina porudzbina);
        void UpdatePorudzbina(Porudzbina porudzbina); // izmeniti
        void DeletePorudzbina(Guid Id_korisnik, Guid Id_kontigentKarata);

        bool SaveChanges();
    }
}
