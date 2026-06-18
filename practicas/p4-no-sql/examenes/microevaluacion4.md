# Microevaluación 4 — MongoDB + Mongo Express

**Materia:** Mantenimiento de Software II  
**Tema:** Bases de datos NoSQL con MongoDB  
**Estudiante:** Raúl Heredia  
**Rama de Git:** `rheredia/Microevaluacion-4`  

---

## Instrucciones de Despliegue de la Práctica

1. **Clonar el repositorio:** `git clone https://github.com/QuelaliUcatec/mantenimiento2.git`
2. **Crear una rama nueva con tu nombre:** `git checkout -b rheredia/Microevaluacion-4`
3. **Ubicarse en la carpeta del proyecto:** `cd practicas/p4-no-sql`
4. **Levantar contenedores Docker (MongoDB + Mongo Express):** `docker compose up -d`
5. **Conectarse a MongoDB Shell interactivo:**
   ```bash
   docker exec -it mongo-server mongosh -u root -p mongo_secret_pass
   ```
6. **Ejecutar comandos:** Todos los comandos se ejecutan dentro del prompt de `mongosh`.
7. **Verificar visualmente:** Tras cada consulta, verificar el impacto en Mongo Express ingresando a `http://localhost:8081` (Usuario: `admin`, Contraseña: `admin`).

---

## Parte A — CRUD en mongosh + verificación en Mongo Express (4 pts)

### Inserción inicial de los documentos
Creación de la base de datos `microeval`, la colección `empleados` e inserción en lote de los registros base.

* **Comandos ejecutados en mongosh:**
  ```javascript
  use microeval

  db.empleados.insertMany([
    { nombre: "Lucía",   puesto: "Desarrolladora", salario: 1200, departamento: "IT",       activo: true },
    { nombre: "Carlos",  puesto: "Analista",        salario: 900,  departamento: "Finanzas", activo: true },
    { nombre: "María",   puesto: "Desarrolladora",  salario: 1500, departamento: "IT",       activo: false },
    { nombre: "Pedro",   puesto: "Tester",          salario: 800,  departamento: "IT",       activo: true }
  ])
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  {
    "acknowledged" : true,
    "insertedIds" : {
      "0" : ObjectId("6a332b75bef729c6cd9df8a3"),
      "1" : ObjectId("6a332b75bef729c6cd9df8a4"),
      "2" : ObjectId("6a332b75bef729c6cd9df8a5"),
      "3" : ObjectId("6a332b75bef729c6cd9df8a6")
    }
  }
  ```

* **Verificación en Mongo Express:**
  Accediendo a `http://localhost:8081/db/microeval/empleados` se listan correctamente los 4 documentos dentro de la colección.

---

### **1. Listar todos los empleados activos y verificar el resultado en Mongo Express.**

* **Comando ejecutado en mongosh:**
  ```javascript
  db.empleados.find({ activo: true })
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  [
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a3"),
      "nombre": "Lucía",
      "puesto": "Desarrolladora",
      "salario": 1200,
      "departamento": "IT",
      "activo": true
    },
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a4"),
      "nombre": "Carlos",
      "puesto": "Analista",
      "salario": 900,
      "departamento": "Finanzas",
      "activo": true
    },
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a6"),
      "nombre": "Pedro",
      "puesto": "Tester",
      "salario": 800,
      "departamento": "IT",
      "activo": true
    }
  ]
  ```

* **Verificación en Mongo Express:**
  En la interfaz gráfica, ingresando `{ activo: true }` en el panel de búsqueda superior y haciendo clic en **Find**, el sistema filtra y expone exactamente a Lucía, Carlos y Pedro.

---

### **2. Mostrar solo `nombre` y `salario` de los empleados de IT, sin mostrar `_id`, y verificar en Mongo Express.**

* **Comando ejecutado en mongosh (Proyección):**
  ```javascript
  db.empleados.find({ departamento: "IT" }, { nombre: 1, salario: 1, _id: 0 })
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  [
    { "nombre": "Lucía", "salario": 1200 },
    { "nombre": "María", "salario": 1500 },
    { "nombre": "Pedro", "salario": 800 }
  ]
  ```

* **Verificación en Mongo Express:**
  Se coloca `{ departamento: "IT" }` en la casilla **Query** y `{ nombre: 1, salario: 1, _id: 0 }` en la casilla **Fields** (Proyección). Al presionar **Find**, se oculta el ID y solo se listan nombres y salarios.

