using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EONIS_IT34_2020.Models.Entities
{
    [Table("Porudzbina")]
    [PrimaryKey(nameof(Id_korisnik), nameof(Id_kontigentKarata))]
    public class Porudzbina
    {
        public DateOnly DatumPorudzbine { get; set; }
        public TimeOnly VremePorudzbine { get; set; }
        public int BrojKarata { get; set; }
        public double UkupnaCena { get; set; }
        public String StatusPorudzbine { get; set; }
        public Boolean PotvrdaPlacanja { get; set; }
        public String MetodaIsporuke { get; set; }
        public String AdresaIsporuke { get; set; }
        public String DodatneNapomene { get; set; }
        public Guid Id_korisnik { get; set; }
        public Guid Id_kontigentKarata { get; set; }

    }
}
