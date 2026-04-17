# Codebase Structure

**Analysis Date:** 2026-04-15

## Directory Layout

```
FresherMisa2026/
├── FresherMisa2026.WebAPI/           # ASP.NET Core Web API project
├── FresherMisa2026.Application/      # Business logic layer
├── FresherMisa2026.Infrastructure/   # Data access layer
├── FresherMisa2026.Entities/          # Domain models and shared types
├── FresherMisa2026.slnx              # Solution file
└── *.sql                              # Database scripts
```

## Directory Purposes

**FresherMisa2026.WebAPI/ (Presentation Layer):**
- Purpose: HTTP API hosting and request handling
- Contains: Controllers, Middlewares, Program.cs, appsettings.json
- Key files:
  - `Program.cs` - Application entry point and configuration
  - `Controllers/BaseController.cs` - Generic CRUD controller
  - `Controllers/DepartmentsController.cs` - Department-specific endpoints
  - `Middlewares/GlobalExceptionMiddleware.cs` - Global error handling

**FresherMisa2026.Application/ (Business Logic Layer):**
- Purpose: Business rules, service interfaces, and implementations
- Contains: Services, Interfaces, Extensions
- Key files:
  - `Services/BaseService.cs` - Generic base service with CRUD + validation
  - `Services/Department/DepartmentService.cs` - Department business logic
  - `Interfaces/Services/IBaseService.cs` - Service contract
  - `Interfaces/Services/IDepartmentSerice.cs` - Department service contract
  - `Interfaces/Repositories/IBaseRepository.cs` - Repository contract
  - `Interfaces/Repositories/IDepartmentRepository.cs` - Department repository contract
  - `ServiceExtensions.cs` - DI registration for Application layer
  - `Extensions/SQLExtension.cs` - SQL initialization

**FresherMisa2026.Infrastructure/ (Data Access Layer):**
- Purpose: Database operations and repository implementations
- Contains: Repository implementations
- Key files:
  - `Repositories/BaseRepository.cs` - Generic DB operations with Dapper
  - `Repositories/DepartmentRepository.cs` - Department-specific queries
  - `ServiceExtensions.cs` - DI registration for Infrastructure layer

**FresherMisa2026.Entities/ (Domain Layer):**
- Purpose: Domain models, enums, attributes, and extensions
- Contains: Models, DTOs, Enums, Attributes, Extensions
- Key files:
  - `BaseModel.cs` - Base class for all entities
  - `Department/Department.cs` - Department entity
  - `ServiceResponse.cs` - Standard API response wrapper
  - `PagingRequest.cs` - Paging input DTO
  - `PagingResponse.cs` - Paging output DTO
  - `ModelSate.cs` - Entity state enum
  - `Enums/ResponseCode.cs` - HTTP status codes
  - `Extensions/ConfigTable.cs` - Table configuration attribute
  - `Extensions/MethodExtensions.cs` - Reflection helpers

## Key File Locations

**Entry Points:**
- `FresherMisa2026.WebAPI/Program.cs` - Application bootstrap

**Configuration:**
- `FresherMisa2026.WebAPI/appsettings.json` - App settings and connection strings

**Core Logic:**
- `FresherMisa2026.Application/Services/BaseService.cs` - Generic service base
- `FresherMisa2026.Infrastructure/Repositories/BaseRepository.cs` - Generic repository base
- `FresherMisa2026.WebAPI/Controllers/BaseController.cs` - Generic controller base

**Domain Models:**
- `FresherMisa2026.Entities/BaseModel.cs` - Base entity
- `FresherMisa2026.Entities/Department/Department.cs` - Department entity

**Database Scripts:**
- `misaemployee_development_department.sql`
- `misaemployee_development_employee.sql`
- `misaemployee_development_position.sql`

## Naming Conventions

**Files:**
- PascalCase: `BaseController.cs`, `DepartmentService.cs`, `ServiceResponse.cs`
- Suffix pattern: `{EntityName}.cs` for models, `{EntityName}Service.cs` for services, `{EntityName}Controller.cs` for controllers

**Directories:**
- PascalCase: `Controllers/`, `Services/`, `Repositories/`, `Extensions/`
- Entity subfolder: `Department/`, `Employee/`

**Interfaces:**
- Prefix `I`: `IBaseService.cs`, `IDepartmentSerice.cs`, `IBaseRepository.cs`

**Classes:**
- Suffix: `Service`, `Controller`, `Repository`, `Middleware`, `Extension`, `Response`

**Methods:**
- PascalCase: `GetEntities()`, `GetEntityByID()`, `Insert()`, `Update()`, `Delete()`

**Properties:**
- PascalCase: `DepartmentCode`, `DepartmentName`, `IsSuccess`, `PageIndex`

## Where to Add New Code

**New Entity:**
1. Model: `FresherMisa2026.Entities/{EntityName}/{EntityName}.cs` (extend `BaseModel`, add `[ConfigTable]` attribute)
2. Repository Interface: `FresherMisa2026.Application/Interfaces/Repositories/I{EntityName}Repository.cs`
3. Repository Implementation: `FresherMisa2026.Infrastructure/Repositories/{EntityName}Repository.cs`
4. Service Interface: `FresherMisa2026.Application/Interfaces/Services/I{EntityName}Service.cs`
5. Service Implementation: `FresherMisa2026.Application/Services/{EntityName}/{EntityName}Service.cs` (extend `BaseService<{EntityName}>`)
6. Controller: `FresherMisa2026.WebAPI/Controllers/{EntityName}Controller.cs` (extend `BaseController<{EntityName}>`)
7. DI Registration: Add to both `FresherMisa2026.Application/ServiceExtensions.cs` and `FresherMisa2026.Infrastructure/ServiceExtensions.cs`

**New Service Method:**
- Add to interface: `FresherMisa2026.Application/Interfaces/Services/I{EntityName}Service.cs`
- Implement in: `FresherMisa2026.Application/Services/{EntityName}/{EntityName}Service.cs`
- Call from: `FresherMisa2026.WebAPI/Controllers/{EntityName}Controller.cs`

**New Validation:**
- Add `[IRequired]` attribute to entity property in `FresherMisa2026.Entities/`
- Or override `ValidateCustom()` in service class

**New Middleware:**
- Create in: `FresherMisa2026.WebAPI/Middlewares/`
- Register in: `FresherMisa2026.WebAPI/Program.cs` with `app.UseMiddleware<T>()`

**New Database Script:**
- Add `.sql` file at root level following naming: `misaemployee_development_{table}.sql`

## Special Directories

**Extensions:**
- Location: `FresherMisa2026.Entities/Extensions/`
- Purpose: Reflection-based helpers for entity metadata (table names, keys, display names)
- Contains: `ConfigTable.cs`, `MethodExtensions.cs`

**Services Subfolder:**
- Location: `FresherMisa2026.Application/Services/Department/`
- Purpose: Entity-specific service implementations
- Pattern: One folder per entity type

**Controllers:**
- Location: `FresherMisa2026.WebAPI/Controllers/`
- Purpose: HTTP endpoint handlers
- Contains: `BaseController.cs` (generic), specific controllers

**Middlewares:**
- Location: `FresherMisa2026.WebAPI/Middlewares/`
- Purpose: Cross-cutting concerns (exception handling)
- Contains: `GlobalExceptionMiddleware.cs`

---

*Structure analysis: 2026-04-15*