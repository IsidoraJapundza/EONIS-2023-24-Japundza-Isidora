namespace EONIS_IT34_2020.Models.Entities
{
    public class Dogadjaj
    {
        public Guid Id_dogadjaj { get; set; }
        public String NazivSportskogDogadjaja { get; set; }
        public DateOnly DatumOdrzavanja { get; set; }
        public TimeOnly VremeOdrzavanja { get; set; }
        public TimeOnly PredvidjenoVremeZavrsetka { get; set; }
        public String MestoOdrzavanja { get; set; }

    }
}