Proceso 

Aplicación full-stack orientada al monitoreo de recursos del sistema, que permite obtener métricas como uso de CPU, memoria y disco en tiempo real, con persistencia y visualización desde una interfaz web.

Descripción

Este proyecto tiene como objetivo explorar la integración entre distintas tecnologías para construir un sistema distribuido simple. La aplicación obtiene métricas del sistema operativo mediante un proceso en Python y las expone a través de una API desarrollada en .NET, que luego son consumidas por una interfaz web.

El sistema permite tanto ejecutar consultas puntuales como almacenar métricas en una base de datos para su posterior visualización. Además, incluye scripts para simplificar la compilación y ejecución de los distintos componentes.

Funcionalidades
Obtención de métricas del sistema:
Uso de CPU
Uso de memoria
Uso de disco
Ejecución dinámica de tareas mediante requests a la API
Persistencia de métricas en base de datos
Consulta de última medición
Consulta de historial de métricas
Interfaz web para visualización
Arquitectura

El proyecto está dividido en tres partes principales:

frontend: aplicación web en React (Vite)
api: backend en .NET que expone endpoints REST
worker: proceso en Python que obtiene métricas del sistema

El flujo es el siguiente:

El frontend realiza una request al backend
El backend ejecuta el worker de Python mediante un proceso externo
El worker devuelve las métricas en formato JSON
El backend procesa la respuesta y la expone al frontend
Opcionalmente, se guardan los datos en la base de datos

Esta arquitectura permite desacoplar la obtención de métricas del resto del sistema.

Tecnologías utilizadas
.NET (ASP.NET Core Web API)
Python (psutil)
React + Vite
PostgreSQL
JSON para comunicación entre procesos
Ejecución
Opción rápida (recomendada)

El proyecto incluye un script .bat que automatiza la compilación y ejecución de los distintos módulos.

build_all.bat

Este script se encarga de construir los componentes necesarios y dejarlos listos para ejecución.

Backend (.NET)
Ir a la carpeta api
Ejecutar:
dotnet run

La API quedará disponible en http://localhost:5180.

Worker (Python)

Requiere Python 3 y la librería psutil:

pip install psutil

El worker es invocado automáticamente por el backend.

Frontend (React)
Ir a la carpeta dashboard
Ejecutar:
npm install
npm run dev
Endpoints principales
POST /tasks/run
Ejecuta tareas dinámicas (CPU, memoria, disco)
GET /metrics/latest
Devuelve la última métrica registrada
GET /metrics/history
Devuelve historial de métricas
Notas técnicas
La comunicación entre .NET y Python se realiza mediante stdin/stdout, enviando y recibiendo JSON.
El worker utiliza la librería psutil para acceder a métricas del sistema.
El backend maneja la ejecución del proceso Python y transforma la salida en respuestas HTTP.
La persistencia se realiza mediante Entity Framework Core y PostgreSQL.
El sistema está preparado para extenderse con nuevas métricas o tareas.
Posibles mejoras
Ejecución concurrente de múltiples tareas
Configuración de intervalos de muestreo automático
Gráficas en tiempo real
Autenticación de usuarios
Dockerización completa del sistema
Sistema de alertas (por ejemplo, uso alto de CPU)
Enfoque

El objetivo principal fue trabajar con integración entre tecnologías distintas y comunicación entre procesos, implementando:

APIs REST
Ejecución de procesos externos
Serialización/deserialización de JSON
Persistencia de datos
Separación en capas y servicios
