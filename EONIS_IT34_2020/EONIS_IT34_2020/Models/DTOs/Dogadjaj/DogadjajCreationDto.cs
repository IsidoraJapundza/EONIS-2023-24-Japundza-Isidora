namespace EONIS_IT34_2020.Models.DTOs.Dogadjaj
{
    public class DogadjajCreationDto
    {
        public String NazivSportskogDogadjaja { get; set; }
        public DateOnly DatumOdrzavanja { get; set; }
        public TimeOnly VremeOdrzavanja { get; set; }
        public TimeOnly PredvidjenoVremeZavrsetka { get; set; }
        public String MestoOdrzavanja { get; set; }
    }
}