---

### **3. Incrementar el salario de Lucía en 200 y verificar el cambio en Mongo Express.**

* **Comando ejecutado en mongosh (Modificación con $inc):**
  ```javascript
  db.empleados.updateOne({ nombre: "Lucía" }, { $inc: { salario: 200 } })
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  {
    "acknowledged": true,
    "insertedId": null,
    "matchedCount": 1,
    "modifiedCount": 1,
    "upsertedCount": 0
  }
  ```

* **Verificación en Mongo Express:**
  Al recargar la vista general de la colección, se observa que el salario del documento de Lucía ha cambiado satisfactoriamente de `1200` a `1400`.

---

### **4. Contar cuántos empleados hay por departamento usando `aggregate` y verificar en Mongo Express.**

* **Comando ejecutado en mongosh (Agrupación):**
  ```javascript
  db.empleados.aggregate([
    { $group: { _id: "$departamento", totalEmpleados: { $sum: 1 } } }
  ])
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  [
    { "_id": "IT", "totalEmpleados": 3 },
    { "_id": "Finanzas", "totalEmpleados": 1 }
  ]
  ```

* **Verificación en Mongo Express:**
  Confirmación del total en la lista visual: se contabilizan manualmente 3 empleados en el departamento de "IT" (Lucía, María, Pedro) y 1 empleado en "Finanzas" (Carlos).

---

### **5. Eliminar todos los empleados inactivos (`activo: false`) y verificar en Mongo Express que ya no aparecen.**

* **Comando ejecutado en mongosh (Eliminación):**
  ```javascript
  db.empleados.deleteMany({ activo: false })
  ```

* **Resultado de ejecución en mongosh:**
  ```json
  {
    "acknowledged": true,
    "deletedCount": 1
  }
  ```

* **Verificación en Mongo Express:**
  Al refrescar el listado general en el puerto `8081`, el documento de María (`activo: false`) ha desaparecido completamente, quedando únicamente 3 documentos.

---

## Parte B — Agregar desde Mongo Express y verificar en mongosh (0,5 pts)

### **6. Desde la interfaz de Mongo Express, agregá un nuevo empleado con los campos que quieras.**

* **Acción en la interfaz web:**
  1. Hacer clic en **New Document** en la esquina superior derecha del panel de la colección `empleados`.
  2. Introducir el siguiente documento JSON:
     ```json
     {
       "nombre": "Fernando",
       "puesto": "QA",
       "salario": 1100,
       "departamento": "IT",
       "activo": true
     }
     ```
  3. Presionar **Save** para guardarlo.

---

### **7. Volvé a `mongosh` y ejecutá `db.empleados.find()` para confirmar que el nuevo empleado aparece.**

* **Comando ejecutado en mongosh:**
  ```javascript
  db.empleados.find()
  ```

* **Resultado de ejecución en mongosh (Estado Final de la Base de Datos):**
  ```json
  [
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a3"),
      "nombre": "Lucía",
      "puesto": "Desarrolladora",
      "salario": 1400,
      "departamento": "IT",
      "activo": true
    },
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a4"),
      "nombre": "Carlos",
      "puesto": "Analista",
      "salario": 900,
      "departamento": "Finanzas",
      "activo": true
    },
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a6"),
      "nombre": "Pedro",
      "puesto": "Tester",
      "salario": 800,
      "departamento": "IT",
      "activo": true
    },
    {
      "_id": ObjectId("6a332b75bef729c6cd9df8a7"),
      "nombre": "Fernando",
      "puesto": "QA",
      "salario": 1100,
      "departamento": "IT",
      "activo": true
    }
  ]
  ```

---

## Criterios de evaluación

| Ítem | Puntos |
|:-----|:-------|
| Inserción inicial + verificación en Express | 0,7 |
| 1 — Listar activos + verificar en Express | 0,7 |
| 2 — Proyección con filtro + verificar en Express | 0,7 |
| 3 — Incrementar salario (`$inc`) + verificar en Express | 0,7 |
| 4 — Aggregation (`$group`) + verificar en Express | 0,7 |
| 5 — Eliminar (`deleteMany`) + verificar en Express | 0,7 |
| 6 — Agregar desde Mongo Express | 0,25 |
| 7 — Verificar desde mongosh | 0,25 |
| Presentación y orden | 0,5 |
| **Total** | **5** |
