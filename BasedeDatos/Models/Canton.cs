namespace BasedeDatos.Models
{
    public class Canton
    {
        public decimal IdCan { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal? IdProv { get; set; }

        public decimal? Region { get; set; }

        public ICollection<Parroquia> Parroquias { get; set; }


        // Constructor
        public Canton()
        {
            Parroquias = new List<Parroquia>();
        }
    }
}
