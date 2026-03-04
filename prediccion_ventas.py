import pandas as pd
from sklearn.linear_model import LinearRegression
import numpy as np
import pyodbc

CONNECTION_STRING = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=DESKTOP-G88OCVU;"
    "DATABASE=EcommerceAnalytics;"
    "Trusted_Connection=yes;"
)

MESES = {
    1: "Enero", 2:"Febrero", 3:"Marzo", 4:"Abril",
    5: "Mayo", 6:"Junio", 7:"Julio", 8:"Agosto",
    9:"Septiembre", 10:"Octubre", 11:"Noviembre", 12:"Diciembre"
}

print("Leyendo datos históricos de SQL Server...")

conn = pyodbc.connect(CONNECTION_STRING)

query = """
    SELECT
        f.mes,
        f.nombre_mes,
        f.anio,
        SUM(v.total_neto) AS total_vendido
    FROM FactVentas v
    JOIN DimFecha f ON v.fecha_id = f.fecha_id
    GROUP BY f.mes, f.nombre_mes, f.anio
    ORDER BY f.mes
"""

df = pd.read_sql(query, conn)

print(f"{len(df)} meses de datos históricos leídos")
print(df.to_string(index=False))
print()

# X = variable de entrada (el número de mes: 1, 2, 3... 12)
# Y = variable de salida (cuánto se vendió ese mes)

X = df["mes"].values.reshape(-1, 1)
Y = df["total_vendido"].values

print("Entrenando modelo de regresión lineal")

modelo = LinearRegression()
modelo.fit(X, Y)

# El modelo encuentra una línea: Y = pendiente * X + intercepto

pendiente = modelo.coef_[0]
tendencia = "creciente" if pendiente > 0 else "decreciente"
print(f"Modelo entrenado - Tendencia {tendencia} (pendiente: {pendiente:.2f})")
print()

print("Generando predicciones para los proximos 6 meses...")

meses_futuros = np.array([13, 14, 15, 16, 17, 18]).reshape(-1, 1)
predicciones = modelo.predict(meses_futuros)

for i, (mes_futuro, pred) in enumerate(zip(meses_futuros, predicciones)):
    mes_real = (mes_futuro[0] -1) % 12 + 1
    print(f"{MESES[mes_real]} 2025: ${pred:.2f}")

print()

print("Guardando resultados en FactPredicciones... ")

cursor = conn.cursor()

cursor.execute("DELETE FROM FactPredicciones")

for _, fila in df.iterrows():
    cursor.execute("""
        INSERT INTO FactPredicciones (mes, nombre_mes, anio, total_predicho, tipo)
        VALUES (?, ?, ?, ?, ?)
    """, int(fila["mes"]), fila["nombre_mes"], int(fila["anio"]),
        float(fila["total_vendido"]), "historico")
    
print(f" {len(df)} meses históricos guardados")

for i, (mes_futuro, pred) in enumerate(zip(meses_futuros, predicciones)):
    mes_real = int((mes_futuro[0] - 1) % 12 + 1)
    cursor.execute("""
        INSERT INTO FactPredicciones (mes, nombre_mes, anio, total_predicho, tipo)
        VALUES (?, ?, ?, ?, ?)
    """, mes_real, MESES[mes_real], 2025, float(pred), "prediccion")

print(f"6 meses de prediccion guardados")

conn.commit()
cursor.close()
conn.close()

print()
print("PREDICCIÓN COMPLETADA")
print("Podés verificar los datos en SQL Server")
