# 6. Glosario de términos

> [← Volver al README](../README.md)

Definiciones de conceptos clave usados en la documentación.

| Término | Definición |
|:--------|:-----------|
| **API** | *Application Programming Interface* — Conjunto de funciones y protocolos que permite que dos aplicaciones se comuniquen entre sí. En MongoDB, la API es el conjunto de métodos (find, insertOne, etc.) que se usan para operar sobre la base de datos. |
| **Big Data** | Conjunto de datos tan grande y complejo que requiere herramientas y técnicas especiales para su procesamiento, más allá de las bases de datos tradicionales. NoSQL surgió en gran parte para manejar Big Data. |
| **Bridge (modo de red Docker)** | Controlador de red por defecto en Docker. Los contenedores conectados a una misma red bridge pueden comunicarse entre sí por nombre de contenedor, aislados del host y de otros contenedores. |
| **Caché** | Capa de almacenamiento temporal de alta velocidad que guarda datos de acceso frecuente para reducir la latencia. Redis es el ejemplo clásico de base de datos utilizada como caché. |
| **CLI** | *Command Line Interface* — Interfaz de línea de comandos. Permite interactuar con el sistema operativo o aplicaciones mediante comandos de texto en lugar de una interfaz gráfica. |
| **Cluster** | Conjunto de servidores (nodos) que trabajan juntos como un solo sistema. En MongoDB, un cluster puede tener múltiples réplicas y shards para alta disponibilidad y escalabilidad. |
| **Connection String (URI)** | Cadena de texto que contiene toda la información necesaria para conectarse a una base de datos: protocolo, usuario, contraseña, host, puerto y base de datos. Ej: `mongodb://user:pass@localhost:27017/mydb`. |
| **CRUD** | *Create, Read, Update, Delete* — Las cuatro operaciones fundamentales de cualquier sistema de almacenamiento persistente. En MongoDB se corresponden con `insert`, `find`, `update` y `delete`. |
| **DBMS** | *Database Management System* — Sistema de software que permite crear, consultar, actualizar y administrar bases de datos. Ejemplos: MongoDB (NoSQL), PostgreSQL (SQL). |
| **Decimal128** | Tipo de dato BSON de 128 bits que permite representar números decimales con precisión exacta, ideal para valores monetarios donde los errores de redondeo de punto flotante son inaceptables. |
| **Denormalización** | Práctica de almacenar datos redundantes en una misma colección para evitar joins o consultas múltiples. Es común en NoSQL para optimizar lecturas a expensas de mayor espacio de almacenamiento. |
| **Driver** | Biblioteca que permite a un lenguaje de programación conectarse e interactuar con una base de datos. MongoDB ofrece drivers oficiales para Python, Node.js, Java, C#, Go, Rust y más. |
| **Escalabilidad horizontal** | Capacidad de aumentar el rendimiento agregando más nodos (servidores) a un sistema distribuido. Es el modelo de escalabilidad principal de NoSQL. |
| **Escalabilidad vertical** | Capacidad de aumentar el rendimiento mejorando el hardware de un solo servidor (más CPU, RAM, disco). Es el modelo típico de las bases de datos SQL tradicionales. |
| **Failover** | Mecanismo automático por el cual, si el nodo principal de un sistema falla, un nodo secundario asume su rol automáticamente para mantener la disponibilidad del servicio. |
| **GeoJSON** | Formato estándar abierto para representar datos geográficos como puntos, líneas y polígonos. MongoDB tiene soporte nativo para consultas geoespaciales sobre datos GeoJSON. |
| **Hash** | Función matemática que transforma datos de entrada en una salida de tamaño fijo (hash). En bases de datos clave-valor y en índices hash de MongoDB, se usa para distribuir y localizar datos rápidamente. |
| **IoT** | *Internet of Things* — Red de dispositivos físicos (sensores, actuadores, wearables) conectados a internet que generan datos continuamente. Las bases de datos time-series y column families son ideales para IoT. |
| **JSON** | *JavaScript Object Notation* — Formato ligero de intercambio de datos basado en pares clave-valor. MongoDB no usa JSON directamente sino BSON, una representación binaria de JSON con tipos adicionales. |
| **Latencia** | Tiempo que tarda una operación en completarse desde que se solicita hasta que se recibe la respuesta. En bases de datos, una latencia baja es crítica para aplicaciones en tiempo real. |
| **MVP** | *Minimum Viable Product* — Versión mínima de un producto que permite validar una idea con el menor esfuerzo posible. MongoDB es popular en MVPs porque su flexibilidad acelera el desarrollo inicial. |
| **Nodo** | Cada servidor o instancia individual que forma parte de un sistema distribuido. En MongoDB, un replica set típico tiene 3 nodos: uno primario (escribe/lee) y dos secundarios (réplicas de solo lectura). |
| **Open Source** | *Código abierto* — Modelo de desarrollo donde el código fuente del software está disponible públicamente para ser usado, modificado y distribuido. MongoDB es open source bajo licencia SSPL. |
| **Pipeline** | Secuencia de etapas donde la salida de una etapa es la entrada de la siguiente. MongoDB usa el Aggregation Pipeline para transformar documentos en varias fases: filtrar, agrupar, ordenar, proyectar, etc. |
| **Políglota / Polyglot Persistence** | Estrategia que consiste en usar múltiples tipos de bases de datos en un mismo sistema, cada una para el tipo de dato que mejor maneja. Ej: PostgreSQL + MongoDB + Redis en una misma aplicación. |
| **RDBMS** | *Relational Database Management System* — Sistema de gestión de bases de datos relacionales (SQL). Ejemplos: PostgreSQL, MySQL, Oracle, SQL Server. Se caracterizan por usar tablas, esquemas fijos y ACID. |
| **Sharding** | Técnica de particionamiento horizontal que distribuye los datos a través de múltiples servidores. Cada shard contiene un subconjunto de los datos. MongoDB implementa sharding automático a nivel de colección. |
| **Throughput** | Cantidad de operaciones que un sistema puede procesar en un período de tiempo determinado. Generalmente se mide en operaciones por segundo (ops/s) o lecturas/escrituras por segundo. |
| **Time-series** | Secuencia de datos ordenados en el tiempo, como lecturas de sensores, métricas de servidor o precios de acciones. Cassandra y MongoDB (con colecciones time-series) son opciones populares para este tipo de datos. |
| **TTL** | *Time To Live* — Tiempo de vida de un dato. MongoDB permite crear índices TTL sobre un campo de fecha para que los documentos se eliminen automáticamente después de cierto tiempo. Útil para sesiones y logs. |

---

> **Siguiente:** [Referencias →](07-referencias.md)
