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

        public Porudzbina GetExactPorudzbina(Guid Id_porudzbina)
        {
            return this.context.Porudzbina.FirstOrDefault(e => e.Id_porudzbina == e.Id_porudzbina);
        }


        public Porudzbina GetPorudzbinaById(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata)
        {
            return this.context.Porudzbina.FirstOrDefault(e => e.Id_porudzbina == e.Id_porudzbina && e.Id_korisnik == Id_korisnik && e.Id_kontingentKarata == Id_kontingentKarata);
        }

        public List<Porudzbina> GetPorudzbinaByKorisnik(Guid Id_korisnik)
        {
            return this.context.Porudzbina.Where(e => e.Id_korisnik == Id_korisnik).ToList();
        }


        public Porudzbina CreatePorudzbina(Porudzbina porudzbina)
        {
            porudzbina.Id_porudzbina = Guid.NewGuid();

            var kontingentKarataEntity = context.KontingentKarata.FirstOrDefault(kk => kk.Id_kontingentKarata == porudzbina.Id_kontingentKarata);
            
            if (kontingentKarataEntity == null)
            {
                throw new Exception("KontingentKarata not found.");
            }

            porudzbina.DatumPorudzbine= DateOnly.FromDateTime(DateTime.Now);
            porudzbina.VremePorudzbine = TimeOnly.FromDateTime(DateTime.Now);
            porudzbina.UkupnaCena = porudzbina.BrojKarata * kontingentKarataEntity.Cena;

            var createdPorudzbina = this.context.Porudzbina.Add(porudzbina);
            this.context.SaveChanges();

            // Log pre 
            Console.WriteLine("Pre reload-a: " + createdPorudzbina.Entity.UkupnaCena);

            // Reload entiteta da bi se povukle promene napravljene trigerom
            context.Entry(createdPorudzbina.Entity).Reload();

            // Log posle 
            Console.WriteLine("Posle reload-a: " + createdPorudzbina.Entity.UkupnaCena);

            return mapper.Map<Porudzbina>(createdPorudzbina.Entity);
        }

        public Porudzbina UpdatePorudzbina(Porudzbina porudzbina)
        {
            try
            {
                var existingPorudzbina = this.context.Porudzbina.FirstOrDefault(e => (e.Id_korisnik == porudzbina.Id_korisnik && e.Id_kontingentKarata == porudzbina.Id_kontingentKarata));

                if (existingPorudzbina != null)
                {
                    existingPorudzbina.BrojKarata = porudzbina.BrojKarata;
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

        public void DeletePorudzbina(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata)
        {
            var deletedPorudzbina = GetPorudzbinaById(Id_porudzbina, Id_korisnik, Id_kontingentKarata);
            context.Remove(deletedPorudzbina);
            this.context.SaveChanges();
        }
    }
}