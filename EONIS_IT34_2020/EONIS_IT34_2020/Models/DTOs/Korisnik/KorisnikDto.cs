namespace EONIS_IT34_2020.Models.DTOs.Korisnik
{
    public class KorisnikDto
    {
        public String ImeKorisnika { get; set; }
        public String PrezimeKorisnika { get; set; }
        public String KorisnickoImeKorisnika { get; set; }
        public String MejlKorisnika { get; set; }
        public String KontaktKorisnika { get; set; }
        public String AdresaKorisnika { get; set; }
        public String PrebivalisteKorisnika { get; set; }
        public int PostanskiBroj { get; set; }
        public DateOnly DatumRodjenjaKorisnika { get; set; }
        public DateOnly DatumRegistracijeKorisnika { get; set; }
    }
}
