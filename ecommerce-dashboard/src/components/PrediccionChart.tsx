import {
  LineChart, Line, XAxis, YAxis, CartesianGrid,
  Tooltip, ResponsiveContainer, Legend, ReferenceLine
} from 'recharts';
import { PrediccionDto } from '../types';

interface Props {
  data: PrediccionDto[];
}

export default function PrediccionChart({ data }: Props) {
  const chartData = data.map(d => ({
    nombre: `${d.nombreMes} ${d.anio}`,
    historico: d.tipo === 'historico' ? d.totalVendido : null,
    prediccion: d.tipo === 'prediccion' ? d.totalVendido : null,
  }));

  return (
    <div style={{
      backgroundColor: 'white',
      borderRadius: '12px',
      padding: '24px',
      boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      marginBottom: '24px'
    }}>
      <h3 style={{ margin: '0 0 8px', color: '#111827' }}>
        Ventas Históricas + Predicción 2025
      </h3>
      <p style={{ margin: '0 0 24px', color: '#6b7280', fontSize: '14px' }}>
        Modelo de regresión lineal — tendencia calculada sobre datos de 2024
      </p>
      <ResponsiveContainer width="100%" height={350}>
        <LineChart data={chartData}>
          <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
          <XAxis dataKey="nombre" tick={{ fontSize: 10 }} angle={-25} textAnchor="end" height={60} />
          <YAxis tick={{ fontSize: 12 }} />
          <Tooltip formatter={(value: number | undefined) => [`$${(value ?? 0).toFixed(2)}`, '']} />
          <Legend />
          <ReferenceLine x="Diciembre 2024" stroke="#d1d5db" strokeDasharray="4 4" label={{ value: 'hoy', fontSize: 11 }} />
          <Line
            type="monotone"
            dataKey="historico"
            stroke="#6366f1"
            strokeWidth={2}
            dot={{ r: 4 }}
            name="Histórico 2024"
            connectNulls={false}
          />
          <Line
            type="monotone"
            dataKey="prediccion"
            stroke="#f59e0b"
            strokeWidth={2}
            strokeDasharray="6 3"
            dot={{ r: 4 }}
            name="Predicción 2025"
            connectNulls={false}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}