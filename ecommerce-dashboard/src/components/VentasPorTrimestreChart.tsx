import { 
    BarChart, Bar, XAxis, YAxis, CartesianGrid,
    Tooltip, ResponsiveContainer
} from "recharts";
import { VentasPorTrimestreDto } from "../types";

interface Props{
    data: VentasPorTrimestreDto[];
}

export default function VentasPorTrimestreChart ({data} : Props){
    return(
        <div style={{backgroundColor: 'white', borderRadius: '12px', padding: '24px', boxShadow: '0 2px 8px rgba(0,0,0,0.08)'}}>
            <h3 style={{margin: '0 0 24px', color: '#111827'}}>Ventas por Trimestre</h3>
            <ResponsiveContainer width="100%" height={300}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                    <BarChart data={data}>
                        <XAxis dataKey="nombreTrimestre" tick={{ fontSize: 12}}/>
                        <YAxis tick={{fontSize: 12}} />
                        <Tooltip formatter={(value: number | undefined) => [`$${(value ?? 0).toFixed(2)}`, 'Total vendido']} />
                        <Bar dataKey="totalVendido" fill="#f59e0b" radius={[4, 4, 0, 0]} />
                    </BarChart>
            </ResponsiveContainer>
        </div>
    );
}