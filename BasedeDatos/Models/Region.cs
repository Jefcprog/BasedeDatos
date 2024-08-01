namespace BasedeDatos.Models
{
    public class Region
    {
        public int IdReg { get; set; }

        public string? Nombre { get; set; }

        
        public ICollection<Provincia>? Provincias { get; set; }
        public ICollection<Canton>? Cantones { get; set; }
        public ICollection<Parroquia>? Parroquias { get; set; }


        // Constructor
        public Region()
        {
            Provincias = new List<Provincia>();
        }
    }
}
