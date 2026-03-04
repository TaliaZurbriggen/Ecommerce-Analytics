using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using EcommerceETL.Models.DbModels;

namespace EcommerceETL.Services
{
    public class LoadService
    {
        private readonly string _connectionString;

        public LoadService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CargarFechasAsync(List<DimFecha> fechas)
        {
            Console.WriteLine("Cargando DimFecha... ");
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            foreach (var f in fechas)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO DimFecha
                        (fecha_id, fecha, dia, mes, nombre_mes, trimestre, anio, dia_semana, es_fin_de_semana)
                    VALUES
                        (@fechaId, @fecha, @dia, @mes, @nombreMes, @trimestre, @anio, @diaSemana, @esFinDeSemana)", 
                    conn);

                cmd.Parameters.AddWithValue("@fechaId", f.FechaId);
                cmd.Parameters.AddWithValue("@fecha", f.Fecha);
                cmd.Parameters.AddWithValue("@dia", f.Dia);
                cmd.Parameters.AddWithValue("@mes", f.Mes);
                cmd.Parameters.AddWithValue("@nombreMes", f.NombreMes);
                cmd.Parameters.AddWithValue("@trimestre", f.Trimestre);
                cmd.Parameters.AddWithValue("@anio", f.Anio);
                cmd.Parameters.AddWithValue("@diaSemana", f.DiaSemana);
                cmd.Parameters.AddWithValue("@esFinDeSemana", f.EsFinDeSemana);

                await cmd.ExecuteNonQueryAsync();
            }

            Console.WriteLine($"{fechas.Count} fechas cargadas");
        }

        public async Task<Dictionary<string, int>> CargarCategoriasAsync(List<DimCategoria> categorias)
        {
            Console.WriteLine("Cargando DimCategoria...");
            var mapeo = new Dictionary<string, int>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            foreach (var c in categorias)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO DimCategoria (nombre) VALUES (@nombre);
                    SELECT SCOPE_IDENTITY();",
                    conn);

                cmd.Parameters.AddWithValue("@nombre", c.Nombre);
                var idGenerado = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                mapeo[c.Nombre] = idGenerado;
            }

            Console.WriteLine($"{categorias.Count} categorías cargadas");
            return mapeo;
        }

        public async Task CargarProductosAsync(List<DimProducto> productos)
        {
            Console.WriteLine("Cargando DimProducto");

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            foreach (var p in productos)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO DimProducto
                        (producto_id, nombre, marca, categoria_id, precio_unitario, stock, rating)
                    VALUES
                        (@productoId, @nombre, @marca, @categoriaId, @precioUnitario, @stock, @rating)",
                    conn);

                cmd.Parameters.AddWithValue("@productoId", p.ProductoId);
                cmd.Parameters.AddWithValue("@nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@marca", p.Marca);
                cmd.Parameters.AddWithValue("@categoriaId", p.CategoriaId);
                cmd.Parameters.AddWithValue("@precioUnitario", p.PrecioUnitario);
                cmd.Parameters.AddWithValue("@stock", p.Stock);
                cmd.Parameters.AddWithValue("@rating", p.Rating);

                await cmd.ExecuteNonQueryAsync();
            }

            Console.WriteLine($"{productos.Count} productos cargados");
        }

        public async Task CargarClientesAsync(List<DimCliente> clientes)
        {
            Console.WriteLine("Cargando DimCliente... ");
            using var conn = new SqlConnection( _connectionString);
            await conn.OpenAsync();

            foreach(var c in clientes)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO DimCliente
                        (cliente_id, nombre_completo, email, edad, rango_edad, ciudad, estado)
                    VALUES
                        (@clienteId, @nombreCompleto, @email, @edad, @rangoEdad, @ciudad, @estado)",
                    conn);

                cmd.Parameters.AddWithValue("@clienteId", c.ClienteId);
                cmd.Parameters.AddWithValue("@nombreCompleto", c.NombreCompleto);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.Parameters.AddWithValue("@edad", c.Edad);
                cmd.Parameters.AddWithValue("@rangoEdad", c.RangoEdad);
                cmd.Parameters.AddWithValue("@ciudad", c.Ciudad);
                cmd.Parameters.AddWithValue("@estado", c.Estado);

                await cmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine($"{clientes.Count} clientes cargados");
        }

        public async Task CargarVentasAsync (List<FactVentas> ventas)
        {
            Console.WriteLine("Cargando FactVentas... ");
            using var conn = new SqlConnection( _connectionString);
            await conn.OpenAsync();

            foreach(var v in ventas)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO FactVentas
                        (carrito_id, fecha_id, cliente_id, producto_id, categoria_id, cantidad, precio_unitario, descuento_pct, monto_descuento, total_bruto, total_neto)
                    VALUES
                        (@carritoId, @fechaId, @clienteId, @productoId, @categoriaId, @cantidad, @precioUnitario, @descuentoPct, @montoDescuento, @totalBruto, @totalNeto)",
                    conn);

                cmd.Parameters.AddWithValue("@carritoId", v.CarritoId);
                cmd.Parameters.AddWithValue("@fechaId", v.FechaId);
                cmd.Parameters.AddWithValue("@clienteId", v.ClienteId);
                cmd.Parameters.AddWithValue("@productoId", v.ProductoId);
                cmd.Parameters.AddWithValue("@categoriaId", v.CategoriaId);
                cmd.Parameters.AddWithValue("@cantidad", v.Cantidad);
                cmd.Parameters.AddWithValue("@precioUnitario", v.PrecioUnitario);
                cmd.Parameters.AddWithValue("@descuentoPct", v.DescuentoPct);
                cmd.Parameters.AddWithValue("@montoDescuento", v.MontoDescuento);
                cmd.Parameters.AddWithValue("@totalBruto", v.TotalBruto);
                cmd.Parameters.AddWithValue("@totalNeto", v.TotalNeto);

                await cmd.ExecuteNonQueryAsync();
            }

            Console.WriteLine($"{ventas.Count} filas de ventas cargadas");
        }

        public async Task LimpiarTablaAsync()
        {
            Console.WriteLine("Limpiando tablas anteriores...");
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                DELETE FROM FactVentas;
                DELETE FROM DimCliente;
                DELETE FROM DimProducto;
                DELETE FROM DimCategoria;
                DELETE FROM DimFecha;",
                conn);

            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Tablas limpias");
        }
    }
}
