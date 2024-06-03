using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EONIS_IT34_2020.Models.Entities
{
    [Table("Porudzbina")]
    [PrimaryKey(nameof(Id_porudzbina), nameof(Id_korisnik), nameof(Id_kontingentKarata))]
    public class Porudzbina
    {
        public Guid Id_porudzbina { get; set; }
        public DateOnly DatumPorudzbine { get; set; }
        public TimeOnly VremePorudzbine { get; set; }
        public int BrojKarata { get; set; }
        public int UkupnaCena { get; set; } 
        public String StatusPorudzbine { get; set; } // 'U toku', 'Završena', 'Otkazana'
        public String? PotvrdaPlacanja { get; set; } // 'Za naplatu', 'Plaćeno'
        public String? MetodaIsporuke { get; set; }
        public String? AdresaIsporuke { get; set; }
        public String? DodatneNapomene { get; set; }
        public Guid Id_korisnik { get; set; }
        public Guid Id_kontingentKarata { get; set; }
    }
}
