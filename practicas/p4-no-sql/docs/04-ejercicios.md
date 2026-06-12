# 4. Ejercicios básicos

> [← Volver al README](../README.md)

Ejercicios prácticos para afianzar conceptos de NoSQL con MongoDB y explorar otros DBMS (Redis, Neo4j y almacenamiento cloud).

---

## 4.1 Ejercicios con MongoDB

> Los ejercicios asumen que los contenedores están corriendo (`docker compose up -d`).

### Nivel 1: CRUD básico

**Ejercicio 1.1 — Biblioteca personal**

Crear una base de datos `biblioteca` con una colección `libros` e insertar:

```javascript
use biblioteca
db.libros.insertMany([
  { titulo: "Cien años de soledad", autor: "Gabriel García Márquez", año: 1967, genero: "Realismo mágico", paginas: 432, disponible: true },
  { titulo: "1984", autor: "George Orwell", año: 1949, genero: "Distopía", paginas: 328, disponible: true },
  { titulo: "El principito", autor: "Antoine de Saint-Exupéry", año: 1943, genero: "Infantil", paginas: 96, disponible: true },
  { titulo: "Dune", autor: "Frank Herbert", año: 1965, genero: "Ciencia ficción", paginas: 688, disponible: false }
])
```

**Consultas a realizar:**

1. Todos los libros disponibles
2. Libros del género "Distopía"
3. Libros con más de 300 páginas
4. Libros publicados después de 1950 y con menos de 500 páginas
5. Contar cuántos libros hay por género (usar aggregation)
6. Actualizar "Dune" a disponible: true
7. Incrementar las páginas de "El principito" en 2 (tiene 96 pero deberían ser 98 😉)
8. Eliminar todos los libros no disponibles

<details>
<summary>Ver soluciones</summary>

```javascript
// 1
db.libros.find({ disponible: true }).pretty()

// 2
db.libros.find({ genero: "Distopía" }).pretty()

// 3
db.libros.find({ paginas: { $gt: 300 } }).pretty()

// 4
db.libros.find({ año: { $gt: 1950 }, paginas: { $lt: 500 } }).pretty()

// 5
db.libros.aggregate([
  { $group: { _id: "$genero", cantidad: { $sum: 1 } } }
])

// 6
db.libros.updateOne({ titulo: "Dune" }, { $set: { disponible: true } })

// 7
db.libros.updateOne({ titulo: "El principito" }, { $set: { paginas: 98 } })

// 8
db.libros.deleteMany({ disponible: false })
```
</details>

### Nivel 2: Aggregation Pipeline

**Ejercicio 2.1 — Ventas**

Insertar en la base `tienda`:

```javascript
db.ventas.insertMany([
  { producto: "Teclado", cantidad: 2, precio: 89.99, fecha: ISODate("2026-01-15"), vendedor: "Ana" },
  { producto: "Mouse", cantidad: 5, precio: 49.99, fecha: ISODate("2026-01-15"), vendedor: "Luis" },
  { producto: "Monitor", cantidad: 1, precio: 399.99, fecha: ISODate("2026-01-16"), vendedor: "Ana" },
  { producto: "Teclado", cantidad: 3, precio: 89.99, fecha: ISODate("2026-01-16"), vendedor: "Carlos" },
  { producto: "Mousepad", cantidad: 10, precio: 29.99, fecha: ISODate("2026-01-17"), vendedor: "Luis" }
])
```

**Consultas con aggregation:**

1. Ingreso total por producto (cantidad × precio)
2. Total vendido por cada vendedor
3. Producto más vendido en cantidad
4. Ingreso total por día

<details>
<summary>Ver soluciones</summary>

```javascript
// 1
db.ventas.aggregate([
  { $group: { _id: "$producto", ingresoTotal: { $sum: { $multiply: ["$cantidad", "$precio"] } } } }
])

// 2
db.ventas.aggregate([
  { $group: { _id: "$vendedor", total: { $sum: { $multiply: ["$cantidad", "$precio"] } } } }
])

// 3
db.ventas.aggregate([
  { $group: { _id: "$producto", totalVendido: { $sum: "$cantidad" } } },
  { $sort: { totalVendido: -1 } },
  { $limit: 1 }
])

// 4
db.ventas.aggregate([
  { $group: { _id: { $dateToString: { format: "%Y-%m-%d", date: "$fecha" } }, total: { $sum: { $multiply: ["$cantidad", "$precio"] } } } }
])
```
</details>

### Nivel 3: Índices y rendimiento

**Ejercicio 3.1**

1. Crear un índice ascendente sobre `email` en la colección `usuarios` (creala con algunos documentos primero)
2. Crear un índice compuesto sobre `categoria` + `precio` en `productos`
3. Crear un índice único sobre `email` para evitar duplicados
4. Crear un índice TTL sobre un campo `createdAt` que expire a los 7 días
5. Explicar con `explain()` cómo MongoDB usa un índice en una consulta

---

## 4.2 Introducción a Redis

