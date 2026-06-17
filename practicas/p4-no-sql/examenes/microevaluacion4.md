# Microevaluación 4 — MongoDB + Mongo Express

**Materia:** Mantenimiento de Software II  
**Tema:** Bases de datos NoSQL con MongoDB  
**Duración estimada:** 25 minutos  
**Total:** 5 puntos  

---

## Instrucciones

1. Clonar el repositorio: `git clone https://github.com/QuelaliUcatec/mantenimiento2.git`
2. Crear una rama nueva con tu nombre: `git checkout -b gquelali/Microevalucion-4`
3. Ubicarse en la carpeta del proyecto: `cd practicas/p4-no-sql`
4. Asegurate de tener los contenedores corriendo: `docker compose up -d`
5. Conectate a MongoDB con: `docker exec -it mongo-server mongosh -u root -p mongo_secret_pass` (mongosh = MongoDB Shell interactivo, viene instalado en la imagen)
6. Todos los comandos se ejecutan dentro de `mongosh`
7. Después de cada comando, verificá el resultado en Mongo Express (`http://localhost:8081`, usuario: `admin`, contraseña: `admin`)
8. Mostrá cada resultado al docente

---

## Parte A — CRUD en mongosh + verificación en Mongo Express (4 pts)

Creá la base de datos `microeval` y la colección `empleados`. Insertá los siguientes documentos:

```javascript
db.empleados.insertMany([
  { nombre: "Lucía",   puesto: "Desarrolladora", salario: 1200, departamento: "IT",       activo: true },
  { nombre: "Carlos",  puesto: "Analista",        salario: 900,  departamento: "Finanzas", activo: true },
  { nombre: "María",   puesto: "Desarrolladora",  salario: 1500, departamento: "IT",       activo: false },
  { nombre: "Pedro",   puesto: "Tester",          salario: 800,  departamento: "IT",       activo: true }
])
```

Después de insertar, **verificar en Mongo Express** que los 4 documentos aparecen en la colección `empleados` de la base `microeval`. (0,7 pts — 0,4 comando + 0,3 verificación)

---

**1.** Listar todos los empleados activos y verificar el resultado en Mongo Express. (0,7 pts — 0,4 comando + 0,3 verificación)

```javascript

```

---

**2.** Mostrar solo `nombre` y `salario` de los empleados de IT, sin mostrar `_id`, y verificar en Mongo Express. (0,7 pts — 0,4 comando + 0,3 verificación)

```javascript

```

---

**3.** Incrementar el salario de **Lucía** en 200 y verificar el cambio en Mongo Express. (0,7 pts — 0,4 comando + 0,3 verificación)

```javascript

```

---

**4.** Contar cuántos empleados hay por departamento usando `aggregate` y verificar en Mongo Express. (0,7 pts — 0,4 comando + 0,3 verificación)

```javascript

```

---

**5.** Eliminar todos los empleados inactivos (`activo: false`) y verificar en Mongo Express que ya no aparecen. (0,7 pts — 0,4 comando + 0,3 verificación)

```javascript

```

---

## Parte B — Agregar desde Mongo Express y verificar en mongosh (0,5 pts)

**6.** Desde la interfaz de Mongo Express, **agregá un nuevo empleado** con los campos que quieras. (0,25 pts)

**7.** Volvé a `mongosh` y ejecutá `db.empleados.find()` para confirmar que el nuevo empleado aparece. (0,25 pts)

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

---


