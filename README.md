# CompanyEmployees Preview - Arquitectura CQRS + Servicios

Una versión **lite y representativa** del proyecto principal, que demuestra patrones reales con **CQRS**, **JWT**, servicios infraestructura, y relaciones entre entidades.

## 🎯 Filosofía

Este proyecto **representa fielmente** la arquitectura del proyecto original en miniatura, eligiendo los patrones más destacados:

✅ **3 Entidades** con relaciones (Employee ↔ Department, Employee ↔→ Role)  
✅ **CQRS Pattern** - Separación de Comandos y Queries  
✅ **JWT Authentication** - Autenticación basada en tokens  
✅ **Servicios de Infraestructura** - Audit, Notifications, Resilience  
✅ **Mediator Pattern** - Desacoplamiento de lógica  
✅ **Layered Architecture** - 3 capas (Presentation, Application, Infrastructure)  
✅ **Entity Framework Core** - ORM con relaciones  
✅ **Swagger/OpenAPI** - Documentación automática  
✅ **Logging Estructurado** - Serilog integrado

## 📁 Estructura (3 Capas + Relaciones)

```
preview/
├── PreviewApi/                           # Presentation Layer
│   ├── Controllers/
│   │   └── EmployeesController.cs       # API REST (usa Mediator)
│   ├── Program.cs
│   └── PreviewApi.csproj
│
├── PreviewApi.Application/               # Application Layer
│   ├── Common/
│   │   ├── Mediator.cs                  # CQRS interfaces + Mediador simple
│   │   └── Dtos.cs                      # Data Transfer Objects
│   ├── Commands/Employee/
│   │   └── EmployeeCommands.cs          # CreateEmployeeCommand, etc
│   ├── Handlers/Commands/
│   │   └── EmployeeCommandHandlers.cs   # Handlers para Commands
│   ├── Queries/Employee/
│   │   └── EmployeeQueries.cs           # GetAllEmployeesQuery, etc
│   ├── Handlers/Queries/
│   │   └── EmployeeQueryHandlers.cs     # Handlers para Queries
│   └── PreviewApi.Application.csproj
│
├── PreviewApi.Infrastructure/            # Infrastructure Layer
│   ├── Entities/
│   │   └── Employee.cs                  # Domain Models
│   ├── Data/
│   │   └── AppDbContext.cs              # DbContext
│   └── PreviewApi.Infrastructure.csproj
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## 🚀 Quick Start

### Opción 1: Docker Compose (Recomendado)

```powershell
cd preview
docker compose up -d
```

La API estará en: `http://localhost:5000`
Swagger: `http://localhost:5000/swagger`

### Opción 2: Desarrollo Local

```powershell
cd preview/PreviewApi

# Restaurar librerías
dotnet restore

# Ejecutar
dotnet run
```

## 📜 Flujo CQRS en Acción

Cuando haces `POST /api/employees`:

```
1. Controller.CreateEmployee(dto)
        ↓
2. Controller convierte DTO a Command
        ↓
3. Mediator.Send(CreateEmployeeCommand)
        ↓
4. IMediator busca ICommandHandler<CreateEmployeeCommand, EmployeeDto>
        ↓
5. CreateEmployeeCommandHandler.Handle(command)
        ↓
6. Handler valida, persiste en DB, notifica eventos
        ↓
7. Retorna EmployeeDto
        ↓
8. Controller retorna 201 Created
```

### En Código

```csharp
// Controller
[HttpPost]
public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto)
{
    var command = new CreateEmployeeCommand(dto);
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
}

// Handler
public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand command, CancellationToken ct)
    {
        var employee = new Employee { /* mapping */ };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(ct);
        return MapToDto(employee);
    }
}
```

## 📚 API Endpoints

### Employees

| Método | Endpoint              | Patrón CQRS                      | Require Auth |
| ------ | --------------------- | -------------------------------- | ------------ |
| POST   | `/api/auth/login`     | Service: `JwtTokenService`       | ❌           |
| GET    | `/api/employees`      | Query: `GetAllEmployeesQuery`    | ❌           |
| GET    | `/api/employees/{id}` | Query: `GetEmployeeByIdQuery`    | ❌           |
| POST   | `/api/employees`      | Command: `CreateEmployeeCommand` | ❌           |
| PUT    | `/api/employees/{id}` | Command: `UpdateEmployeeCommand` | ❌           |
| DELETE | `/api/employees/{id}` | Command: `DeleteEmployeeCommand` | ❌           |

## 🔐 Autenticación JWT

Este proyecto implementa **autenticación basada en JWT tokens**:

```bash
# 1. Obtener token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "john.doe@example.com"}'

# Response
{
  "userId": -123456789,
  "email": "john.doe@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

# 2. Usar el token en requests autenticados (futuro)
curl http://localhost:5000/api/employees \
  -H "Authorization: Bearer <token>"
```

**Token Properties**:

- Algoritmo: HS256
- Expiración: 60 minutos
- Claims: UserId, Email, Role

