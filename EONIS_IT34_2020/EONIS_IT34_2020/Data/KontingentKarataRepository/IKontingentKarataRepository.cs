using EONIS_IT34_2020.Models.DTOs.KontingentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontingentKarataRepository
{
    public interface IKontingentKarataRepository
    {
        List<KontingentKarata> GetKontingentKarata();
        KontingentKarata GetKontingentKarataById(Guid Id_kontingentKarata);
        List<KontingentKarata> GetKontingentKarataByNaziv(string naziv);
        KontingentKarata CreateKontingentKarata(KontingentKarata kontingentKarata);
        KontingentKarata UpdateKontingentKarata(KontingentKarata kontingentKarata);
        void DeleteKontingentKarata(Guid Id_kontingentKarata);
        bool SaveChanges();
    }
}