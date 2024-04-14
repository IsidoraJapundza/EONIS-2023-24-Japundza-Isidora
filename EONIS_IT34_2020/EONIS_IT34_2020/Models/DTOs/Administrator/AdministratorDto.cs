namespace EONIS_IT34_2020.Models.DTOs.Administrator
{
    public class AdministratorDto
    {
        public String ImeAdministratora { get; set; }
        public String PrezimeAdministratora { get; set; }
        public String KorisnickoImeAdministratora { get; set; }
        // email?
        public String KontaktAdministratora { get; set; }
        public Boolean StatusAktivnosti { get; set; }
        public String Privilegije { get; set; }
    }
}
