using EcommerceETL.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceETL.Helpers
{
    public static class DateHelper
    {
        private static readonly Random _random = new();

        // Convierto una fecha al formato YYYYMMDD que usamos como ID
        public static int ToFechaId(DateTime fecha) =>
            fecha.Year * 10000 + fecha.Month * 100 + fecha.Day;

        // Genero una fecha aleatoria dentro del año 2024
        // Cada carrito va a recibir una fecha distinta
        public static DateTime GenerarFechaAleatoria()
        {
            var inicio = new DateTime(2024, 1, 1);
            var fin = new DateTime(2024, 12, 31);
            int rango = (fin - inicio).Days;
            return inicio.AddDays(_random.Next(rango));
        }

        // Genero TODAS las filas de DimFecha para el año 2024 (365 filas, una por día)
        public static List<DimFecha> GenerarDimFecha(int anio = 2024)
        {
            var fechas = new List<DimFecha>();
            var fecha = new DateTime(anio, 1, 1);
            var fin = new DateTime(anio, 12, 31);

            while (fecha <= fin)
            {
                fechas.Add(new DimFecha
                {
                    FechaId = ToFechaId(fecha),
                    Fecha = fecha,
                    Dia = fecha.Day,
                    Mes = fecha.Month,
                    NombreMes = ObtenerNombreMes(fecha.Month),
                    Trimestre = (fecha.Month - 1) / 3 + 1,
                    Anio = fecha.Year,
                    DiaSemana = ObtenerDiaSemana(fecha.DayOfWeek),
                    EsFinDeSemana = fecha.DayOfWeek == DayOfWeek.Saturday ||
                                   fecha.DayOfWeek == DayOfWeek.Sunday
                });

                fecha = fecha.AddDays(1);
            }

            return fechas;
        }

        // Calcula el rango de edad para DimCliente
        public static string CalcularRangoEdad(int edad) => edad switch
        {
            < 18 => "Menor de 18",
            <= 25 => "18-25",
            <= 35 => "26-35",
            <= 45 => "36-45",
            <= 55 => "46-55",
            <= 65 => "56-65",
            _ => "Mayor de 65"
        };

        private static string ObtenerNombreMes(int mes) => mes switch
        {
            1 => "Enero",
            2 => "Febrero",
            3 => "Marzo",
            4 => "Abril",
            5 => "Mayo",
            6 => "Junio",
            7 => "Julio",
            8 => "Agosto",
            9 => "Septiembre",
            10 => "Octubre",
            11 => "Noviembre",
            12 => "Diciembre",
            _ => string.Empty
        };

        private static string ObtenerDiaSemana(DayOfWeek dia) => dia switch
        {
            DayOfWeek.Monday => "Lunes",
            DayOfWeek.Tuesday => "Martes",
            DayOfWeek.Wednesday => "Miércoles",
            DayOfWeek.Thursday => "Jueves",
            DayOfWeek.Friday => "Viernes",
            DayOfWeek.Saturday => "Sábado",
            DayOfWeek.Sunday => "Domingo",
            _ => string.Empty
        };
    }
}
