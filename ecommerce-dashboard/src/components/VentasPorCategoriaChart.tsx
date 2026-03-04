import { 
    BarChart, Bar, XAxis, YAxis, CartesianGrid,
    Tooltip, ResponsiveContainer
} from "recharts";
import { VentasPorCategoriaDto } from "../types";

interface Props{
    data: VentasPorCategoriaDto[];
}

export default function VentasPorCategoriaCharts ({data} : Props){
    return(
        <div style={{backgroundColor: 'white', borderRadius: '12px', padding: '24px', boxShadow: '0 2px 8px rgba(0,0,0,0.08)'}}>
            <h3 style={{ margin: '0 0 24px', color: '#111827' }}>Ventas por Categoría</h3>
            <ResponsiveContainer width="100%" height={300}>
                <BarChart data={data} layout="vertical">
                    <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0"/>
                    <XAxis type="number" tick={{fontSize: 11}} />
                    <YAxis dataKey="categoria" type="category" tick={{fontSize: 11}} width={100} />
                    <Tooltip formatter={(value: number | undefined) => [`$${(value ?? 0).toFixed(2)}`, 'Total vendido']} />
                    <Bar dataKey="totalVendido" fill="#6366f1" radius={[0, 4, 4, 0]}/>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}