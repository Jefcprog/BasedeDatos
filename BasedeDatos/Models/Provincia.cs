namespace BasedeDatos.Models
{
    public class Provincia
    {
        public int IdProv { get; set; }

        public string? Nombre { get; set; }

        public int? Region { get; set; }

        public ICollection<Canton>? Cantones { get; set; } // Acepta NULL
        public ICollection<Parroquia> Parroquias { get; set; }

        // Constructor
        public Provincia()
        {
            Cantones = new List<Canton>();
        }
    }
}
