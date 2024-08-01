namespace BasedeDatos.DTOs
{
    public class CantonDto
    {
        public decimal Id_Canton { get; set; }

        public string Canton { get; set; } = null!;

        public string? Provincia { get; set; }

        public string? Region { get; set; }
    }
}
