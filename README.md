# TaskManagerAPI: API de Gestión de Tareas

Esta es una API REST para un sistema de gestión de tareas, creada con .NET 8 como parte de una prueba técnica.

**Repositorio del Proyecto**: [https://github.com/nfuegopy/TaskManagerAPI](https://github.com/nfuegopy/TaskManagerAPI)

## Decisiones Clave Durante el Desarrollo

Aquí explico algunas de las decisiones que tomé para construir el proyecto de una forma moderna y fácil de mantener.

1. **Estructura del Proyecto: Capas Verticales (Vertical Slice)**  
   En lugar de agrupar archivos por su tipo (Controllers, Models, etc.), organicé el proyecto por características de negocio. Todo el código para una funcionalidad (como Tasks) está en su propia carpeta (`Features/Tasks`).  
   **La razon** es más práctico. Si quieres cambiar algo sobre las tareas, no tenes que buscar en cinco carpetas distintas. Todo lo que necesitas está en un solo lugar, lo que hace el código mucho más fácil de mantener.

2. **Creación de Usuarios: Registro Público vs. Gestión de Admin**  
   Hay dos formas de crear un usuario, y es a propósito:  
   - `POST /api/auth/register`: Es la puerta para el público. Cualquiera puede usarla para crear su propia cuenta.  
   - `POST /api/users`: Es la puerta para un administrador. En una aplicación real, esta ruta estaría protegida para que solo un admin pudiera usarla para crear usuarios directamente.  
     Esta separación permite tener un registro público simple y un sistema de gestión interno más potente.
   - Ejemplo no aplicado pero se puede, para no tocar el modelo solicitado en la prueba no agregue un campo extra, este campo extra
	 lo que haria es tener un rol en caso de ser admin, el usuario se creara por medio de un formulario se condicionaria, para no tener que enviar
	 la contraseña la cual se puede autogenerar por medio de una funcion ya sea en la bd o en la misma y ser enviada al usuario, una vez que se loguea le obligue
	 a cambiar el password generado, pero esto seria en un escenario real, de igual manera deje ambos endpoint por ese motivo.
			

3. **Solución a un Problema Técnico: Ambigüedad de "Task"**  
   Durante el desarrollo, el nombre de mi clase para las tareas, `Task`, chocaba con la clase `Task` del sistema de .NET.  
   **Solución**: En vez de cambiarle el nombre a mi clase, ejemplo pasarlo a español tareas, usé un alias de using en los archivos necesarios: `using Task = TaskManagerAPI.Features.Tasks.Task;`. Esto se puede explicar mejor en un post de staroverflow justamente.
1. esto seria una explicacion aproximada que se encuentra https://stackoverflow.com/questions/44523562/why-does-adding-a-namespace-alias-in-c-sharp-remove-ambiguity.

4. **Manejo de Errores: Un Middleware Global**  
   Implementé un Middleware de Manejo de Excepciones Global. Esta es una "red de seguridad" que atrapa cualquier error inesperado en la aplicación, lo registra internamente y devuelve una respuesta de error 500 genérica y segura, evitando que la aplicación se caiga o filtre información sensible.
1. Ejemplo si ya existe un usuario con el mismo correo no es necesario mostrar el correo con la respuesta del Middleware manejamos esos casos sin exponer

## Cómo Ejecutar el Proyecto

Las instrucciones varían ligeramente dependiendo de tu sistema operativo, principalmente por la base de datos.

### 🖥️ Para Windows (con SQL Server LocalDB)

En Windows, es común usar SQL Server Express LocalDB, que se instala automáticamente con Visual Studio. El proyecto está configurado para usarlo por defecto.

**Requisitos**:  
- .NET 8 SDK  
- Git  
- SQL Server Express LocalDB  

**Pasos en la Terminal (PowerShell o CMD)**:

Clona el proyecto:
```bash
git clone https://github.com/nfuegopy/TaskManagerAPI.git
cd TaskManagerAPI
```

Instala las dependencias: Este comando lee el archivo del proyecto y descarga todo lo necesario.
```bash
dotnet restore
```

Crea la Base de Datos: Este comando usa Entity Framework para crear la base de datos `TaskManagerDb` en tu LocalDB.
```bash
dotnet ef database update
```

Ejecuta la API:
```bash
dotnet run
```
La terminal te mostrará la URL donde está corriendo la API (normalmente `https://localhost:7141`).

### 🐧 Para Linux (con SQL Server en Docker u otro)

En Linux, no existe LocalDB. Deberás tener una instancia de SQL Server corriendo (por ejemplo, en Docker) y ajustar la cadena de conexión.

**Requisitos**:  
- .NET 8 SDK  
- Git  
- Una instancia de SQL Server accesible desde tu máquina.

**Pasos en la Terminal**:

Clona el proyecto:
```bash
git clone https://github.com/nfuegopy/TaskManagerAPI.git
cd TaskManagerAPI
```

Instala las dependencias:
```bash
dotnet restore
```

Ajusta la Cadena de Conexión: Abre el archivo `appsettings.json` con un editor de texto y modifica la `DefaultConnection` para que apunte a tu instancia de SQL Server.  
**Ejemplo**: 
```json
Server=localhost,1433;Database=TaskManagerDb;User=sa;Password=TuPasswordSeguro123;TrustServerCertificate=True;
```

Crea la Base de Datos: El comando es el mismo. Entity Framework usará la nueva cadena de conexión que configuraste.
```bash
dotnet ef database update
```

Ejecuta la API:
```bash
dotnet run
```

La API se ejecutará y podrás acceder a ella desde la URL que te indique la terminal.

## Notas de Configuración

- **Expiración del Token JWT**: En el archivo `AuthService.cs`, el token está configurado para expirar en 5 minutos para facilitar las pruebas. Para un uso real, se puede cambiar a 8 horas o más (la línea de código está comentada justo al lado).
- la linea especifica es 