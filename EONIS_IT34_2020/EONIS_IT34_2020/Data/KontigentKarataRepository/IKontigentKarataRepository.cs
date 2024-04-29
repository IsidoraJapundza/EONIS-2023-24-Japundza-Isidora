using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontigentKarataRepository
{
    public interface IKontigentKarataRepository
    {
        List<KontigentKarata> GetKontigentKarata();
        KontigentKarata GetKontigentKarataById(Guid Id_kontigentKarata);
        KontigentKarata CreateKontigentKarata(KontigentKarata kontigentKarata);
        //void UpdateKontigentKarata(KontigentKarata kontigentKarata); // izmeniti
        KontigentKarata UpdateKontigentKarata(KontigentKarata kontigentKarata);
        void DeleteKontigentKarata(Guid Id_kontigentKarata);
        bool SaveChanges();
    }
}