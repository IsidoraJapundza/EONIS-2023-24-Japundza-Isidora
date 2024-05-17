using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.PorudzbinaRepository
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

        public PorudzbinaRepository(DatabaseContextDB context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Porudzbina> GetPorudzbina()
        {
            return this.context.Porudzbina.ToList();
        }

        public Porudzbina GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontingentKarata)
        {
            return this.context.Porudzbina.FirstOrDefault(e => e.Id_korisnik == Id_korisnik && e.Id_kontingentKarata == Id_kontingentKarata);
        }

        public Porudzbina CreatePorudzbina(Porudzbina porudzbina)
        {
            var createdPorudzbina = this.context.Porudzbina.Add(porudzbina);
            this.context.SaveChanges();
            return mapper.Map<Porudzbina>(createdPorudzbina.Entity);
        }

        public Porudzbina UpdatePorudzbina(Porudzbina porudzbina)
        {
            try
            {
                var existingPorudzbina = this.context.Porudzbina.FirstOrDefault(e => (e.Id_korisnik == porudzbina.Id_korisnik && e.Id_kontingentKarata == porudzbina.Id_kontingentKarata));

                if (existingPorudzbina != null)
                {
                    existingPorudzbina.DatumPorudzbine = porudzbina.DatumPorudzbine;
                    existingPorudzbina.VremePorudzbine = porudzbina.VremePorudzbine;
                    existingPorudzbina.BrojKarata = porudzbina.BrojKarata;
                    existingPorudzbina.UkupnaCena = porudzbina.UkupnaCena;
                    existingPorudzbina.StatusPorudzbine = porudzbina.StatusPorudzbine;
                    existingPorudzbina.PotvrdaPlacanja = porudzbina.PotvrdaPlacanja;
                    existingPorudzbina.MetodaIsporuke = porudzbina.MetodaIsporuke;
                    existingPorudzbina.AdresaIsporuke = porudzbina.AdresaIsporuke;
                    existingPorudzbina.DodatneNapomene = porudzbina.DodatneNapomene;
                    existingPorudzbina.Id_korisnik = porudzbina.Id_korisnik;
                    existingPorudzbina.Id_kontingentKarata = porudzbina.Id_kontingentKarata;
                    this.context.SaveChanges();

                    return existingPorudzbina;
                }
                else
                {
                    throw new KeyNotFoundException($"Porudzbina with IDs {porudzbina.Id_korisnik} and {porudzbina.Id_kontingentKarata} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Porudzbina.", ex);
            }
        }

        public void DeletePorudzbina(Guid Id_korisnik, Guid Id_kontingentKarata)
        {
            var deletedPorudzbina = GetPorudzbinaById(Id_korisnik, Id_kontingentKarata);
            context.Remove(deletedPorudzbina);
            this.context.SaveChanges();
        }
    }
}