[Redis](https://redis.io) es una base de datos **clave-valor** en memoria, conocida por su velocidad extrema.

### Agregar Redis al proyecto (opcional)

Agregá este servicio al `docker-compose.yml`:

```yaml
redis:
  image: redis:7-alpine
  container_name: redis-server
  restart: unless-stopped
  ports:
    - "6379:6379"
  volumes:
    - redis_data:/data
  networks:
    - app_network

# Y el volumen:
volumes:
  redis_data:
```

### Comandos básicos de Redis

```bash
# Conectarse
docker exec -it redis-server redis-cli
```

```redis
# Operaciones básicas
SET usuario:1 "Gastón"
GET usuario:1

SET contador 0
INCR contador
INCR contador
GET contador              # → 2

# Listas
LPUSH tareas "comprar pan"
LPUSH tareas "estudiar NoSQL"
LRANGE tareas 0 -1

# Sets
SADD lenguajes "python" "javascript" "go"
SMEMBERS lenguajes

# TTL
SET sesion:abc "token123"
EXPIRE sesion:abc 3600     # expira en 1 hora
TTL sesion:abc
```

### Ejercicios propuestos Redis

1. Implementar un contador de visitas que se incremente automáticamente
2. Crear una lista de tareas pendientes y marcarlas como completadas
3. Usar TTL para simular una sesión que expira después de 5 segundos
4. Investigar: ¿qué diferencia hay entre Redis y MongoDB en términos de persistencia?

---

## 4.3 Introducción a Neo4j

[Neo4j](https://neo4j.com) es una base de datos **orientada a grafos**.

### Agregar Neo4j al proyecto (opcional)

```yaml
neo4j:
  image: neo4j:5-enterprise
  container_name: neo4j-server
  restart: unless-stopped
  ports:
    - "7474:7474"   # HTTP (navegador)
    - "7687:7687"   # Bolt (drivers)
  environment:
    NEO4J_AUTH: neo4j/neo4j_secret
  volumes:
    - neo4j_data:/data
  networks:
    - app_network

volumes:
  neo4j_data:
```

### Conceptos clave de grafos

```
(nodo:Persona {nombre: "Gastón"})
  -[r:AMIGO_DE {desde: 2020}]->
(nodo:Persona {nombre: "María"})
```

### Consultas básicas en Cypher (lenguaje de Neo4j)

```cypher
// Crear nodos
CREATE (g:Persona {nombre: "Gastón", edad: 30})
CREATE (m:Persona {nombre: "María", edad: 28})

// Crear relación
MATCH (g:Persona {nombre: "Gastón"})
MATCH (m:Persona {nombre: "María"})
CREATE (g)-[:AMIGO_DE {desde: 2020}]->(m)

// Consultar
MATCH (g:Persona)-[:AMIGO_DE]->(amigos)
RETURN g.nombre, amigos.nombre

// Encontrar amigos de amigos
MATCH (g:Persona {nombre: "Gastón"})-[:AMIGO_DE]->()-[:AMIGO_DE]->(amigoDeAmigo)
RETURN amigoDeAmigo.nombre
```

### Ejercicios propuestos Neo4j

1. Modelar una red social de 4 personas con relaciones de amistad
2. Encontrar el camino más corto entre dos personas
3. Investigar: ¿qué problema resuelven mejor los grafos que los documentos o SQL?

---

## 4.4 Almacenamiento cloud: MongoDB Atlas

[MongoDB Atlas](https://www.mongodb.com/atlas) es el servicio cloud administrado de MongoDB. Ofrece:

- **Free tier**: 512 MB de almacenamiento, compartido, sin costo
- **Serverless**: escala a cero cuando no se usa
- **Multi-cloud**: AWS, Azure y GCP
- **Características adicionales**: Atlas Search, Vector Search, Data Lake, Charts

### Cómo empezar con Atlas (gratis)

1. Crear cuenta en [mongodb.com/atlas](https://www.mongodb.com/atlas)
2. Crear un cluster free (AWS, Azure o GCP)
3. Configurar IP Whitelist (permitir tu IP)
4. Crear usuario de base de datos
5. Obtener la connection string:

```
mongodb+srv://<usuario>:<contraseña>@cluster0.xxxxx.mongodb.net/
```

### Ejercicios propuestos cloud

1. Crear un cluster gratuito en MongoDB Atlas
2. Conectarte desde mongosh local usando la connection string
3. Insertar documentos desde tu máquina local a la nube
4. Explorar Atlas Search y probar búsquedas de texto
5. Investigar: diferencias de latencia entre MongoDB local y Atlas

---

## 4.5 Tabla comparativa de lo aprendido

| Aspecto | MongoDB | Redis | Neo4j |
|:--------|:--------|:------|:------|
| **Tipo** | Documental | Clave-Valor | Grafos |
| **Modelo** | JSON/BSON | Pares key-value | Nodos + relaciones |
| **Puerto** | 27017 | 6379 | 7474 (web), 7687 (bolt) |
| **Lenguaje** | MongoDB Query Language | Redis CLI / comandos | Cypher |
| **Persistencia** | Disco (WiredTiger) | Memoria + snapshots opcionales | Disco |
| **Mejor para** | Datos semiestructurados | Caching, sesiones, colas | Datos conectados |
| **Web UI** | Mongo Express | RedisInsight | Neo4j Browser |

---

> **Siguiente:** [Integración SQL + NoSQL →](05-integracion.md)
