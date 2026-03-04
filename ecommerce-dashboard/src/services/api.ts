import axios from "axios";
import { 
    KpisDto,
    VentasPorMesDto,
    VentasPorCategoriaDto,
    TopProductoDto,
    TopClientesDto,
    VentasPorTrimestreDto
 } from "../types";

 const API_BASE = 'https://localhost:7269/api/analytics';

 export const getKpi = () =>
    axios.get<KpisDto>(`${API_BASE}/kpis`).then(r => r.data);

 export const getVentasPorVes = () =>
    axios.get<VentasPorMesDto[]>(`${API_BASE}/ventas/por-mes`).then(r => r.data);

 export const getVentasPorCategoria = () =>
    axios.get<VentasPorCategoriaDto[]>(`${API_BASE}/ventas/por-categoria`).then(r => r.data);

 export const getTopProductos = () =>
    axios.get<TopProductoDto[]>(`${API_BASE}/productos/top`).then(r => r.data);

 export const getTopClientes = () =>
    axios.get<TopClientesDto[]>(`${API_BASE}/clientes/top`).then(r => r.data);

 export const getVentasPorTrimestre = () =>
    axios.get<VentasPorTrimestreDto[]>(`${API_BASE}/ventas/por-trimestre`).then(r => r.data);