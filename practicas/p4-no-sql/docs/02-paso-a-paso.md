# 2. Paso a paso — Cómo levantar el proyecto

> [← Volver al README](../README.md)

Guía detallada para levantar MongoDB y Mongo Express usando Docker Compose, desde cero.

---

## Requisitos

- **Docker** instalado ([descargar](https://docs.docker.com/get-docker/))
- **Docker Compose** (viene incluido en Docker Desktop)

Verificar:

```bash
docker --version
docker compose version
```

---

## 1. Estructura del proyecto

```
practicas/p4-no-sql/
├── docker-compose.yml   # Definición de los servicios
├── .env                 # Variables de entorno (credenciales)
├── README.md            # Hub principal
└── docs/                # Documentación
    ├── 01-introduccion-nosql.md
    ├── 02-paso-a-paso.md
    ├── 03-ejemplo-basico.md
    ├── 04-ejercicios.md
    ├── 05-integracion.md
    ├── 06-glosario.md
    └── 07-referencias.md
```

---

## 2. Variables de entorno

El archivo `.env` contiene las credenciales de los servicios:

```bash
MONGO_USER=root
MONGO_PASSWORD=mongo_secret_pass
ME_USER=admin
ME_PASSWORD=admin
```

| Variable | Propósito |
|:---------|:----------|
| `MONGO_USER` | Usuario administrador de MongoDB |
| `MONGO_PASSWORD` | Contraseña del usuario administrador |
| `ME_USER` | Usuario para autenticación básica de Mongo Express |
| `ME_PASSWORD` | Contraseña para autenticación básica de Mongo Express |

Puedes cambiarlas antes de levantar los servicios.

---

## 3. Iniciar los contenedores

```bash
# Parado en la raíz del proyecto (practicas/p4-no-sql/)
docker compose up -d
```

Salida esperada:

```
[+] Running 3/3
 ✔ Network p4-no-sql_app_network   Created
 ✔ Volume "p4-no-sql_mongo_data"   Created
 ✔ Container mongo-server           Started
 ✔ Container mongo-express         Started
```

### ¿Qué hace `docker compose up -d`?

- Descarga las imágenes de Docker Hub (`mongo:7` y `mongo-express:latest`) si no las tenés localmente
- Crea un volumen `mongo_data` para persistir los datos de MongoDB
- Crea una red `app_network` para que los contenedores se comuniquen
- Inicia `mongo-server` (MongoDB) y `mongo-express` (interfaz web)
- El flag `-d` los ejecuta en segundo plano (*detached*)

---

## 4. Verificar que están corriendo

```bash
docker compose ps
```

Salida esperada:

```
NAME                IMAGE                   COMMAND                  SERVICE         STATUS          PORTS
mongo-express       mongo-express:latest    "tini -- /docker-ent…"   mongo-express   Up              0.0.0.0:8081->8081/tcp
mongo-server        mongo:7                "docker-entrypoint.s…"   mongo           Up              0.0.0.0:27017->27017/tcp
```

Ambos deben aparecer como `Up`.

---

## 5. Ver los logs

```bash
# Logs de MongoDB
docker compose logs mongo

# Logs de Mongo Express
docker compose logs mongo-express

# Logs en tiempo real (follow)
docker compose logs -f

# Últimas 10 líneas de cada uno
docker compose logs --tail=10
```

Señales de que MongoDB arrancó bien:

```
Waiting for connections
mongod started
```

---

## 6. Conectarse a MongoDB con mongosh

```bash
docker exec -it mongo-server mongosh -u root -p mongo_secret_pass
```

Una vez dentro, ves el shell de MongoDB:

```
Current Mongosh Log ID: ...
Connecting to: mongodb://<credentials>@127.0.0.1:27017/...
Using MongoDB: 7.0.x
...

test>
```

Probá que funciona:

```javascript
test> db.runCommand({ ping: 1 })
{ ok: 1 }

test> show dbs
admin   100.00 KiB
config   12.00 KiB
local    72.00 KiB
```

Para salir:

```javascript
test> exit
```

### Si olvidaste la contraseña

Podés conectarte sin autenticarte primero y luego ver las variables de entorno dentro del contenedor:

```bash
docker exec -it mongo-server env | grep MONGO
```

---

## 7. Abrir Mongo Express

Mongo Express es una interfaz web para administrar MongoDB, similar a phpMyAdmin.

```
http://localhost:8081
```

Te pedirá autenticación:
- **Usuario:** `admin` (o el valor de `ME_USER` en `.env`)
- **Contraseña:** `admin` (o el valor de `ME_PASSWORD` en `.env`)

Una vez dentro podés:
- Ver todas las bases de datos y colecciones
- Insertar, editar y eliminar documentos
- Crear y eliminar colecciones
- Ver estadísticas del servidor

---

## 8. Detener los contenedores

```bash
# Detener (los contenedores se pueden volver a iniciar)
docker compose stop

# Detener y eliminar contenedores (los datos persisten en el volumen)
docker compose down

# Detener, eliminar contenedores y BORRAR los datos del volumen
docker compose down -v
```

| Comando | Contenedores | Volumen de datos | Red |
|:--------|:-------------|:-----------------|:----|
| `docker compose stop` | Se detienen | Se conserva | Se conserva |
| `docker compose down` | Se eliminan | Se conserva | Se elimina |
| `docker compose down -v` | Se eliminan | **Se elimina** | Se elimina |

> ⚠️ `docker compose down -v` borra TODOS los datos de MongoDB. Usarlo solo si querés empezar de cero.

---

## 9. Solución de problemas comunes

### Puerto 27017 ya en uso

Si ya tenés MongoDB instalado localmente o otro contenedor usando el puerto:

```yaml
# En docker-compose.yml, cambiar el puerto del host
ports:
  - "27018:27017"   # MongoDB escuchará en 27018 en tu máquina
```

### Los contenedores no arrancan

```bash
# Ver logs detallados
docker compose logs

# Verificar que el archivo .env existe y tiene las variables correctas
cat .env

# Reconstruir desde cero
docker compose down -v && docker compose up -d
```

### Mongo Express no se conecta a MongoDB

```bash
# Verificar que los contenedores están en la misma red
docker network inspect p4-no-sql_app_network

# Verificar la URL de conexión en Mongo Express
docker exec mongo-express env | grep ME_CONFIG_MONGODB_URL
```

### Error de permisos en Linux

Si ves errores de permisos en `/data/db`, probablemente sea por SELinux o AppArmor:

```bash
# Solución temporal: deshabilitar SELinux
sudo setenforce 0
```

O agregar `:z` al final del volumen:

```yaml
volumes:
  - mongo_data:/data/db:z
```

---

> **Siguiente:** [Ejemplo básico guiado →](03-ejemplo-basico.md)
