namespace EONIS_IT34_2020.Models.Entities
{
    public class Administrator
    {
        public Guid Id_administrator { get; set; }
        public String ImeAdministratora { get; set; }
        public String PrezimeAdministratora { get; set; }
        public String KorisnickoImeAdministratora { get; set; }
        // email?
        public String LozinkaAdministratora { get; set; } // hash
        public String KontaktAdministratora { get; set; }
        public Boolean StatusAktivnosti { get; set; }
        public String Privilegije { get; set; }
    }
}