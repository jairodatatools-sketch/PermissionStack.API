
# PermissionStack.API

## Arquitectura y estructura de la solución

La solución **PermissionStack.API** está organizada en 5 proyectos principales con responsabilidades claras, siguiendo principios de arquitectura limpia y separación de capas:

### 0. PermissionStack.API
- Proyecto Web API que expone los endpoints REST.
- Carpetas principales:
  - **Controllers**: Controladores API que reciben las solicitudes HTTP.
  - **.docker**: Archivos y configuraciones Docker para contenerización.
  - **Logs**: Archivos generados de logging de la aplicación.

### 1. PermissionStack.Application
- Contiene la lógica de aplicación y mediación.
- Carpetas principales:
  - **Commands**: Comandos para operaciones que modifican estado (CRUD).
  - **DTOs**: Objetos de transferencia de datos para entrada/salida.
  - **Handlers**: Manejadores que procesan comandos y consultas.
  - **Interfaces**: Contratos usados en la capa de aplicación.
  - **Queries**: Consultas para obtener datos sin modificar.

### 2. PermissionStack.Domain
- Modelos del dominio y reglas de negocio.
- Carpetas principales:
  - **Entities**: Clases que representan entidades del dominio.
  - **Interfaces**: Contratos relacionados con el dominio.

### 3. PermissionStack.Infrastructure
- Implementación de acceso a datos y servicios externos.
- Carpetas principales:
  - **Persistence**: Contexto de base de datos y configuración EF Core.
  - **Repositories**: Implementaciones de repositorios para acceso a datos.
  - **Services**: Servicios para integración con otros sistemas (ej. Kafka, Elasticsearch).

### 4. PermissionStack.Tests
- Proyecto para pruebas unitarias e integración.
- Carpetas principales:
  - **Handlers**: Pruebas para los manejadores de comandos y consultas.

---

## Configuración y despliegue con Docker

El proyecto está preparado para ejecutarse en contenedores Docker, facilitando el despliegue y el desarrollo en entornos Linux.

### Contenedores involucrados:

- **sqlserver**: Imagen oficial de SQL Server 2022 en Linux, expuesto en el puerto 1433.
- **permissionstackapi**: Aplicación Web API .NET Core escuchando en el puerto 8080 (expuesto en el host como 5100).
- **elasticsearch**: Motor de búsqueda para logging y análisis, puerto 9200.
- Otros servicios como **zookeeper** para coordinación de Kafka, etc.

### Ejemplo básico de `docker-compose.yml` para levantar los servicios:

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrong!Passw0rd"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sqlserverdata:/var/opt/mssql

  permissionstackapi:
    image: docker-permissionstackapi
    depends_on:
      - sqlserver
    ports:
      - "5100:8080"
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=PermissionStackDb;User Id=sa;Password=YourStrong!Passw0rd;"

volumes:
  sqlserverdata:
````

---

## Conexión a SQL Server desde Docker en Linux

Para evitar errores comunes de conexión:

* Usa el nombre del contenedor de SQL Server como servidor en la cadena de conexión (`Server=sqlserver`).
* Asegúrate que el contenedor SQL Server esté corriendo y accesible (`docker ps`).
* El usuario y contraseña deben coincidir con la configuración del contenedor (`sa` y `YourStrong!Passw0rd`).
* Verifica que el firewall o políticas de red no bloqueen el puerto 1433.
* Desde el contenedor API, puedes probar conectividad con:

```bash
docker exec -it permissionstackapi bash
# Dentro del contenedor:
apt-get update && apt-get install -y curl
curl sqlserver:1433
```

---

## Manejo del error: "Could not open a connection to SQL Server"

Si recibes un error similar a:

```
Microsoft.Data.SqlClient.SqlException (0x80131904): A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible.
```

* Confirma que el nombre del servidor en la cadena de conexión es correcto (debe ser el nombre del contenedor, ej. `sqlserver`).
* Asegúrate que el contenedor SQL Server está levantado y listo. Revisa logs con:

```bash
docker logs sqlserver
```

* Espera a que SQL Server termine de inicializarse antes de iniciar la API.
* Comprueba que la API tiene la cadena de conexión adecuada, puede definirse en `appsettings.json` o vía variables de entorno en Docker.

---

## Ejemplo de configuración del DbContext en Infrastructure

```csharp
using Microsoft.EntityFrameworkCore;
using PermissionStack.Domain.Entities;

namespace PermissionStack.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
    }
}
```

En `Program.cs` o `Startup.cs`, registra el contexto con:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## Contacto y soporte

Para dudas o problemas, puedes contactarme en \[jairo.datatools@gmail.com].

---

¡Gracias por usar PermissionStack!

```
