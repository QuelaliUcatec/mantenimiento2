# 3. Ejemplo básico guiado

> [← Volver al README](../README.md)

Primeros pasos con MongoDB: crear una base de datos, insertar documentos, consultar, actualizar, eliminar y ver los cambios en Mongo Express.

---

## Requisitos previos

Antes de empezar, asegurate de tener los contenedores corriendo:

```bash
cd practicas/p4-no-sql
docker compose up -d
docker compose ps   # ambos deben estar "Up"
```

---

## 1. Conectarse a MongoDB

```bash
docker exec -it mongo-server mongosh -u root -p mongo_secret_pass
```

Vas a ver el prompt `test>`. Ahí vas a escribir todos los comandos de este ejemplo.

---

## 2. Crear una base de datos y una colección

En MongoDB no necesitás crear explícitamente una base de datos o colección. Se crean automáticamente al insertar el primer documento.

```javascript
use tienda
```

Cambia a la base de datos `tienda`. Si no existe, la creará al insertar el primer documento.

Verificá que estás en la base correcta:

```javascript
db
```

Salida: `tienda`

---

## 3. Insertar documentos (Create)

```javascript
db.productos.insertOne({
  nombre: "Teclado mecánico RGB",
  precio: 89.99,
  categoria: "periféricos",
  stock: 150,
  disponible: true,
  tags: ["teclado", "mecánico", "rgb"]
})
```

MongoDB devuelve:

```javascript
{
  acknowledged: true,
  insertedId: ObjectId("...")
}
```

Ahora insertá varios a la vez:

```javascript
db.productos.insertMany([
  {
    nombre: "Mouse inalámbrico",
    precio: 49.99,
    categoria: "periféricos",
    stock: 200,
    disponible: true,
    tags: ["mouse", "inalámbrico"]
  },
  {
    nombre: "Monitor 27\" 4K",
    precio: 399.99,
    categoria: "monitores",
    stock: 30,
    disponible: true,
    tags: ["monitor", "4k", "27pulgadas"]
  },
  {
    nombre: "Mousepad XXL",
    precio: 29.99,
    categoria: "accesorios",
    stock: 80,
    disponible: true,
    tags: ["mousepad", "escritorio"]
  }
])
```

---

## 4. Consultar documentos (Read)

### Todos los documentos

```javascript
db.productos.find()
```

### Con formato legible

```javascript
db.productos.find().pretty()
```

### Filtrar por categoría

```javascript
db.productos.find({ categoria: "periféricos" }).pretty()
```

### Filtrar por precio (operadores de comparación)

```javascript
// Productos con precio mayor o igual a 50
db.productos.find({ precio: { $gte: 50 } }).pretty()

// Productos con precio entre 30 y 100
db.productos.find({ precio: { $gte: 30, $lte: 100 } }).pretty()
```

### Filtrar por array (tags)

```javascript
// Productos que tengan el tag "4k"
db.productos.find({ tags: "4k" }).pretty()

// Productos que tengan TODOS esos tags
db.productos.find({ tags: { $all: ["periféricos"] } }).pretty()
```

### Proyección (elegir qué campos mostrar)

```javascript
// Solo nombre y precio, sin _id
db.productos.find(
  { categoria: "periféricos" },
  { nombre: 1, precio: 1, _id: 0 }
)
```

### Contar documentos

```javascript
db.productos.countDocuments()
db.productos.countDocuments({ categoria: "periféricos" })
```

---

## 5. Actualizar documentos (Update)

### Actualizar un campo específico

```javascript
db.productos.updateOne(
  { nombre: "Teclado mecánico RGB" },
  { $set: { precio: 79.99 } }
)
```

### Incrementar un valor numérico

```javascript
db.productos.updateOne(
  { nombre: "Monitor 27\" 4K" },
  { $inc: { stock: -5 } }
)
```

### Agregar un elemento a un array

