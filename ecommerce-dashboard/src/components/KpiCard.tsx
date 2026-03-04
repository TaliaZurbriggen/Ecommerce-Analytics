interface Props {
    titulo: string;
    valor: string;
    descripcion: string;
    color: string;
}

export default function KpiCard({titulo, valor, descripcion, color}: Props){
    return(
        <div style={{
            backgroundColor: 'white',
            borderRadius: '12px',
            padding: '24px',
            borderLeft: `4px solid ${color}`,
            boxShadow: '0 2px 8px rgba(0,0,0,0,0.08)'
        }}>
            <p style={{ color: '#6b7280', fontSize: '14px', margin: 0}}>{titulo}</p>
            <p style={{fontSize: '28px', fontWeight: 'bold', margin: '8px 0', color: '#111827'}}> {valor}</p>
            <p style={{color: '#9ca3af', fontSize: '12px', margin: 0}}>{descripcion}</p>
        </div>
    );
}