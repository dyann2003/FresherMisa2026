# External Integrations

**Analysis Date:** 2026-04-15

## APIs & External Services

**External APIs:**
- None detected - No HTTP client libraries or external API integrations

## Data Storage

**Databases:**
- MySQL 8.0+ (MySQL Server)
  - Connection: Configured in `appsettings.development.json`
    ```
    Server=localhost;Database=misaemployee_development;User=root;Password=
    ```
  - Client: MySqlConnector 2.5.0
    - Location: `FresherMisa2026.Infrastructure/Repositories/BaseRepository.cs`
  - ORM: Dapper 2.1.72 (micro-ORM, not full ORM)

**File Storage:**
- Not applicable - No file upload/storage functionality detected

**Caching:**
- None - No caching layer implemented

## Authentication & Identity

**Auth Provider:**
- Not implemented - No authentication middleware
- No JWT, OAuth, or identity providers configured

## Monitoring & Observability

**Error Tracking:**
- None - No external error tracking service (Sentry, Raygun, etc.)

**Logs:**
- ASP.NET Core built-in logging
  - Configuration: `appsettings.json` LogLevel settings
  - Default level: Information
  - Microsoft.AspNetCore: Warning

**Middleware:**
- GlobalExceptionMiddleware for error handling
  - Location: `FresherMisa2026.WebAPI/Middlewares/GlobalExceptionMiddleware.cs`

## CI/CD & Deployment

**Hosting:**
- Self-hosted via ASP.NET Core (Kestrel)
- Configuration: `Program.cs` with `app.UseHttpsRedirection()`

**CI Pipeline:**
- None detected - No CI/CD configuration files (.github/workflows, azure-pipelines.yml, etc.)

## Environment Configuration

**Required env vars:**
- None explicitly required - Configuration loaded from appsettings files

**Configuration sources:**
1. `appsettings.json` - Base configuration
2. `appsettings.{Environment}.json` - Environment-specific overrides
3. Environment variables (ASP.NET Core default)
4. Command-line args

**Database configuration:**
- Connection string managed via `ConnectionStrings:DefaultConnection` in config files

## Webhooks & Callbacks

**Incoming:**
- None detected - No webhook endpoints

**Outgoing:**
- None detected - No HTTP calls to external services

## Database Schema

**Schema files:**
- `misaemployee_development_department.sql` - Department table
- `misaemployee_development_employee.sql` - Employee table
- `misaemployee_development_position.sql` - Position table

**Database:** `misaemployee_development`

**Tables:**
- `department` - Departments (ID, Code, Name, Description)
- `employee` - Employees
- `position` - Positions

---

*Integration audit: 2026-04-15*