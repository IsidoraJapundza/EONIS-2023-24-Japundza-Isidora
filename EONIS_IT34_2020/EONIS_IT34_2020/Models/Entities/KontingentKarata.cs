﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EONIS_IT34_2020.Models.Entities
{
    //[Table("KontingentKarata")]
    public class KontingentKarata
    {
        [Key]
        public Guid Id_kontingentKarata { get; set; }
        public String NazivKarte { get; set; }
        public String Sektor { get; set; }
        public String Ulaz { get; set; }
        public int Cena { get; set; }
        public int Kolicina { get; set; }
        public String Napomena { get; set; }
        public Guid? Id_administrator { get; set; }
        public Guid? Id_dogadjaj { get; set; }
    }
}
