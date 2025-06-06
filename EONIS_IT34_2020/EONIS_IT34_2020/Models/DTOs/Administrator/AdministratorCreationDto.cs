﻿namespace EONIS_IT34_2020.Models.DTOs.Administrator
{
    public class AdministratorCreationDto
    {
        public String ImeAdministratora { get; set; }
        public String PrezimeAdministratora { get; set; }
        public String KorisnickoImeAdministratora { get; set; }
        public String MejlAdministratora { get; set; }
        public String? LozinkaAdministratora { get; set; } 
        public String KontaktAdministratora { get; set; }
        public String? StatusAktivnosti { get; set; }
        public String? Privilegije { get; set; }
    }
}
