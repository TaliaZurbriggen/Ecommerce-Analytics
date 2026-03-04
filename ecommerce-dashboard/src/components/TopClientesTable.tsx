import { TopClientesDto } from "../types";

interface Props {
  data: TopClientesDto[];
}

export default function TopClientesTable({ data }: Props) {
  return (
    <div style={{ backgroundColor: 'white', borderRadius: '12px', padding: '24px', boxShadow: '0 2px 8px rgba(0,0,0,0.08)' }}>
      <h3 style={{ margin: '0 0 24px', color: '#111827' }}>Top 10 Clientes</h3>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '14px' }}>
        <thead>
          <tr style={{ borderBottom: '2px solid #e5e7eb' }}>
            <th style={{ textAlign: 'left', padding: '8px', color: '#6b7280' }}>Cliente</th>
            <th style={{ textAlign: 'left', padding: '8px', color: '#6b7280' }}>Ubicación</th>
            <th style={{ textAlign: 'left', padding: '8px', color: '#6b7280' }}>Edad</th>
            <th style={{ textAlign: 'right', padding: '8px', color: '#6b7280' }}>Órdenes</th>
            <th style={{ textAlign: 'right', padding: '8px', color: '#6b7280' }}>Total gastado</th>
          </tr>
        </thead>
        <tbody>
          {data.map((cliente, i) => (
            <tr key={i} style={{ borderBottom: '1px solid #f3f4f6' }}>
              <td style={{ padding: '10px 8px', fontWeight: 500 }}>{cliente.nombreCompleto}</td>
              <td style={{ padding: '10px 8px', color: '#6b7280' }}>{cliente.ciudad}, {cliente.estado}</td>
              <td style={{ padding: '10px 8px', color: '#6b7280' }}>{cliente.rangoEdad}</td>
              <td style={{ padding: '10px 8px', textAlign: 'right' }}>{cliente.cantidadOrdenes}</td>
              <td style={{ padding: '10px 8px', textAlign: 'right', fontWeight: 600, color: '#6366f1' }}>
                ${cliente.totalGastado.toFixed(2)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}