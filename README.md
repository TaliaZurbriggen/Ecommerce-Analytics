# Ecommerce Analytics — ETL + Reporting Pipeline + ML

Dashboard de análisis de datos para un e-commerce construido de punta a punta: desde la extracción de datos de una API pública hasta la visualización interactiva con predicciones de Machine Learning.

---

## 📸 Vista del Dashboard

<img width="1607" height="837" alt="image" src="https://github.com/user-attachments/assets/6c953072-516a-47c7-889f-e3650cd7ccc6" />
<img width="1896" height="473" alt="image" src="https://github.com/user-attachments/assets/534cb38c-a310-401c-a4c4-361142a8d555" />
<img width="1597" height="467" alt="image" src="https://github.com/user-attachments/assets/a849abc9-7bde-49c3-80ce-0822b9149656" />
<img width="1598" height="621" alt="image" src="https://github.com/user-attachments/assets/dd83153c-c118-446f-ad6e-0d3bbe77a059" />


---

## Arquitectura general

```
API Pública (DummyJSON)
        ↓
  ETL Pipeline (.NET 8)
  Extrae, transforma y carga los datos
        ↓
  SQL Server — Data Warehouse
  Esquema estrella (dimensiones + hechos)
        ↓
  Python + scikit-learn
  Entrena un modelo de regresión lineal
  y genera predicciones de ventas
        ↓
  ASP.NET Core Web API
  Expone los datos analíticos
        ↓
  React + TypeScript + Recharts
  Dashboard interactivo
```

---

## ¿Qué muestra el dashboard?

| Sección | Descripción |
|---|---|
| KPIs | Total vendido, cantidad de órdenes, ticket promedio, unidades despachadas |
| Predicción de ventas | Gráfico con datos históricos 2024 + predicción Ene-Jun 2025 |
| Ventas por mes | Tendencia mensual de ingresos |
| Ventas por trimestre | Comparativa Q1, Q2, Q3 y Q4 |
| Ventas por categoría | Categorías de productos ordenadas por ingreso |
| Top 10 productos | Productos más vendidos por unidades |
| Top 10 clientes | Clientes con mayor gasto total |

---

## Diseño de la base de datos

Se usa un **esquema estrella**, patrón estándar en Data Warehousing. Tiene una tabla central de hechos (`FactVentas`) rodeada de tablas de dimensiones que dan contexto a cada venta.

```
                ┌──────────────┐
                │   DimFecha   │
                │  fecha_id PK │
                │  dia         │
                │  mes         │
                │  trimestre   │
                │  anio        │
                └──────┬───────┘
                       │
┌──────────────┐ ┌─────▼──────────────┐ ┌──────────────────┐
│  DimCliente  ├─►    FactVentas      ◄─┤   DimProducto    │
│  cliente_id  │ │  venta_id    PK    │ │  producto_id  PK │
│  nombre      │ │  fecha_id    FK    │ │  nombre          │
│  ciudad      │ │  cliente_id  FK    │ │  marca           │
│  rango_edad  │ │  producto_id FK    │ │  precio          │
└──────────────┘ │  categoria_id FK   │ └──────────────────┘
                 │  cantidad          │
                 │  total_neto        │ ┌──────────────────┐
                 └────────────────────┘ │  DimCategoria    │
                                        │  categoria_id PK │
                                        │  nombre          │
                                        └──────────────────┘

┌──────────────────────┐
│   FactPredicciones   │  ← generada por el modelo de ML
│  mes                 │
│  nombre_mes          │
│  anio                │
│  total_predicho      │
│  tipo                │  "historico" o "prediccion"
└──────────────────────┘
```

---

## Modelo de Machine Learning

Se usa **regresión lineal** (scikit-learn) para predecir las ventas de los próximos 6 meses.

