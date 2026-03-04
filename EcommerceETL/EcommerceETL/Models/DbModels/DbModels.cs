using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceETL.Models.DbModels
{
    public class DimFecha
    {
        public int FechaId { get; set; }    
        public DateTime Fecha { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public int Trimestre { get; set; }
        public int Anio { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public bool EsFinDeSemana { get; set; }
    }

    public class DimCategoria
    {
        public int CategoriaId { get; set; }   
        public string Nombre { get; set; } = string.Empty;
    }

    public class DimProducto
    {
        public int ProductoId { get; set; }    
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public decimal Rating { get; set; }
    }

    public class DimCliente
    {
        public int ClienteId { get; set; }      
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string RangoEdad { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

    public class FactVentas
    {
        public int CarritoId { get; set; }
        public int FechaId { get; set; }
        public int ClienteId { get; set; }
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal DescuentoPct { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalNeto { get; set; }
    }
}
