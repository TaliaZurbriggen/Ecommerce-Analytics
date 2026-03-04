using EcommerceETL.Services;

namespace EcommerceETL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string CONNECTION_STRING =
                "Server=DESKTOP-G88OCVU;Database=EcommerceAnalytics;Trusted_Connection=True;TrustServerCertificate=True;";

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     ETL - Ecommerce Analytics        ║");
            Console.WriteLine("╚══════════════════════════════════════╝\n");

            var extract = new ExtractService();
            var transform = new TransformService();
            var load = new LoadService(CONNECTION_STRING);

            try
            {
                Console.WriteLine("EXTRACCIÓN");
                var apiProductos = await extract.ObtenerProductosAsync();
                var apiCarritos = await extract.ObtenerCarritosAsync();
                var apiUsuarios = await extract.ObtenerUsuariosAsync();
                Console.WriteLine();

                Console.WriteLine("TRANSFORMACIÓN");
                var fechas = transform.TransformarFechas();
                var (categorias, mapCategorias) = transform.TransformarCategorias(apiProductos);
                var productos = transform.TransformarProductos(apiProductos, mapCategorias);
                var clientes = transform.TransformarClientes(apiUsuarios);
                var mapProductoCategorias = productos.ToDictionary(p => p.ProductoId, p => p.CategoriaId);
                var ventas = transform.TransformarVentas(apiCarritos, mapCategorias, mapProductoCategorias);
                Console.WriteLine();

                await load.LimpiarTablaAsync();

                Console.WriteLine("CARGA EN BASE DE DATOS");
                await load.CargarFechasAsync(fechas);
                var mapeoCategoriasReal = await load.CargarCategoriasAsync(categorias);

                foreach (var p in productos)
                {
                    p.CategoriaId = mapeoCategoriasReal[
                        categorias.First(c => c.CategoriaId == p.CategoriaId).Nombre];
                }

                foreach (var v in ventas)
                {
                    var prod = productos.First(p => p.ProductoId == v.ProductoId);
                    v.CategoriaId = prod.CategoriaId;
                }

                await load.CargarProductosAsync(productos);
                await load.CargarClientesAsync(clientes);
                await load.CargarVentasAsync(ventas);
                Console.WriteLine();

                Console.WriteLine("ETL COMPLETADO EXITOSAMENTE");
                Console.WriteLine($"Fechas cargadas: {fechas.Count}");
                Console.WriteLine($"Categorías cargadas: {categorias.Count}");
                Console.WriteLine($"Productos cargados: {productos.Count}");
                Console.WriteLine($"Clientes cargados: {clientes.Count}");
                Console.WriteLine($"Ventas cargadas: {ventas.Count}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\n ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
