namespace EONIS_IT34_2020.Models.DTOs.Dogadjaj
{
    public class DogadjajUpdateDto
    {
        public Guid Id_dogadjaj { get; set; }
        public String NazivSportskogDogadjaja { get; set; }
        public DateOnly DatumOdrzavanja { get; set; }
        public TimeOnly VremeOdrzavanja { get; set; }
        public TimeOnly PredvidjenoVremeZavrsetka { get; set; }
        public String MestoOdrzavanja { get; set; }
    }
}
