import { 
    BarChart, Bar, XAxis, YAxis, CartesianGrid,
    Tooltip, ResponsiveContainer
} from "recharts";
import { TopProductoDto } from "../types";

interface Props{
    data: TopProductoDto[];
}

export default function TopProductosChart({data} : Props){
    return(
        <div style={{backgroundColor: 'white', borderRadius: '12px', padding: '24px', boxShadow: '0 2px 8px rgba(0,0,0,0.08)' }}>
            <h3 style={{margin: '0 0 24px', color: '#111827'}}>Top 10 Productos mas Vendidos</h3>
            <ResponsiveContainer width="100%" height={300}>
                <BarChart data={data}>
                    <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0"/>
                    <XAxis dataKey="nombre" tick={{fontSize: 10}} angle={-20} textAnchor="end" height={50} />
                    <YAxis tick={{fontSize: 12}} />
                    <Tooltip formatter={(value: number | undefined) => [value ?? 0, 'Unidades vendidas']}/>
                    <Bar dataKey="cantidadVendida" fill="#10b981" radius={[4, 4, 0, 0]}/>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}