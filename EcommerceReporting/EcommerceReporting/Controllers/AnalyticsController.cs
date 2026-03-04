using Microsoft.AspNetCore.Mvc;
using EcommerceReporting.Services;

namespace EcommerceReporting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly QueryService _queryService;

        public AnalyticsController(QueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var result = await _queryService.ObtenerKpisAsync();
            return Ok(result);
        }

        [HttpGet("ventas/por-mes")]
        public async Task<IActionResult> GetVentasPorMes()
        {
            var result = await _queryService.ObtenerVentasPorMesAsync();
            return Ok(result);
        }

        [HttpGet("ventas/por-categoria")]
        public async Task<IActionResult> GetVentasPorCategoria()
        {
            var result = await _queryService.ObtenerVentasPorCategoriaAsync();
            return Ok(result);
        }

        [HttpGet("productos/top")]
        public async Task<IActionResult> GetTopProductos()
        {
            var result = await _queryService.ObtenerTopProductosAsync();
            return Ok(result);
        }

        [HttpGet("clientes/top")]
        public async Task<IActionResult> GetClientesTop()
        {
            var result = await _queryService.ObtenerTopClientesAsync();
            return Ok(result);
        }

        [HttpGet("ventas/por-trimestre")]
        public async Task<IActionResult> GetVentasPorTrimestre()
        {
            var result = await _queryService.ObtenerVentasPorTrimestreAsync();
            return Ok(result);
        }
    }
}
