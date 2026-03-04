-- ============================================================
-- PROYECTO: Ecommerce Analytics
-- Creación del esquema estrella en SQL Server
-- ============================================================
-- Paso 1: Crear la base de datos (ejecutá esto por separado
-- antes de correr el resto del script)
--
-- CREATE DATABASE EcommerceAnalytics;
-- GO
-- USE EcommerceAnalytics;
-- GO
-- ============================================================

-- ============================================================
-- DimFecha
-- Una fila por cada día del año 2024.
-- La genera el pipeline ETL, no viene de la API.
-- ============================================================
CREATE TABLE DimFecha (
    fecha_id            INT          NOT NULL,
    fecha               DATE         NOT NULL,
    dia                 TINYINT      NOT NULL,
    mes                 TINYINT      NOT NULL,
    nombre_mes          VARCHAR(20)  NOT NULL,
    trimestre           TINYINT      NOT NULL,
    anio                SMALLINT     NOT NULL,
    dia_semana          VARCHAR(20)  NOT NULL,
    es_fin_de_semana    BIT          NOT NULL,

    CONSTRAINT PK_DimFecha PRIMARY KEY (fecha_id)
);

-- ============================================================
-- DimCategoria
-- Una fila por cada categoría de producto.
-- ============================================================
CREATE TABLE DimCategoria (
    categoria_id    INT             IDENTITY(1,1) NOT NULL,
    nombre          VARCHAR(100)    NOT NULL,

    CONSTRAINT PK_DimCategoria PRIMARY KEY (categoria_id)
);

-- ============================================================
-- DimProducto
-- Una fila por cada producto del catálogo.
-- Viene del endpoint /products de DummyJSON.
-- ============================================================
CREATE TABLE DimProducto (
    producto_id         INT             NOT NULL,
    nombre              VARCHAR(200)    NOT NULL,
    marca               VARCHAR(100)    NULL,
    categoria_id        INT             NOT NULL,
    precio_unitario     DECIMAL(10,2)   NOT NULL,
    stock               INT             NOT NULL,
    rating              DECIMAL(3,2)    NULL,

    CONSTRAINT PK_DimProducto PRIMARY KEY (producto_id),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (categoria_id)
        REFERENCES DimCategoria (categoria_id)
);

-- ============================================================
-- DimCliente
-- Una fila por cada usuario.
-- Viene del endpoint /users de DummyJSON.
-- ============================================================
CREATE TABLE DimCliente (
    cliente_id      INT             NOT NULL,
    nombre_completo VARCHAR(200)    NOT NULL,
    email           VARCHAR(200)    NULL,
    edad            TINYINT         NULL,
    rango_edad      VARCHAR(20)     NULL,
    ciudad          VARCHAR(100)    NULL,
    estado          VARCHAR(100)    NULL,

    CONSTRAINT PK_DimCliente PRIMARY KEY (cliente_id)
);

-- ============================================================
-- FactVentas
-- Tabla central del esquema estrella.
-- Una fila por cada producto dentro de cada carrito.
-- ============================================================
CREATE TABLE FactVentas (
    venta_id            INT             IDENTITY(1,1) NOT NULL,
    carrito_id          INT             NOT NULL,
    fecha_id            INT             NOT NULL,
    cliente_id          INT             NOT NULL,
    producto_id         INT             NOT NULL,
    categoria_id        INT             NOT NULL,
    cantidad            INT             NOT NULL,
    precio_unitario     DECIMAL(10,2)   NOT NULL,
    descuento_pct       DECIMAL(5,2)    NULL,
    monto_descuento     DECIMAL(10,2)   NULL,
    total_bruto         DECIMAL(10,2)   NOT NULL,
    total_neto          DECIMAL(10,2)   NOT NULL,

    CONSTRAINT PK_FactVentas        PRIMARY KEY (venta_id),
    CONSTRAINT FK_Ventas_Fecha      FOREIGN KEY (fecha_id)      REFERENCES DimFecha     (fecha_id),
    CONSTRAINT FK_Ventas_Cliente    FOREIGN KEY (cliente_id)    REFERENCES DimCliente   (cliente_id),
    CONSTRAINT FK_Ventas_Producto   FOREIGN KEY (producto_id)   REFERENCES DimProducto  (producto_id),
    CONSTRAINT FK_Ventas_Categoria  FOREIGN KEY (categoria_id)  REFERENCES DimCategoria (categoria_id)
);

CREATE INDEX IX_FactVentas_Fecha     ON FactVentas (fecha_id);
CREATE INDEX IX_FactVentas_Cliente   ON FactVentas (cliente_id);
CREATE INDEX IX_FactVentas_Producto  ON FactVentas (producto_id);
CREATE INDEX IX_FactVentas_Categoria ON FactVentas (categoria_id);

-- ============================================================
-- FactPredicciones
-- Generada por el modelo de Machine Learning (prediccion_ventas.py).
-- Contiene tanto datos históricos como predicciones futuras.
-- ============================================================
CREATE TABLE FactPredicciones (
    prediccion_id   INT             IDENTITY(1,1) NOT NULL,
    mes             TINYINT         NOT NULL,
    nombre_mes      VARCHAR(20)     NOT NULL,
    anio            SMALLINT        NOT NULL,
    total_predicho  DECIMAL(10,2)   NOT NULL,
    tipo            VARCHAR(20)     NOT NULL,   -- 'historico' o 'prediccion'

    CONSTRAINT PK_FactPredicciones PRIMARY KEY (prediccion_id)
);
