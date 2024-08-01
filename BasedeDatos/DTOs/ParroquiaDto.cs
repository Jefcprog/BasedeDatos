namespace BasedeDatos.DTOs
{
    public class ParroquiaDto
    {
        public decimal Id_Parroquia { get; set; }

        public string Parroquia { get; set; } = null!;

        public string? Canton { get; set; }

        public string? Provincia { get; set; }

        public string? Region { get; set; }
    }
}
