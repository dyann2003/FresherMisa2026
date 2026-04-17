# Architecture

**Analysis Date:** 2026-04-15

## Pattern Overview

**Overall:** Clean Architecture with Repository Pattern and Generic Base Classes

**Key Characteristics:**
- Layered architecture with clear separation of concerns (WebAPI → Application → Infrastructure → Entities)
- Generic base classes (`BaseRepository<T>`, `BaseService<T>`, `BaseController<T>`) for code reuse
- Dependency injection throughout all layers
- Stored procedure-based database operations with Dapper
- Attribute-based entity configuration and validation

## Layers

**WebAPI (Presentation Layer):**
- Purpose: HTTP endpoints and request/response handling
- Location: `FresherMisa2026.WebAPI/`
- Contains: Controllers, Middlewares, Program entry point
- Depends on: Application layer
- Used by: External clients (HTTP requests)

**Application (Business Logic Layer):**
- Purpose: Business rules, validation, orchestration
- Location: `FresherMisa2026.Application/`
- Contains: Services, Service Interfaces, Extensions
- Depends on: Entities, Infrastructure
- Used by: WebAPI Controllers

**Infrastructure (Data Access Layer):**
- Purpose: Database operations and repository implementations
- Location: `FresherMisa2026.Infrastructure/`
- Contains: Repository implementations, Service extensions for DI
- Depends on: Entities
- Used by: Application Services

**Entities (Domain Layer):**
- Purpose: Domain models, enums, base classes, extensions
- Location: `FresherMisa2026.Entities/`
- Contains: Models, DTOs, Enums, Attributes
- Used by: All other layers

## Data Flow

**Request Flow (GET/POST/PUT/DELETE):**

1. **HTTP Request** → `FresherMisa2026.WebAPI/Controllers/BaseController.cs`
2. **Route Matching** → Method invocation (e.g., `Get()`, `Post()`, `Put()`, `DeleteByID()`)
3. **Service Call** → Invokes `IBaseService<T>.GetEntities()`, `Insert()`, `Update()`, `DeleteByID()`
4. **Repository Call** → `IBaseRepository<T>` methods in `FresherMisa2026.Infrastructure/Repositories/BaseRepository.cs`
5. **Database Execution** → Raw SQL/Stored procedures via Dapper + MySQLConnector
6. **Response Serialization** → Results return up the chain as `ServiceResponse`

**Paging Flow:**

1. `GET /api/[controller]/Paging?search=&sort=&pageSize=&pageIndex=&searchFields=`
2. Creates `PagingRequest` object
3. Calls `_baseService.GetFilterPaging(pagingRequest)`
4. Repository calls stored procedure `Proc_{Table}_FilterPaging`
5. Returns `(long Total, IEnumerable<TEntity> Data)` tuple
6. Wrapped in `PagingResponse<TEntity>`

**State Management:**
- Entity state tracked via `ModelSate` enum (Add/Update/Delete) - not persisted to DB
- Soft delete via `IsDeleted` boolean property
- CRUD operations use transaction blocks in repository

## Key Abstractions

**Generic Base Classes:**
- `BaseRepository<TEntity>` - `FresherMisa2026.Infrastructure/Repositories/BaseRepository.cs`
  - Implements `IBaseRepository<TEntity>`
  - Methods: `GetEntities()`, `GetEntityByID()`, `Insert()`, `Update()`, `Delete()`, `GetFilterPaging()`
  - Uses reflection to get table name, primary key from entity attributes

- `BaseService<TEntity>` - `FresherMisa2026.Application/Services/BaseService.cs`
  - Implements `IBaseService<TEntity>`
  - Methods: `GetEntities()`, `GetEntityByID()`, `Insert()`, `Update()`, `DeleteByID()`, `GetFilterPaging()`
  - Contains validation logic via `[IRequired]` attribute

