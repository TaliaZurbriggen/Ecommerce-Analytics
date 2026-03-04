namespace EcommerceReporting.Models
{
    public class KpisDto
    {
        public decimal TotalVendido { get; set; }
        public int CantidadOrdenes { get; set; }
        public decimal TicketPromedio { get; set; }
        public int TotalProductosVendidos { get; set; }
    }

    public class VentasPorMesDto
    {
        public int Mes {  get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public decimal TotalVendido { get; set; }
        public int CantidadOrdenes { get; set; }
    }

    public class VentasPorCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal TotalVendido { get; set; }
        public int CantidadProductosVendidos { get; set; }
    }

    public class TopProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Marca {  get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadVendida {  get; set; }
        public decimal TotalGenerado { get; set; }
    }

    public class TopClienteDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Ciudad {  get; set; } = string.Empty;
        public string Estado {  get; set; } = string.Empty;
        public string RangoEdad {  get; set; } = string.Empty;
        public decimal TotalGastado { get; set; }
        public int CantidadOrdenes { get; set; }   
    }

    public class VentasPorTrimestreDto
    {
        public int Trimestre { get; set; }
        public string NombreTrimestre {  get; set; } = string.Empty;
        public decimal TotalVendido { get; set; }
        public int CantidadOrdenes { get; set; }
    }

    public class PrediccionDto
    {
        public int Mes {  get; set; }
        public string NombreMes {  get; set; } = string.Empty;
        public int Anio { get; set; } 
        public decimal TotalVendido { get; set; }
        public string Tipo {  get; set; } = string.Empty;
    }
}
