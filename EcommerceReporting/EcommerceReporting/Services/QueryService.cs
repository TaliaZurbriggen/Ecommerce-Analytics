using Microsoft.Data.SqlClient;
using EcommerceReporting.Models;

namespace EcommerceReporting.Services
{
    public class QueryService
    {
        private readonly string _connectionString;

        public QueryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<KpisDto> ObtenerKpisAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT
                    SUM(total_neto) AS total_vendido,
                    COUNT(DISTINCT carrito_id) AS cantidad_ordenes,
                    SUM(total_neto) / COUNT(DISTINCT carrito_id) AS ticket_promedio,
                    SUM(cantidad) AS total_productos_vendidos
                FROM FactVentas", conn);

            using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            return new KpisDto
            {
                TotalVendido = reader.GetDecimal(0),
                CantidadOrdenes = reader.GetInt32(1),
                TicketPromedio = reader.GetDecimal(2),
                TotalProductosVendidos = reader.GetInt32(3)
            };
        }

        public async Task<List<VentasPorMesDto>> ObtenerVentasPorMesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT f.mes, f.nombre_mes,
                       SUM(v.total_neto) AS total_vendido,
                       COUNT(DISTINCT v.carrito_id) AS cantidad_ordenes
                FROM FactVentas v
                INNER JOIN DimFecha f ON v.fecha_id = f.fecha_id
                GROUP BY f.mes, f.nombre_mes
                ORDER BY f.mes", conn);

            var resultado = new List<VentasPorMesDto>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultado.Add(new VentasPorMesDto
                {
                    Mes = reader.GetByte(0),
                    NombreMes = reader.GetString(1),
                    TotalVendido = reader.GetDecimal(2),
                    CantidadOrdenes = reader.GetInt32(3)
                });
            }
            return resultado;
        }

        public async Task<List<VentasPorCategoriaDto>> ObtenerVentasPorCategoriaAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT
                    c.nombre AS categoria,
                    SUM(v.total_neto) AS total_vendido,
                    SUM(v.cantidad) AS cantidad_productos_vendidos
                FROM FactVentas v
                JOIN DimCategoria c ON v.categoria_id = c.categoria_id
                GROUP BY c.nombre
                ORDER BY total_vendido DESC", conn);

            var resultado = new List<VentasPorCategoriaDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new VentasPorCategoriaDto
                {
                    Categoria = reader.GetString(0),
                    TotalVendido = reader.GetDecimal(1),
                    CantidadProductosVendidos = reader.GetInt32(2)
                });
            }
            return resultado;
        }

        public async Task<List<TopProductoDto>> ObtenerTopProductosAsync()
        {
            using var conn = new SqlConnection (_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT TOP 10
                    p.nombre,
                    p.marca,
                    c.nombre AS categoria,
                    SUM(v.cantidad) AS cantidad_vendida,
                    SUM(v.total_neto) AS total_generado
                FROM FactVentas v
                JOIN DimProducto p ON v.producto_id = p.producto_id
                JOIN DimCategoria c ON v.categoria_id = c.categoria_id
                GROUP BY p.nombre, p.marca, c.nombre
                ORDER BY cantidad_vendida DESC", conn);

            var resultado = new List<TopProductoDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                resultado.Add(new TopProductoDto
                {
                    Nombre = reader.GetString(0),
                    Marca = reader.GetString(1),
                    Categoria = reader.GetString(2),
                    CantidadVendida = reader.GetInt32(3),
                    TotalGenerado = reader.GetDecimal(4),
                });
            }
            return resultado;
        }

        public async Task<List<TopClienteDto>> ObtenerTopClientesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT TOP 10
                       cl.nombre_completo, cl.ciudad, cl.estado, cl.rango_edad,
                       SUM(v.total_neto) AS total_gastado,
                       COUNT(DISTINCT v.carrito_id) AS cantidad_ordenes
                FROM FactVentas v
                INNER JOIN DimCliente cl ON v.cliente_id = cl.cliente_id
                GROUP BY cl.nombre_completo, cl.ciudad, cl.estado, cl.rango_edad
                ORDER BY total_gastado DESC", conn);

            var resultado = new List<TopClienteDto>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultado.Add(new TopClienteDto
                {
                    NombreCompleto = reader.GetString(0),
                    Ciudad = reader.GetString(1),
                    Estado = reader.GetString(2),
                    RangoEdad = reader.GetString(3),
                    TotalGastado = reader.GetDecimal(4),
                    CantidadOrdenes = reader.GetInt32(5)
                });
            }
            return resultado;
        }
        
        public async Task<List<VentasPorTrimestreDto>> ObtenerVentasPorTrimestreAsync()
        {
            using var conn = new SqlConnection (_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT
                    f.trimestre,
                    SUM(v.total_neto) AS total_vendido,
                    COUNT(DISTINCT v.carrito_id) AS cantidad_ordenes
                FROM FactVentas v
                JOIN DimFecha f ON v.fecha_id = f.fecha_id
                GROUP BY f.trimestre
                ORDER BY f.trimestre", conn);

            var resultado = new List<VentasPorTrimestreDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) 
            { 
                var trimestre = reader.GetByte(0);
                resultado.Add(new VentasPorTrimestreDto
                {
                    Trimestre = trimestre,
                    NombreTrimestre = $"Q{trimestre} 2024",
                    TotalVendido = reader.GetDecimal(1),
                    CantidadOrdenes = reader.GetInt32(2)
                });
            }
            return resultado;
        }

        public async Task<List<PrediccionDto>> ObtenerPrediccionesAsync() 
        {
            using var conn = new SqlConnection (_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT mes, nombre_mes, anio, total_predicho, tipo
                FROM FactPredicciones
                ORDER BY anio, mes", conn);

            var resultado = new List<PrediccionDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new PrediccionDto
                {
                    Mes = reader.GetByte(0),
                    NombreMes = reader.GetString(1),
                    Anio = reader.GetInt16(2),
                    TotalVendido = reader.GetDecimal(3),
                    Tipo = reader.GetString(4)
                });
            }
            return resultado;
        }

    }
}
