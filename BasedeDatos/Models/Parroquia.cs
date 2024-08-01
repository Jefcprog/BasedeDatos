namespace BasedeDatos.Models
{
    public class Parroquia
    {
        public int IdPar { get; set; }

        public string? Nombre { get; set; }

        public int? IdCan { get; set; }

        public int? IdProv { get; set; }

        public int? Region { get; set; }

        
        public Region? Regiones { get; set; } // Acepta NULL
        public Provincia? Provincias { get; set; } // Acepta NULL
        public Canton? Cantones { get; set; } // Acepta NULL
    }
}
