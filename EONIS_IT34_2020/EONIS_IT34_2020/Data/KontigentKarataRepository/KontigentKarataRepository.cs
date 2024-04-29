using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.KontigentKarataRepository
{
    public class KontigentKarataRepository : IKontigentKarataRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

        public KontigentKarataRepository(DatabaseContextDB context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<KontigentKarata> GetKontigentKarata()
        {
            
            return this.context.KontigentKarata.ToList();
        }

        /*public List<KontigentKarata> GetKontigentKarata()
        {
            var kontigentKarataList = this.context.KontigentKarata.ToList();

            // Check for null values before accessing properties
            foreach (var kontigentKarata in kontigentKarataList)
            {
                // Handle nullable properties as needed
                if (kontigentKarata.Napomena == null)
                {
                    kontigentKarata.Napomena = ""; // Provide a default value or handle null case
                }
            }

            return kontigentKarataList;
        }*/

        public KontigentKarata GetKontigentKarataById(Guid Id_kontigentKarata)
        {
            return this.context.KontigentKarata.FirstOrDefault(e => e.Id_kontigentKarata == Id_kontigentKarata);
        }

        public KontigentKarata CreateKontigentKarata(KontigentKarata kontigentKarata)
        {
            var createdKontigentKarata = this.context.KontigentKarata.Add(kontigentKarata);
            this.context.SaveChanges();
            return mapper.Map<KontigentKarata>(createdKontigentKarata.Entity);
        }


        public KontigentKarata UpdateKontigentKarata(KontigentKarata kontigentKarata)
        {
            try
            {
                var existingKontigentKarata = this.context.KontigentKarata.FirstOrDefault(e => e.Id_kontigentKarata == kontigentKarata.Id_kontigentKarata);

                if (existingKontigentKarata != null)
                {
                    existingKontigentKarata.NazivKarte = kontigentKarata.NazivKarte;
                    existingKontigentKarata.Sektor = kontigentKarata.Sektor;
                    existingKontigentKarata.Ulaz = kontigentKarata.Ulaz;
                    existingKontigentKarata.Cena = kontigentKarata.Cena;
                    existingKontigentKarata.Kolicina = kontigentKarata.Kolicina;
                    existingKontigentKarata.Napomena = kontigentKarata.Napomena;
                    existingKontigentKarata.Id_administrator = kontigentKarata.Id_administrator;
                    existingKontigentKarata.Id_dogadjaj = kontigentKarata.Id_dogadjaj;
                    this.context.SaveChanges();

                    return existingKontigentKarata;
                }
                else
                {
                    throw new KeyNotFoundException($"KontigentKarata with ID {kontigentKarata.Id_kontigentKarata} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating KontigentKarata.", ex);
            }
        }

        public void DeleteKontigentKarata(Guid Id_kontigentKarata)
        {
            var deletedKontigentKarata = GetKontigentKarataById(Id_kontigentKarata);
            this.context.Remove(deletedKontigentKarata);
            this.context.SaveChanges();
        }
    }
}