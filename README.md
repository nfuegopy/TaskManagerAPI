# TaskManagerAPI: API de Gestión de Tareas

Esta es una API REST para un sistema de gestión de tareas, creada con .NET 8 como parte de una prueba técnica. La API permite a los usuarios registrarse, iniciar sesión, y gestionar sus tareas personales de forma segura.

**Repositorio del Proyecto**: [https://github.com/nfuegopy/TaskManagerAPI](https://github.com/nfuegopy/TaskManagerAPI)

## Características Principales ✨
- **Autenticación Segura con JWT**: Endpoints para registro y login que generan un token JWT para proteger el acceso a los datos.
- **Hashing de Contraseñas**: Las contraseñas se almacenan de forma segura usando el algoritmo BCrypt.
- **Gestión de Usuarios y Tareas (CRUD)**: Funcionalidad completa para crear, leer, actualizar y eliminar usuarios y tareas.
- **Arquitectura Moderna**: Implementación de Vertical Slice Architecture para máxima mantenibilidad.
- **Paginación y Filtrado**: Endpoints optimizados para manejar grandes volúmenes de datos.
- **Validación de Datos**: Reglas de validación a nivel de DTOs para garantizar la integridad de los datos de entrada.
- **Documentación con Swagger**: Todos los endpoints están documentados con comentarios XML para una fácil exploración y prueba.

## Decisiones Técnicas Clave 🧠

### Estructura del Proyecto: Capas Verticales (Vertical Slice)
En lugar de agrupar archivos por su tipo (Controllers, Models, etc.), organicé el proyecto por características de negocio. Todo el código para una funcionalidad (como Tasks) está en su propia carpeta (`Features/Tasks`).  
**¿Por qué?** Es más práctico y escalable. Si necesitas cambiar algo sobre las tareas, no tienes que buscar en cinco carpetas distintas. Todo lo que necesitas está en un solo lugar, lo que hace el código mucho más fácil de mantener.

### Creación de Usuarios: Registro Público vs. Gestión de Admin
Hay dos formas de crear un usuario, y es a propósito:  
- `POST /api/auth/register`: Es la puerta para el público. Cualquiera puede usarla para crear su propia cuenta.  
- `POST /api/users`: Es la puerta para un administrador. En una aplicación real, esta ruta estaría protegida para que solo un admin pudiera usarla para crear usuarios directamente, enviarles una contraseña autogenerada y forzar un cambio en el primer login.

### Seguridad y Rendimiento
- **Validación de Datos**: Se implementaron Data Annotations en los DTOs (ej: `CreateUserDto`) para validar los datos de entrada (longitud de contraseña, formato de email, etc.) antes de que lleguen a la lógica de negocio.
- **Paginación**: Los endpoints que devuelven listas (`GET /api/users` y `GET /api/tasks`) soportan paginación a través de parámetros en la URL (`?pageNumber=1&pageSize=10`) para asegurar un buen rendimiento con grandes cantidades de datos.
- **Protección contra Inyección SQL**: El uso de Entity Framework Core con consultas parametrizadas protege la aplicación de forma nativa contra este tipo de ataques.

### Manejo de Problemas Técnicos
- **Ambigüedad de "Task"**: El nombre de la clase `Task` chocaba con la clase `Task` del sistema de .NET. En lugar de cambiar el nombre del modelo, se utilizó un alias de using (`using Task = ...`), una práctica estándar en .NET para resolver este tipo de conflictos.
- **Referencias Cíclicas**: La relación entre `User` y `Task` creaba un bucle infinito al generar el JSON de respuesta. Se solucionó configurando el serializador de JSON en `Program.cs` para ignorar estos ciclos.

## Cómo Ejecutar el Proyecto

### 🖥️ Para Windows (con SQL Server LocalDB)
El proyecto está configurado para usar SQL Server LocalDB, que se instala con Visual Studio.

**Requisitos**:  
- .NET 8 SDK  
- Git  
- SQL Server Express LocalDB  

**Pasos en la Terminal**:
```bash
# 1. Clona el proyecto
git clone https://github.com/nfuegopy/TaskManagerAPI.git
cd TaskManagerAPI

# 2. Restaura los paquetes NuGet
dotnet restore

# 3. Crea la base de datos a partir de las migraciones
dotnet ef database update

# 4. Ejecuta la API
dotnet run
```
La API estará corriendo en la URL que indique la terminal (ej: `https://localhost:7141`).

### 🐧 Para Linux / macOS (con SQL Server en Docker)
En otros sistemas, necesitarás una instancia de SQL Server y ajustar la cadena de conexión.

**Requisitos**:  
- .NET 8 SDK  
- Git  
- Una instancia de SQL Server (ej: Docker)

**Pasos**:
1. Sigue los mismos pasos de clonación y restauración de arriba.
2. Ajusta la Cadena de Conexión: Abre `appsettings.json` y modifica la `DefaultConnection` para que apunte a tu instancia.  
   **Ejemplo**: 
   ```json
   Server=localhost,1433;Database=TaskManagerDb;User=sa;Password=TuPasswordSeguro123;TrustServerCertificate=True;
   ```
3. Continúa con los comandos:
   ```bash
   dotnet ef database update
   dotnet run
   ```

## Notas de Configuración
- **Expiración del Token JWT**: Se ha configurado una expiración de 8 horas en el archivo `AuthService.cs`.
- **Puerto de la Aplicación**: El puerto por defecto (7141) se puede cambiar en el archivo `Properties/launchSettings.json`.