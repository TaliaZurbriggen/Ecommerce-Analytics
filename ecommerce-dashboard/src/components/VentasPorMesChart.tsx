import { 
    LineChart, Line, XAxis, YAxis, CartesianGrid,
    Tooltip, ResponsiveContainer
 } from "recharts";
 import { VentasPorMesDto } from "../types";

 interface Props{
    data: VentasPorMesDto[];
 }

 export default function VentasPorMesChart ({data}: Props){
    return(
        <div style={{ backgroundColor: 'white', borderRadius: '12px', padding: '24px', boxShadow: '0 2px 8px rgba(0,0,0,0.08)'}}>
            <h3 style={{ margin: '0 0 24px', color: '#111827'}}>Ventas por Mes</h3>
            <ResponsiveContainer width="100%" height={300}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                    <LineChart data={data}>
                        <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                        <XAxis dataKey="nombreMes" tick={{ fontSize: 12 }} />
                        <YAxis tick={{ fontSize: 12 }} />
                        <Tooltip formatter={(value: number | undefined) => [`$${(value ?? 0).toFixed(2)}`, 'Total vendido']} />
                        <Line type="monotone" dataKey="totalVendido" stroke="#6366f1" strokeWidth={2} dot={{ r: 4 }} />
                    </LineChart>
            </ResponsiveContainer>
        </div>
    );
 }