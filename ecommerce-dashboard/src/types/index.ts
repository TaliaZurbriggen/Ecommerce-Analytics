export interface KpisDto{
    totalVendido : number;
    cantidadOrdenes: number;
    ticketPromedio: number;
    totalProductosVendidos: number;
}

export interface VentasPorMesDto{
    mes: number;
    nombreMEs: string;
    totalVendido: number;
    cantidadOrdenes: number;
}

export interface VentasPorCategoriaDto{
    categoria: string;
    totalVendido: number;
    cantidadProductosVendidos: number;
}

export interface TopProductoDto{
    nombre: string;
    marca: string;
    categoria: string;
    cantidadVendida: number;
    totalGenerado: number;
}

export interface TopClientesDto{
    nombreCompleto: string;
    ciudad: string;
    estado:string;
    rangoEdad: string;
    totalGastado: number;
    cantidadOrdenes: number;
}

export interface VentasPorTrimestreDto{
    trimestre: number;
    nombreTrimestre: string;
    totalVendido: number;
    cantidadOrdenes: number;
}