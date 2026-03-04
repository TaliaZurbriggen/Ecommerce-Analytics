# Ecommerce Analytics — ETL + Reporting Pipeline + ML

Dashboard de análisis de datos para un e-commerce, construido con un pipeline ETL completo: extracción desde una API pública, transformación y carga en un Data Warehouse, modelo de predicción de ventas con Machine Learning, y visualización interactiva en un dashboard web.

---

## Vista del Dashboard

<img width="1607" height="837" alt="image" src="https://github.com/user-attachments/assets/0ec4eb35-a115-4c8d-8cc7-9c64cfd43672" />
<img width="1896" height="473" alt="image" src="https://github.com/user-attachments/assets/9050c2b5-9767-47ec-83ef-02177f3bb3cb" />
<img width="1597" height="467" alt="image" src="https://github.com/user-attachments/assets/2c6d538c-1c95-40be-bea6-472a19faaadc" />
<img width="1598" height="621" alt="image" src="https://github.com/user-attachments/assets/fb00b896-0445-453e-baf7-0a0cf11e7d5b" />

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
  Python + scikit-learn
  Modelo de Regresión Lineal
        ↓
  SQL Server (FactPredicciones)
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

┌──────────────────────┐
│   FactPredicciones   │  ← generada por el modelo de ML
│  mes                 │
│  nombre_mes          │
│  anio                │
│  total_predicho      │
│  tipo (historico /   │
│        prediccion)   │
└──────────────────────┘
```

---

## Métricas del Dashboard

| Sección | Descripción |
|---|---|
| KPIs | Total vendido, órdenes, ticket promedio, unidades vendidas |
| Predicción de Ventas | Histórico 2024 + predicción Ene-Jun 2025 (regresión lineal) |
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
| Machine Learning | Python 3.13 + scikit-learn + pandas |
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
├── ecommerce-dashboard/           # Frontend React
│   └── src/
│       ├── components/            # Gráficos y tablas
│       ├── services/
│       │   └── api.ts             # Llamadas a la API
│       ├── types/
│       │   └── index.ts           # Tipos TypeScript
│       └── App.tsx                # Dashboard principal
│
└── prediccion_ventas.py           # Modelo de ML (regresión lineal)
```

---

## Cómo ejecutarlo localmente

### Requisitos previos
- .NET 8 SDK
- SQL Server (local)
- Python 3.13+
- Node.js 18+

### 1. Base de datos

En SQL Server Management Studio:

```sql
CREATE DATABASE EcommerceAnalytics;
```

Ejecutás un script para crear las tablas.

### 2. ETL — Cargar los datos

Editás `EcommerceETL/Program.cs` y reemplazás `TU_SERVIDOR`:

```bash
cd EcommerceETL
dotnet run
```

### 3. Modelo de predicción

Instalás las dependencias de Python:

```bash
pip install pandas scikit-learn pyodbc
```

Editás `prediccion_ventas.py` y reemplazás `TU_SERVIDOR`, luego:

```bash
python prediccion_ventas.py
```

### 4. API de Reporting

Editás `EcommerceReporting/Program.cs` y reemplazás `TU_SERVIDOR`:

```bash
cd EcommerceReporting
dotnet run
```

La API queda disponible en `https://localhost:7269`.
Swagger: `https://localhost:7269/swagger`

### 5. Dashboard

```bash
cd ecommerce-dashboard
npm install
npm start
```

Dashboard en `http://localhost:3000`.

> ⚠️ La API de Reporting tiene que estar corriendo antes de levantar el dashboard.

---

## Endpoints de la API

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/analytics/kpis` | KPIs generales |
| GET | `/api/analytics/ventas/por-mes` | Ventas agrupadas por mes |
| GET | `/api/analytics/ventas/por-categoria` | Ventas por categoría |
| GET | `/api/analytics/ventas/por-trimestre` | Comparativa trimestral |
| GET | `/api/analytics/productos/top` | Top 10 productos |
| GET | `/api/analytics/clientes/top` | Top 10 clientes |
| GET | `/api/analytics/predicciones` | Histórico + predicción ML |

---

## Modelo de Machine Learning

Se usa **regresión lineal** de scikit-learn para predecir las ventas de los próximos 6 meses basándose en el historial de 2024.

- **Variable de entrada (X):** número de mes (1-12)
- **Variable de salida (Y):** total vendido ese mes
- **Predicción:** meses 13-18 → Enero-Junio 2025

Los resultados se guardan en `FactPredicciones` con un campo `tipo` que diferencia datos históricos de predicciones, permitiendo mostrar ambas series en el mismo gráfico.

---

## Fuente de datos

Los datos provienen de [DummyJSON](https://dummyjson.com/), una API pública que simula un e-commerce con productos, carritos de compra y usuarios. Las fechas de compra son simuladas dentro del año 2024.

---

## Conceptos aplicados

- **Esquema estrella** — diseño estándar de Data Warehousing
- **Pipeline ETL** — separación clara de Extract, Transform, Load
- **Regresión lineal** — modelo de ML supervisado para predicción de series temporales
- **Separación de responsabilidades** — cada servicio tiene una única función
- **API RESTful** — endpoints semánticos con DTOs tipados
- **Componentes reutilizables** — cada gráfico es un componente independiente en React
