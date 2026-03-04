# Ecommerce Analytics — ETL + Reporting Pipeline

Dashboard de análisis de datos para un e-commerce, construido con un pipeline ETL completo: extracción desde una API pública, transformación y carga en un Data Warehouse, y visualización interactiva en un dashboard web.

---

## Vista del Dashboard

> *KPIs, gráficos de ventas por mes, categoría, trimestre, top productos y top clientes.*

---

## Arquitectura

```
API Pública (DummyJSON)
        ↓
  ETL Pipeline (.NET)
   Extract → Transform → Load
        ↓
  SQL Server (Data Warehouse)
  Esquema Estrella
        ↓
  Reporting API (.NET Web API)
        ↓
  Dashboard (React + Recharts)
```

---

## Esquema Estrella (Data Warehouse)

```
                ┌──────────────┐
                │   DimFecha   │
                └──────┬───────┘
                       │
┌──────────────┐ ┌─────▼──────────────┐ ┌──────────────────┐
│  DimCliente  ├─►    FactVentas      ◄─┤   DimProducto    │
└──────────────┘ │  venta_id          │ └──────────────────┘
                 │  carrito_id        │
                 │  fecha_id     FK   │ ┌──────────────────┐
                 │  cliente_id   FK   ◄─┤  DimCategoria    │
                 │  producto_id  FK   │ └──────────────────┘
                 │  categoria_id FK   │
                 │  cantidad          │
                 │  total_neto        │
                 └────────────────────┘
```

---

## Métricas del Dashboard

| Sección | Descripción |
|---|---|
| KPIs | Total vendido, órdenes, ticket promedio, unidades vendidas |
| Ventas por Mes | Línea de tendencia mensual |
| Ventas por Trimestre | Comparativa Q1-Q4 |
| Ventas por Categoría | Barras horizontales por categoría |
| Top 10 Productos | Productos más vendidos por unidades |
| Top 10 Clientes | Clientes con mayor gasto total |

---

## Stack Tecnológico

| Capa | Tecnología |
|---|---|
| ETL | .NET 8 (Console App) |
| Base de datos | SQL Server |
| API | ASP.NET Core Web API |
| Frontend | React 18 + TypeScript |
| Gráficos | Recharts |
| HTTP Client | Axios |
| Datos | DummyJSON API |

---

## Estructura del Proyecto

```
Ecommerce/
├── EcommerceETL/                  # Pipeline ETL
│   ├── Models/
│   │   ├── ApiModels/             # Modelos de la API cruda
│   │   └── DbModels/              # Modelos de base de datos
│   ├── Services/
│   │   ├── ExtractService.cs      # Consume DummyJSON
│   │   ├── TransformService.cs    # Limpia y transforma
│   │   └── LoadService.cs         # Inserta en SQL Server
│   ├── Helpers/
│   │   └── DateHelper.cs          # Generación de fechas simuladas
│   └── Program.cs                 # Orquestador del pipeline
│
├── EcommerceReporting/            # API de Reporting
│   ├── Controllers/
│   │   └── AnalyticsController.cs # Endpoints REST
│   ├── Models/
│   │   └── ReportModels.cs        # DTOs de respuesta
│   ├── Services/
│   │   └── QueryService.cs        # Queries analíticas
│   └── Program.cs
│
└── ecommerce-dashboard/           # Frontend React
    └── src/
        ├── components/            # Gráficos y tablas
        ├── services/
        │   └── api.ts             # Llamadas a la API
        ├── types/
        │   └── index.ts           # Tipos TypeScript
        └── App.tsx                # Dashboard principal
```

---

## Cómo ejecutarlo localmente

### Requisitos previos
- .NET 8 SDK
- SQL Server (local)
- Node.js 18+
- SQL Server Management Studio (opcional)

### 1. Base de datos

Abrís SQL Server Management Studio y ejecutás:

```sql
CREATE DATABASE EcommerceAnalytics;
```

Después ejecutás el script `01_crear_esquema.sql` para crear las tablas.

### 2. ETL — Cargar los datos

```bash
cd EcommerceETL
```

Editás `Program.cs` y reemplazás `TU_SERVIDOR` con el nombre de tu instancia de SQL Server:

```csharp
const string CONNECTION_STRING =
    "Server=TU_SERVIDOR;Database=EcommerceAnalytics;Trusted_Connection=True;TrustServerCertificate=True;";
```

```bash
dotnet run
```

Deberías ver en consola los pasos del pipeline con el resumen final.

### 3. API de Reporting

```bash
cd EcommerceReporting
```

Mismo cambio de `TU_SERVIDOR` en `Program.cs`, luego:

```bash
dotnet run
```

La API queda disponible en `https://localhost:7269`. Podés probar los endpoints en Swagger: `https://localhost:7269/swagger`

### 4. Dashboard

```bash
cd ecommerce-dashboard
npm install
npm start
```

El dashboard abre en `http://localhost:3000`.

> ⚠️ La API de Reporting tiene que estar corriendo antes de levantar el dashboard.

---

## 🔌 Endpoints de la API

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/analytics/kpis` | KPIs generales |
| GET | `/api/analytics/ventas/por-mes` | Ventas agrupadas por mes |
| GET | `/api/analytics/ventas/por-categoria` | Ventas por categoría |
| GET | `/api/analytics/ventas/por-trimestre` | Comparativa trimestral |
| GET | `/api/analytics/productos/top` | Top 10 productos |
| GET | `/api/analytics/clientes/top` | Top 10 clientes |

---

## Fuente de datos

Los datos provienen de [DummyJSON](https://dummyjson.com/), una API pública que simula un e-commerce con productos, carritos de compra y usuarios.

Las fechas de compra son **simuladas** dentro del año 2024 — práctica común cuando se trabaja con datasets sin fechas reales.

---

## 💡 Conceptos aplicados

- **Esquema estrella** — diseño estándar de Data Warehousing con tabla de hechos y dimensiones
- **Pipeline ETL** — separación clara de responsabilidades: Extract, Transform, Load
- **Separación de responsabilidades** — cada servicio tiene una única función
- **API RESTful** — endpoints semánticos con DTOs tipados
- **Componentes reutilizables** — cada gráfico es un componente independiente en React
