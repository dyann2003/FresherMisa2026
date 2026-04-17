# Technology Stack

**Analysis Date:** 2026-04-15

## Languages

**Primary:**
- C# 10.0 - Core application logic across all projects

**Secondary:**
- SQL - Database schema and queries (MySQL 8.0.45 compatible)

## Runtime

**Environment:**
- .NET 10.0 (ASP.NET Core)
- Target Framework: `net10.0`

**Package Manager:**
- NuGet (built into .NET SDK)
- No lockfile in source control (packages restored on build)

## Frameworks

**Core:**
- ASP.NET Core 10.0 - Web API framework
  - Location: `FresherMisa2026.WebAPI/`
  - Entry point: `Program.cs`

**Testing:**
- Not detected - No test projects in solution

**Build/Dev:**
- .NET SDK (implicit via csproj)
- Swagger/OpenAPI via Swashbuckle.AspNetCore 10.1.7

## Key Dependencies

**Critical:**
- Dapper 2.1.72 - Lightweight micro-ORM for database queries
  - Location: `FresherMisa2026.Infrastructure/`
  - Used in: `Repositories/BaseRepository.cs`, `Repositories/DepartmentRepository.cs`
- MySqlConnector 2.5.0 - MySQL database driver
  - Used in: `Infrastructure/Repositories/BaseRepository.cs`
  - Database: MySQL 8.0.x

**Infrastructure:**
- Microsoft.AspNetCore.OpenApi 10.0.5 - OpenAPI schema generation
- Swashbuckle.AspNetCore 10.1.7 - Swagger UI
- Microsoft.Extensions.DependencyInjection.Abstractions 8.0.2 - DI container
- Microsoft.Extensions.Configuration.Abstractions 10.0.5 - Configuration

## Project Structure Dependencies

```
FresherMisa2026.slnx
├── FresherMisa2026.WebAPI (depends on Application, Infrastructure)
├── FresherMisa2026.Application (depends on Entities)
├── FresherMisa2026.Infrastructure (depends on Application)
└── FresherMisa2026.Entities (no dependencies)
```

## Configuration

**Environment:**
- `appsettings.json` - Base configuration
  - Location: `FresherMisa2026.WebAPI/appsettings.json`
- `appsettings.development.json` - Development overrides
  - Location: `FresherMisa2026.WebAPI/appsettings.development.json`
- Connection string stored in development config:
  ```
  Server=localhost;Database=misaemployee_development;User=root;Password=
  ```

**Build:**
- Project files: `*.csproj` (SDK-style)
- No additional build configuration files detected

## Platform Requirements

**Development:**
- .NET 10.0 SDK
- MySQL Server 8.0+ (or compatible like XAMPP/WAMP)
- Visual Studio 2022 or VS Code with C# extension

**Production:**
- ASP.NET Core 10.0 Runtime
- MySQL Server 8.0+
- IIS or reverse proxy (nginx)

---

*Stack analysis: 2026-04-15*