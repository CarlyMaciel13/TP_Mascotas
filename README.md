# 🐾 Query SQL para crear la tabla `Mascota` en SQL Server _TP:🐾 
<p align="center">
  <img src="https://mypu.es/wp-content/uploads/2019/07/perros-y-gato-2.png" alt="Imagen decorativa de mascota" width="300"/>
</p>

Esta consulta permite crear la estructura base para registrar mascotas en tu base de datos.  
Incluye campos como nombre, especie, edad, estado de vacunación y raza.

> 💡 **Sugerencia:** Ejecuta este script en SQL Server Management Studio (SSMS) conectado a tu servidor.

```sql
CREATE TABLE Mascota (
    id INT PRIMARY KEY IDENTITY(1,1),     -- Identificador único, autoincremental
    nombre VARCHAR(50),                   -- Nombre de la mascota
    especie VARCHAR(50),                  -- Tipo de animal (ej: Perro, Gato)
    edad INT,                             -- Edad en años
    vacunada BIT,                         -- 1 = Sí, 0 = No
    raza VARCHAR(50)                      -- Raza específica de la mascota
);
```
