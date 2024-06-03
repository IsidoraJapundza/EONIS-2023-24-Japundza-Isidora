using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontingentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontingentKarataRepository
{
    public class KontingentKarataRepository : IKontingentKarataRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

        public KontingentKarataRepository(DatabaseContextDB context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<KontingentKarata> GetKontingentKarata()
        {
            
            return this.context.KontingentKarata.ToList();
        }

        public KontingentKarata GetKontingentKarataById(Guid Id_kontingentKarata)
        {
            return this.context.KontingentKarata.FirstOrDefault(e => e.Id_kontingentKarata == Id_kontingentKarata);
        }

        public List<KontingentKarata> GetKontingentKarataByNaziv(string naziv)
        {
            return context.KontingentKarata.Where(e => e.NazivKarte == naziv).ToList();
        }

        public KontingentKarata CreateKontingentKarata(KontingentKarata kontingentKarata)
        {
            var createdKontingentKarata = this.context.KontingentKarata.Add(kontingentKarata);
            this.context.SaveChanges();
            return mapper.Map<KontingentKarata>(createdKontingentKarata.Entity);
        }

        public KontingentKarata UpdateKontingentKarata(KontingentKarata kontingentKarata)
        {
            try
            {
                var existingKontingentKarata = this.context.KontingentKarata.FirstOrDefault(e => e.Id_kontingentKarata == kontingentKarata.Id_kontingentKarata);

                if (existingKontingentKarata != null)
                {
                    existingKontingentKarata.NazivKarte = kontingentKarata.NazivKarte;
                    existingKontingentKarata.Sektor = kontingentKarata.Sektor;
                    existingKontingentKarata.Ulaz = kontingentKarata.Ulaz;
                    existingKontingentKarata.Cena = kontingentKarata.Cena;
                    existingKontingentKarata.Kolicina = kontingentKarata.Kolicina;
                    existingKontingentKarata.Napomena = kontingentKarata.Napomena;
                    existingKontingentKarata.Id_administrator = kontingentKarata.Id_administrator;
                    existingKontingentKarata.Id_dogadjaj = kontingentKarata.Id_dogadjaj;
                    this.context.SaveChanges();

                    return existingKontingentKarata;
                }
                else
                {
                    throw new KeyNotFoundException($"KontingentKarata with ID {kontingentKarata.Id_kontingentKarata} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating KontingentKarata.", ex);
            }
        }

        public void DeleteKontingentKarata(Guid Id_kontingentKarata)
        {
            var deletedKontingentKarata = GetKontingentKarataById(Id_kontingentKarata);
            this.context.Remove(deletedKontingentKarata);
            this.context.SaveChanges();
        }
    }
}