```javascript
db.productos.updateOne(
  { nombre: "Teclado mecánico RGB" },
  { $push: { tags: "oferta" } }
)
```

### Actualizar varios documentos a la vez

```javascript
db.productos.updateMany(
  { categoria: "periféricos" },
  { $set: { disponible: true } }
)
```

---

## 6. Eliminar documentos (Delete)

### Eliminar un documento específico

```javascript
db.productos.deleteOne({ nombre: "Mousepad XXL" })
```

### Eliminar varios documentos

```javascript
db.productos.deleteMany({ disponible: false })
```

### Eliminar todos los documentos de una colección

```javascript
db.productos.deleteMany({})
```

> ⚠️ Esto vacía la colección, no la elimina. Los documentos se borran pero la colección sigue existiendo.

---

## 7. Verificar en Mongo Express

1. Abrí [http://localhost:8081](http://localhost:8081)
2. Iniciá sesión con `admin` / `admin`
3. Hacé clic en la base de datos **tienda**
4. Hacé clic en la colección **productos**
5. Vas a ver todos los documentos que insertaste
6. Desde la interfaz podés:
   - Ver documentos en formato tabla o JSON
   - Editar campos directamente
   - Agregar documentos nuevos
   - Eliminar documentos

---

## 8. Script completo del ejemplo

Guardá este script como `ejemplo_basico.js` para ejecutarlo todo de una:

```javascript
// ejemplo_basico.js
// Conectarse con: mongosh -u root -p mongo_secret_pass ejemplo_basico.js

use("tienda")

// Insertar
db.productos.insertMany([
  { nombre: "Teclado mecánico RGB", precio: 89.99, categoria: "periféricos", stock: 150, tags: ["teclado", "mecánico"] },
  { nombre: "Mouse inalámbrico",    precio: 49.99, categoria: "periféricos", stock: 200, tags: ["mouse", "inalámbrico"] },
  { nombre: "Monitor 27\" 4K",      precio: 399.99, categoria: "monitores", stock: 30, tags: ["monitor", "4k"] }
])

// Leer
console.log("=== Todos los productos ===")
printjson(db.productos.find().toArray())

console.log("=== Periféricos ===")
printjson(db.productos.find({ categoria: "periféricos" }).toArray())

console.log("=== Stock total ===")
printjson(db.productos.aggregate([
  { $group: { _id: null, total: { $sum: "$stock" } } }
]).toArray())

// Actualizar
db.productos.updateOne({ nombre: "Teclado mecánico RGB" }, { $inc: { stock: -10 } })

// Verificar actualización
console.log("=== Después de actualizar stock ===")
printjson(db.productos.find({ nombre: "Teclado mecánico RGB" }).toArray())
```

Ejecutarlo:

```bash
# Copiar el script al contenedor
docker cp ejemplo_basico.js mongo-server:/tmp/

# Ejecutarlo
docker exec -it mongo-server mongosh -u root -p mongo_secret_pass /tmp/ejemplo_basico.js
```

---

## Resumen de comandos usados

| Operación | Comando MongoDB |
|:----------|:----------------|
| Crear DB | `use nombre_db` |
| Insertar uno | `db.coleccion.insertOne({...})` |
| Insertar varios | `db.coleccion.insertMany([...])` |
| Leer todos | `db.coleccion.find()` |
| Filtrar | `db.coleccion.find({ campo: valor })` |
| Proyectar | `db.coleccion.find({}, { campo: 1 })` |
| Actualizar uno | `db.coleccion.updateOne({ filtro }, { $set: {...} })` |
| Actualizar varios | `db.coleccion.updateMany({ filtro }, { $set: {...} })` |
| Eliminar uno | `db.coleccion.deleteOne({ filtro })` |
| Eliminar varios | `db.coleccion.deleteMany({ filtro })` |
| Contar | `db.coleccion.countDocuments()` |

---

> **Siguiente:** [Ejercicios básicos →](04-ejercicios.md)
