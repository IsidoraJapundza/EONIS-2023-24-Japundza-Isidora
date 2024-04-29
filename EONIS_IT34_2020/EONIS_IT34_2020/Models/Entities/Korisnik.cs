using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EONIS_IT34_2020.Models.Entities
{
    [Table("Korisnik")]
    public class Korisnik
    {
        [Key]
        public Guid Id_korisnik { get; set; }
        public String ImeKorisnika { get; set; }
        public String PrezimeKorisnika { get; set; }
        public String KorisnickoImeKorisnika { get; set; }
        public byte[] LozinkaKorisnikaHashed { get; set; }         
        public byte[] saltKorisnika { get; set; }   
        public String MejlKorisnika { get; set; }
        public String KontaktKorisnika { get; set; }
        public String? AdresaKorisnika { get; set; }
        public String? PrebivalisteKorisnika { get; set; }
        public int? PostanskiBroj { get; set; }
        public DateOnly DatumRodjenjaKorisnika { get; set; }
        public DateOnly DatumRegistracijeKorisnika { get; set; }

    }
}