## 🔧 Servicios de Infraestructura

La preview implementa servicios reales como en el proyecto principal:

### AuditService

Registra todas las operaciones (Create, Update, Delete) para auditoría.

```csharp
await _auditService.LogAsync("Employee", "Create", userId, employeeData);
```

### NotificationService

Gestiona notificaciones (email, push, etc).

```csharp
await _notificationService.SendAsync(
    "New Employee",
    "Employee Alice assigned to Engineering",
    "manager@company.com");
```

### ResilienceService

Implementa **retry pattern** con exponential backoff para operaciones resilientes.

```csharp
var result = await _resilienceService.ExecuteWithRetryAsync(
    async () => await _context.SaveChangesAsync(),
    maxRetries: 3
);
```

## 📊 Entidades y Relaciones

La preview simula las relaciones del proyecto real:

```
Employee (1) ──┬──→ (1) Department
               │
               └──→ (*) Role (Many-to-Many via EmployeeRole)
```

### Employee

- FirstName, LastName, Email
- HireDate, IsActive
- **FK**: DepartmentId, Roles (M:M)

### Department

- Name, Description
- IsActive
- **Relation**: Employee (1:M)

### Role

- Name, Description
- IsActive
- **Relation**: Employee (M:M)

## 🔑 Conceptos Clave

### CQRS (Command Query Responsibility Segregation)

- **Commands**: Modifican estado (Create, Update, Delete)
- **Queries**: Lee estado sin cambios (Get)
- **Handlers**: Contienen la lógica específica
- **Mediator**: Desacopla Controller → Handler

**Beneficios**:
✅ Separación de concerns claras
✅ Escalabilidad (queries ≠ commands)
✅ Testing fácil (unit tests de handlers)
✅ Patrón en proyecto principal

### Mediator Pattern

En lugar de que el Controller llame directamente al servicio:

```csharp
// ❌ Sin Mediator
var employee = _employeeService.CreateEmployee(dto);

// ✅ Con Mediator
var command = new CreateEmployeeCommand(dto);
var employee = await _mediator.Send(command);
```

**Ventajas**:

- Desacoplamiento
- Reutilización de handlers
- Fácil agregar cross-cutting concerns (logging, validación, etc)

### Layered Architecture

```
┌─────────────────────────────────────────┐
│     Presentation (PreviewApi)           │  Controllers, DTOs
├─────────────────────────────────────────┤
│    Application (PreviewApi.Application) │  Commands, Queries, Handlers
├─────────────────────────────────────────┤
│ Infrastructure (PreviewApi.Infrastructure)│  DbContext, Entities, Data Access
└─────────────────────────────────────────┘
```

Cada capa tiene responsabilidad clara ≠ no hay mezcla de concerns.

## 🔧 Configuración

### Base de Datos

```csharp
// appsettings.Development.json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PreviewApi;..."
}

// appsettings.json (Docker)
"ConnectionStrings": {
  "DefaultConnection": "Server=sqlserver;Database=PreviewApi;..."
}
```

### Servicios Registrados

En `Program.cs`:

```csharp
// Application Services (CQRS)
builder.Services.AddApplicationServices();

// Infrastructure Services (DB)
builder.Services.AddInfrastructureServices(connectionString);
```

## 📊 Diferencias con CRUD Simple

| Aspecto           | CRUD Simple           | CQRS                            |
| ----------------- | --------------------- | ------------------------------- |
| **Patrón**        | Direct DB calls       | Mediator + Handlers             |
| **Estructura**    | Controller → Service  | Controller → Mediator → Handler |
| **Testing**       | Difícil (DB acoplado) | Fácil (handler aislado)         |
| **Escalabilidad** | Reads/Writes juntos   | Separados                       |
| **Proyecto Real** | ❌                    | ✅                              |

## 🎓 Aprendizajes

Esta preview te enseña:

1. ✅ Cómo estruturar con CQRS
2. ✅ Patrón Mediator
3. ✅ Dependency Injection avanzado
4. ✅ Layered architecture
5. ✅ Entity Framework Core patterns
6. ✅ Logging y debugging
7. ✅ Docker en .NET

## 🚀 Próximos Pasos

Puedes extender con:

- 🔐 Autenticación JWT
- 📋 Validación con FluentValidation
- 📢 Domain Events
- 🔔 Notificaciones
- 🗂️ Especificaciones (Repository Pattern mejorado)
- 📊 Caching estratégico
- 🔄 Transacciones y Unit of Work
- 📝 Auditoría y soft delete

## 🔗 Referencias

- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Mediator Pattern](https://refactoring.guru/design-patterns/mediator)
- [Layered Architecture](https://herbertograca.com/2017/08/07/layered-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

## 📞 Notas

- Esta preview usa un **Mediator simple** (sin librerías externas)
- En producción, considera **MediatR** para features avanzadas
- Los **Handlers** se registran automáticamente por reflection
- **Logging** está construido en cada handler

## 📄 Licencia

MIT
