using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceETL.Helpers;
using EcommerceETL.Models.ApiModels;
using EcommerceETL.Models.DbModels;

namespace EcommerceETL.Services
{
    public class TransformService
    {
        public List<DimFecha> TransformarFechas()
        {
            Console.WriteLine("Generando DimFecha (365 días de 2024)...");
            var fechas = DateHelper.GenerarDimFecha(2024);
            Console.WriteLine($"{fechas.Count} fechas generadas");
            return fechas;
        }

        public (List<DimCategoria> categorias, Dictionary<string, int> mapeo)
            TransformarCategorias(List<ApiProduct> productos)
        {
            Console.WriteLine("Generando DimCategoria...");

            var nombresUnicos = productos
                .Select(p => p.Category.Trim().ToLower())
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var categorias = new List<DimCategoria>();

            var mapeo = new Dictionary<string, int>();

            int id = 1;
            foreach (var nombre in nombresUnicos)
            {
                categorias.Add(new DimCategoria
                {
                    CategoriaId = id,
                    Nombre = nombre
                });
                mapeo[nombre] = id;
                id++;
            }

            Console.WriteLine($"{categorias.Count} categorias únicas");
            return (categorias,  mapeo);
        }

        public List<DimProducto> TransformarProductos(
            List<ApiProduct> apiProductos,
            Dictionary<string, int> mapCategorias)
        {
            Console.WriteLine("Generando DimProducto...");

            var productos = apiProductos.Select(p => new DimProducto
            {
                ProductoId = p.Id,
                Nombre = p.Title.Trim(),
                Marca = string.IsNullOrWhiteSpace(p.Brand) ? "Sin marca" : p.Brand.Trim(),
                CategoriaId = mapCategorias[p.Category.Trim().ToLower()],
                PrecioUnitario = p.Price,
                Stock = p.Stock,
                Rating = p.Rating,
            }).ToList();

            Console.WriteLine($"{productos.Count} productos transformados");
            return productos;
        }

        public List<DimCliente> TransformarClientes(List<ApiUser> apiUsuarios)
        {
            Console.WriteLine("Generando DimCliente...");

            var clientes = apiUsuarios.Select(u => new DimCliente
            {
                ClienteId = u.Id,
                NombreCompleto = $"{u.FirstName.Trim()} {u.LastName.Trim()}",
                Email = u.Email.Trim().ToLower(),
                Edad = u.Age,
                RangoEdad = DateHelper.CalcularRangoEdad(u.Age),
                Ciudad = u.Address.City.Trim(),
                Estado = u.Address.State.Trim(),
            }).ToList();

            Console.WriteLine($"{clientes.Count} clientes transformados");
            return clientes;
        }

        public List<FactVentas> TransformarVentas(
            List<ApiCart> apiCarritos,
            Dictionary<string, int> mapCategorias,
            Dictionary<int, int> mapProductoCategorias)
        {
            Console.WriteLine("Generando FactVentas...");

            var ventas = new List<FactVentas>();

            foreach (var carrito in apiCarritos)
            {
                //Genero una fecha simulada para todo el carrito
                var fecha = DateHelper.GenerarFechaAleatoria();
                var fechaId = DateHelper.ToFechaId(fecha);

                foreach (var item in carrito.Products)
                {
                    var totalBruto = item.Price * item.Quantity;
                    var montoDescuento = totalBruto * (item.DiscountPercentage / 100);
                    var totalNeto = totalBruto - montoDescuento;

                    //Si el producto no esta en el mapa lo salto para no romper la FK
                    if(!mapProductoCategorias.TryGetValue(item.Id, out int categoriaId))
                    {
                        Console.WriteLine($"Producto {item.Id} no encontrado, se omite.");
                        continue;
                    }

                    ventas.Add(new FactVentas
                    {
                        CarritoId = carrito.Id,
                        FechaId = fechaId,
                        ClienteId = carrito.UserId,
                        ProductoId = item.Id,
                        CategoriaId = categoriaId,
                        Cantidad = item.Quantity,
                        PrecioUnitario = item.Price,
                        DescuentoPct = item.DiscountPercentage,
                        MontoDescuento = Math.Round(montoDescuento, 2),
                        TotalBruto = Math.Round(totalBruto, 2),
                        TotalNeto = Math.Round(totalNeto, 2)
                    });
                }
            }

            Console.WriteLine($"{ventas.Count} filas de ventas generadas");
            return ventas;
        }
    }
}
