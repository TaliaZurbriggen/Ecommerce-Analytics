import { useEffect, useState } from "react";
import { 
  getKpi, getVentasPorVes, getVentasPorCategoria,
  getTopProductos, getTopClientes, getVentasPorTrimestre, getPredicciones
} from "./services/api";
import { 
  KpisDto, VentasPorMesDto, VentasPorCategoriaDto,
  TopProductoDto, TopClientesDto, VentasPorTrimestreDto, PrediccionDto
} from "./types";
import KpiCard from "./components/KpiCard";
import VentasPorMesChart from "./components/VentasPorMesChart";
import VentasPorCategoriaCharts from "./components/VentasPorCategoriaChart";
import TopProductosChart from "./components/TopProductosChart";
import TopClientesTable from "./components/TopClientesTable";
import VentasPorTrimestreChart from "./components/VentasPorTrimestreChart";
import PrediccionChart from "./components/PrediccionChart";

export default function App(){
  const [kpis, setKpis] = useState<KpisDto | null>(null);
  const [ventasMes, setVentasMes] = useState<VentasPorMesDto[]>([]);
  const [ventasCategoria, setVentasCategoria] = useState<VentasPorCategoriaDto[]>([]);
  const [topProductos, setTopProductos] = useState<TopProductoDto[]>([]);
  const [topClientes, setTopClientes] = useState<TopClientesDto[]>([]);
  const [ventasTrimestre, setVentasTrimestre] = useState<VentasPorTrimestreDto[]>([]);
  const [predicciones, setPredicciones] = useState<PrediccionDto[]>([]);
  const [cargando, setCargando] = useState(true);

  useEffect(()=>{
    Promise.all([
      getKpi(),
      getVentasPorVes(),
      getVentasPorCategoria(),
      getTopProductos(),
      getTopClientes(),
      getVentasPorTrimestre(),
      getPredicciones()
    ]).then(([k, vm, vc, tp, tc, vt, pred]) =>{
      setKpis(k);
      setVentasMes(vm);
      setVentasCategoria(vc);
      setTopProductos(tp);
      setTopClientes(tc);
      setVentasTrimestre(vt);
      setPredicciones(pred);
      setCargando(false);   
    });
  }, []);

  if(cargando) return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', backgroundColor: '#f9fafb'}}>
      <p style={{ color: '#6b7280', fontSize: '18px' }}>Cargando dashboard...</p>
    </div>
  );

  return (
    <div style={{ backgroundColor: '#f9fafb', minHeight: '100vh', padding: '32px' }}>
      <div style={{ maxWidth: '1400px', margin: '0 auto' }}>

        {/* Header */}
        <div style={{ marginBottom: '32px' }}>
          <h1 style={{ margin: 0, color: '#111827', fontSize: '28px' }}>Ecommerce Analytics</h1>
          <p style={{ margin: '4px 0 0', color: '#6b7280' }}>Resumen de ventas 2024</p>
        </div>

        {/* KPIs */}
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '16px', marginBottom: '24px' }}>
          <KpiCard titulo="Total Vendido" valor={`$${kpis!.totalVendido.toFixed(2)}`} descripcion="Ingresos netos 2024" color="#6366f1" />
          <KpiCard titulo="Órdenes" valor={kpis!.cantidadOrdenes.toString()} descripcion="Carritos completados" color="#10b981" />
          <KpiCard titulo="Ticket Promedio" valor={`$${kpis!.ticketPromedio.toFixed(2)}`} descripcion="Por orden" color="#f59e0b" />
          <KpiCard titulo="Unidades Vendidas" valor={kpis!.totalProductosVendidos.toString()} descripcion="Productos despachados" color="#ef4444" />
        </div>

        <PrediccionChart data={predicciones} />
        {/* Gráficos fila 1 */}
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', marginBottom: '24px' }}>
          <VentasPorMesChart data={ventasMes} />
          <VentasPorTrimestreChart data={ventasTrimestre} />
        </div>

        {/* Gráficos fila 2 */}
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', marginBottom: '24px' }}>
          <VentasPorCategoriaCharts data={ventasCategoria} />
          <TopProductosChart data={topProductos} />
        </div>

        {/* Tabla clientes */}
        <TopClientesTable data={topClientes} />

      </div>
    </div>
  );
}
