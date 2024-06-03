namespace EONIS_IT34_2020.Models.DTOs.Porudzbina
{
    public class PorudzbinaCreationDto
    {
        public int BrojKarata { get; set; }
        public String StatusPorudzbine { get; set; }
        public String? PotvrdaPlacanja { get; set; }
        public String? MetodaIsporuke { get; set; }
        public String? AdresaIsporuke { get; set; }
        public String? DodatneNapomene { get; set; }
        public Guid Id_korisnik { get; set; }
        public Guid Id_kontingentKarata { get; set; }
    }
}
