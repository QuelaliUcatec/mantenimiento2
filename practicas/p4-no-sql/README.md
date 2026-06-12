# p4-no-sql — Bases de Datos NoSQL con MongoDB

> Práctica orientada al estudio y despliegue de un sistema de base de datos NoSQL utilizando **MongoDB** como DBMS documental, mediante contenedores Docker.

---

## Documentación

| # | Documento | Descripción |
|:-:|:----------|:------------|
| 1 | [Introducción a NoSQL](docs/01-introduccion-nosql.md) | Modelos NoSQL (documentos, grafos, clave-valor, columnas), Teorema CAP, SQL vs NoSQL, Big Data y tendencias |
| 2 | [Paso a paso](docs/02-paso-a-paso.md) | Cómo levantar el proyecto con Docker, configurar, conectar y administrar |
| 3 | [Ejemplo básico guiado](docs/03-ejemplo-basico.md) | Primeros pasos: crear DB, colección, CRUD completo y visualizar en Mongo Express |
| 4 | [Ejercicios básicos](docs/04-ejercicios.md) | Ejercicios prácticos de MongoDB + introducción a Redis, Neo4j y almacenamiento cloud |
| 5 | [Integración SQL + NoSQL](docs/05-integracion.md) | Polyglot persistence, arquitecturas híbridas y casos empresariales |
| 6 | [Glosario de términos](docs/06-glosario.md) | Definiciones de conceptos clave usados en la documentación |
| 7 | [Referencias](docs/07-referencias.md) | Enlaces a documentación oficial, tutoriales y recursos |

---

## Inicio rápido

```bash
# Clonar e ingresar al proyecto
cd practicas/p4-no-sql

# Ver/editar credenciales
cat .env

# Iniciar servicios
docker compose up -d

# Verificar
docker compose ps

# Conectarse a MongoDB
docker exec -it mongo-server mongosh -u root -p mongo_secret_pass

# Abrir Mongo Express
open http://localhost:8081

# Detener
docker compose down
```

## Servicios

| Servicio | Puerto | Credenciales |
|:---------|:-------|:-------------|
| MongoDB | `27017` | `root` / `mongo_secret_pass` |
| Mongo Express | `8081` | `admin` / `admin` |

## Connection String

```
mongodb://root:mongo_secret_pass@localhost:27017/
```
