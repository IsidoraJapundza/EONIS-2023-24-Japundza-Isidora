namespace EONIS_IT34_2020.Models.Entities
{
    public class KontigentKarata
    {
        public Guid Id_kontigentKarata { get; set; }
        public String NazivKarte { get; set; }
        public String Sektor { get; set; }
        public String Ulaz { get; set; }
        public double Cena { get; set; }
        public int Kolicina { get; set; }
        public String Napomena { get; set; }
        public Guid Id_administrator { get; set; }
        public Guid Id_dogadjaj { get; set; }

    }
}
