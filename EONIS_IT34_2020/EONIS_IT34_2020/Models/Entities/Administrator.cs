using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EONIS_IT34_2020.Models.Entities
{
    public class Administrator
    {
        [Key]
        public Guid Id_administrator { get; set; }
        public String ImeAdministratora { get; set; }
        public String PrezimeAdministratora { get; set; }
        public String KorisnickoImeAdministratora { get; set; }
        public String MejlAdministratora { get; set; }
        public byte[]? LozinkaAdministratoraHashed { get; set; } 
        //public byte[]? saltAdministratora { get; set; }
        public String? KontaktAdministratora { get; set; }
        public String? StatusAktivnosti { get; set; }
        public String? Privilegije { get; set; }
    }
}