- `BaseController<TEntity>` - `FresherMisa2026.WebAPI/Controllers/BaseController.cs`
  - Generic REST endpoints: `GET`, `GET/{id}`, `POST`, `PUT/{id}`, `DELETE/{id}`, `GET/Paging`
  - Returns `ServiceResponse` wrapper

**Custom Attributes:**
- `[ConfigTable("TableName", hasDeletedColumn, uniqueColumns)]` - Entity configuration
- `[IRequired]` - Field validation marker
- `[Key]` - Primary key indicator

**Interface Abstractions:**
- `IBaseRepository<TEntity>` - `FresherMisa2026.Application/Interfaces/Repositories/IBaseRepository.cs`
- `IBaseService<TEntity>` - `FresherMisa2026.Application/Interfaces/Services/IBaseService.cs`
- `IDepartmentRepository` - `FresherMisa2026.Application/Interfaces/Repositories/IDepartmentRepository.cs`
- `IDepartmentSerice` - `FresherMisa2026.Application/Interfaces/Services/IDepartmentSerice.cs`

## Entry Points

**Application Entry Point:**
- Location: `FresherMisa2026.WebAPI/Program.cs`
- Responsibilities:
  - Creates `WebApplication` builder
  - Configures services (controllers, Swagger, DI)
  - Initializes SQL extension (`SQLExtension.Initialize()`)
  - Registers middleware pipeline
  - Maps endpoints

**DI Registration:**
- Application layer: `FresherMisa2026.Application/ServiceExtensions.cs` → `AddApplicationDI()`
  - Registers: `IBaseService<>` → `BaseService<>`, `IDepartmentSerice` → `DepartmentService`
- Infrastructure layer: `FresherMisa2026.Infrastructure/ServiceExtensions.cs` → `AddInfrastructure()`
  - Registers: `IBaseRepository<>` → `BaseRepository<>`, `IDepartmentRepository` → `DepartmentRepository`

**Controller Entry Points:**
- `DepartmentsController` extends `BaseController<Department>` at `/api/Departments`
- Inherits all base CRUD endpoints
- Adds custom: `GET /api/Departments/Code/{code}`

## Error Handling

**Strategy:** Global middleware with centralized exception handling

**Patterns:**
1. **GlobalExceptionMiddleware** - `FresherMisa2026.WebAPI/Middlewares/GlobalExceptionMiddleware.cs`
   - Wraps entire request pipeline in try-catch
   - Returns standardized `ServiceResponse` with error details
   - Returns HTTP 500 for all unhandled exceptions
   - UserMessage: "Có lỗi xảy ra vui lòng liên hệ Misa!"

2. **Controller-level Error Handling**
   - `BaseController.cs` catches exceptions in `Post()` method
   - Returns 500 with `ex.Message`
   - `Put()` method checks `ResponseCode` for explicit error codes

3. **Response Code Enum** - `FresherMisa2026.Entities/Enums/ResponseCode.cs`
   - Success (200), Created (201), BadRequest (400), NotFound (404), InternalServerError (500)

4. **Repository Transaction Handling**
   - All write operations use `BeginTransaction()` / `Commit()` / `Rollback()`
   - Swallow exceptions in catch blocks (silent failures)

## Cross-Cutting Concerns

**Logging:**
- Console.WriteLine statements in middleware (debug logging)
- No structured logging framework detected

**Validation:**
- Attribute-based: `[IRequired]` on entity properties
- Custom validation: `ValidateRequired()`, `ValidateCustom()` in BaseService
- Virtual methods for override: `ValidateBeforeDelete()`

**Authentication/Authorization:**
- `[Authorize]` attribute imported but not actively used
- `[AllowAnonymous]` not present
- No JWT or authentication middleware configured

**Configuration:**
- `appsettings.json` for connection strings and app settings
- `IConfiguration` injected into repositories for connection string access

**Database:**
- MySQL via `MySqlConnector`
- ORM: Dapper (raw SQL / stored procedures)
- Connection string key: "DefaultConnection"

---

*Architecture analysis: 2026-04-15*