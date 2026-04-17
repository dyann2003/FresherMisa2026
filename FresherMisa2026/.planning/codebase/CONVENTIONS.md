# Coding Conventions

**Analysis Date:** 2026-04-15

## Naming Patterns

**Files:**
- PascalCase: `DepartmentService.cs`, `BaseController.cs`, `ServiceResponse.cs`
- Entity files in folders: `Department/Department.cs`

**Classes:**
- PascalCase: `DepartmentService`, `BaseController`, `GlobalExceptionMiddleware`
- Prefix for base classes: `BaseService`, `BaseController`, `BaseModel`, `BaseRepository`
- Suffix for interfaces: `IBaseService`, `IDepartmentSerice`

**Functions:**
- PascalCase: `GetEntities`, `GetEntityByID`, `DeleteByID`, `ValidateRequired`
- Async suffix: `GetDepartmentByCodeAsync`

**Variables:**
- CamelCase for local variables: `entity`, `isValid`, `rowAffects`, `properties`, `pagingRequest`
- Underscore prefix for private fields: `_baseRepository`, `_serviceResult`, `_tableName`, `_configuration`, `_dbConnection`

**Types:**
- PascalCase: `ServiceResponse`, `PagingResponse`, `Department`, `ResponseCode`

## Code Style

**Formatting:**
- Standard C# formatting with Visual Studio defaults
- No explicit .editorconfig detected but implicit formatting follows .NET conventions
- Tab indentation with standard VS settings

**Linting:**
- No explicit linting configuration detected
- Uses .NET 10.0 `ImplicitUsings` and `Nullable` enabled in csproj

**Patterns:**
- Regions for organizing code: `#region Method Get`, `#region OVERRIDE METHODS`, `#region Declare`, `#region Constructer`, `#region Methods`
- Property initializers inline
- Single-line property declarations when simple

## Import Organization

**Order in files:**
1. System namespaces: `using System;`, `using System.Collections.Generic;`
2. Application namespaces: `using FresherMisa2026.Application.Extensions;`, `using FresherMisa2026.Application.Interfaces;`
3. Entity namespaces: `using FresherMisa2026.Entities;`, `using FresherMisa2026.Entities.Department;`
4. Framework namespaces: `using Microsoft.AspNetCore.Mvc;`

**Path Aliases:**
- No aliases detected

## Error Handling

**Patterns:**
- Global exception middleware: `FresherMisa2026.WebAPI\Middlewares\GlobalExceptionMiddleware.cs`
- ServiceResponse pattern: `FresherMisa2026.Entities\ServiceResponse.cs`
- ResponseCode enum: `FresherMisa2026.Entities\Enums\ResponseCode.cs`
- Try-catch in repository with transaction rollback

**ServiceResponse structure:**
```csharp
public class ServiceResponse
{
    public bool IsSuccess { get; set; }
    public int Code { get; set; }
    public object Data { get; set; }
    public object UserMessage { get; set; }
    public object DevMessage { get; set; }
}
```

**Error codes:**
- Success = 200
- Created = 201
- BadRequest = 400
- NotFound = 404
- InternalServerError = 500

## Logging

**Framework:** Console.WriteLine (basic)

**Pattern in GlobalExceptionMiddleware:**
```csharp
Console.WriteLine("Before run middleware");
await _next(context);
Console.WriteLine("After run middleware");
```

**No structured logging framework detected**

## Comments

**When to Comment:**
- All public methods have XML documentation with Vietnamese descriptions
- Private helper methods have comments

**JSDoc/TSDoc equivalent (C# XML Documentation):**
```csharp
/// <summary>
/// Lấy tất cả bản ghi
/// </summary>
/// <returns>Danh sách bản ghi</returns>
/// CREATED BY: DVHAI 11/07/2026
public async Task<IEnumerable<TEntity>> GetEntities()
```

**Creator attribution format:**
- `/// CREATED BY: DVHAI (11/07/2026)` - uppercase
- `/// Created By: dvhai (10/04/2026)` - mixed case

## Function Design

**Size:** Not enforced, but base classes provide reusable implementations

**Parameters:**
- Aligned on continuation lines
- Named parameters for clarity: `GetFilterPaging(pagingRequest.PageSize, pagingRequest.PageIndex, pagingRequest.Search, fields, pagingRequest.Sort)`

**Return Values:**
- Task for async methods: `public async Task<ServiceResponse>`
- IEnumerable for collections: `public async Task<IEnumerable<BaseModel>> GetEntities()`
- Tuple for multiple returns: `public async Task<(long Total, IEnumerable<TEntity> Data)> GetFilterPaging(...)`

**Virtual methods for extensibility:**
```csharp
protected virtual bool ValidateCustom(TEntity entity)
{
    return true;
}

protected virtual async Task<bool> ValidateBeforeDelete(Guid entityId)
{
    return true;
}
```

## Module Design

**Exports:**
- Public classes with explicit interface implementations for services
- Interfaces define contracts: `IBaseService<TEntity>`, `IDepartmentSerice`

**Dependency Injection:**
- Constructor injection pattern:
```csharp
public DepartmentService(
    IBaseRepository<Department> baseRepository,
    IDepartmentRepository departmentRepository
) : base(baseRepository)
{
    _departmentRepository = departmentRepository;
}
```

**Barrel files:**
- Not detected (C# doesn't typically use barrel files the same way as TypeScript)

---

*Convention analysis: 2026-04-15*