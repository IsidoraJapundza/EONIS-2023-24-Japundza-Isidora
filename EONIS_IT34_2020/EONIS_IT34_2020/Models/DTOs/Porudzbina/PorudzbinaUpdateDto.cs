﻿namespace EONIS_IT34_2020.Models.DTOs.Porudzbina
{
    public class PorudzbinaUpdateDto
    {
        public Guid Id_porudzbina { get; set; }
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
