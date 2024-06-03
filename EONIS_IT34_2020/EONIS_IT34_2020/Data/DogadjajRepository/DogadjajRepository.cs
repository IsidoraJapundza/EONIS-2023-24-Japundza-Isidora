using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.DogadjajRepository
{
    public class DogadjajRepository : IDogadjajRepository
    {
        public readonly DatabaseContextDB context;
        public readonly IMapper mapper;

        public DogadjajRepository(DatabaseContextDB context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<Dogadjaj> GetDogadjaj()
        {
            return this.context.Dogadjaj.ToList();
        }

        public Dogadjaj GetDogadjajById(Guid Id_dogadjaj)
        {
            return this.context.Dogadjaj.FirstOrDefault(e => e.Id_dogadjaj == Id_dogadjaj);
        }

        public List<Dogadjaj> GetDogadjajByNaziv(string naziv)
        {
            return context.Dogadjaj.Where(e => e.NazivSportskogDogadjaja.Contains(naziv)).ToList();
        }

        public Dogadjaj CreateDogadjaj(Dogadjaj dogadjaj)
        {
            var createdDogadjaj = this.context.Dogadjaj.Add(dogadjaj);
            this.context.SaveChanges();
            return mapper.Map<Dogadjaj>(createdDogadjaj.Entity);
        }

        public Dogadjaj UpdateDogadjaj(Dogadjaj dogadjaj)
        {
            try
            {
                var existingDogadjaj = this.context.Dogadjaj.FirstOrDefault(e => e.Id_dogadjaj == dogadjaj.Id_dogadjaj);

                if (existingDogadjaj != null)
                {
                    existingDogadjaj.NazivSportskogDogadjaja = dogadjaj.NazivSportskogDogadjaja;
                    existingDogadjaj.DatumOdrzavanja = dogadjaj.DatumOdrzavanja;
                    existingDogadjaj.VremeOdrzavanja = dogadjaj.VremeOdrzavanja;
                    existingDogadjaj.PredvidjenoVremeZavrsetka = dogadjaj.PredvidjenoVremeZavrsetka;
                    existingDogadjaj.MestoOdrzavanja = dogadjaj.MestoOdrzavanja;
                    this.context.SaveChanges();

                    return existingDogadjaj;
                }
                else
                {
                    throw new KeyNotFoundException($"Dogadjaj with ID {dogadjaj.Id_dogadjaj} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Dogadjaj.", ex);
            }
        }

    public void DeleteDogadjaj(Guid Id_dogadjaj)
        {
            var deletedDogadjaj = GetDogadjajById(Id_dogadjaj);
            this.context.Remove(deletedDogadjaj);
            this.context.SaveChanges();
        }
    }
}