El modelo toma el historial de ventas mensuales de 2024, encuentra la línea de tendencia que mejor representa esos datos, y la extiende para estimar Enero-Junio 2025. Los resultados se guardan en `FactPredicciones` junto con los datos históricos, lo que permite mostrar ambas series en el mismo gráfico con colores distintos.

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| ETL | .NET 8 Console App |
| Machine Learning | Python 3.13 · scikit-learn · pandas |
| Base de datos | SQL Server |
| API | ASP.NET Core Web API |
| Frontend | React 18 · TypeScript |
| Gráficos | Recharts |
| HTTP Client | Axios |
| Fuente de datos | [DummyJSON](https://dummyjson.com/) |

---

## Estructura del repositorio

```
Ecommerce/
├── EcommerceETL/                  # Pipeline ETL (.NET)
│   ├── Models/
│   │   ├── ApiModels/             # Clases que mapean la respuesta de DummyJSON
│   │   └── DbModels/              # Clases que mapean las tablas de SQL Server
│   ├── Services/
│   │   ├── ExtractService.cs      # Llama a los endpoints de DummyJSON
│   │   ├── TransformService.cs    # Limpia, normaliza y calcula campos derivados
│   │   └── LoadService.cs         # Inserta los datos en SQL Server
│   ├── Helpers/
│   │   └── DateHelper.cs          # Genera fechas simuladas y rangos de edad
│   └── Program.cs                 # Orquesta el pipeline: Extract → Transform → Load
│
├── EcommerceReporting/            # API de Reporting (ASP.NET Core)
│   ├── Controllers/
│   │   └── AnalyticsController.cs # Define los 7 endpoints REST
│   ├── Models/
│   │   └── ReportModels.cs        # DTOs de respuesta (KpisDto, TopProductoDto, etc.)
│   ├── Services/
│   │   └── QueryService.cs        # Queries analíticas sobre el Data Warehouse
│   └── Program.cs                 # Configuración de la API y CORS
│
├── ecommerce-dashboard/           # Frontend (React + TypeScript)
│   └── src/
│       ├── components/            # Un componente por cada gráfico o tabla
│       ├── services/api.ts        # Centraliza todas las llamadas a la API
│       ├── types/index.ts         # Interfaces TypeScript de cada respuesta
│       └── App.tsx                # Arma el dashboard y orquesta el estado
│
└── prediccion_ventas.py           # Script de ML: entrena el modelo y guarda predicciones
```

---

## Cómo ejecutarlo localmente

### Requisitos
- .NET 8 SDK
- SQL Server
- Python 3.13+
- Node.js 18+

### 1. Base de datos

Creás una base de datos llamada `EcommerceAnalytics` en SQL Server y ejecutás el script de creación de tablas que está en la raíz del proyecto (`01_crear_esquema.sql`).

### 2. Variables de conexión

En los tres archivos siguientes tenes que reemplazar el servidor con el nombre de tu instancia de SQL Server:
- `EcommerceETL/Program.cs`
- `EcommerceReporting/Program.cs`
- `prediccion_ventas.py`

### 3. Cargar los datos (ETL)

```bash
cd EcommerceETL
dotnet run
```

Esto extrae los datos de DummyJSON, los transforma y los carga en SQL Server.

### 4. Generar predicciones (ML)

```bash
pip install pandas scikit-learn pyodbc
python prediccion_ventas.py
```

Esto entrena el modelo de regresión lineal y guarda las predicciones en la base de datos.

### 5. Levantar la API

```bash
cd EcommerceReporting
dotnet run
```

API disponible en `https://localhost:7269` · Swagger en `https://localhost:7269/swagger`

### 6. Levantar el dashboard

```bash
cd ecommerce-dashboard
npm install
npm start
```

Dashboard en `http://localhost:3000`

> ⚠️ La API tiene que estar corriendo antes de abrir el dashboard.

---

## Endpoints disponibles

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/analytics/kpis` | KPIs generales |
| GET | `/api/analytics/ventas/por-mes` | Ventas agrupadas por mes |
| GET | `/api/analytics/ventas/por-categoria` | Ventas por categoría |
| GET | `/api/analytics/ventas/por-trimestre` | Comparativa trimestral |
| GET | `/api/analytics/productos/top` | Top 10 productos más vendidos |
| GET | `/api/analytics/clientes/top` | Top 10 clientes por gasto |
| GET | `/api/analytics/predicciones` | Histórico 2024 + predicción 2025 |

---

## Conceptos aplicados

- **Esquema estrella** — diseño estándar de Data Warehousing con tabla de hechos y dimensiones
- **Pipeline ETL** — separación clara de responsabilidades: Extract, Transform, Load
- **Regresión lineal** — modelo de ML supervisado para predicción de series temporales
- **API RESTful** — endpoints semánticos con DTOs tipados
- **Separación de responsabilidades** — cada clase y servicio tiene una única función
- **Componentes reutilizables** — cada gráfico del dashboard es un componente independiente
