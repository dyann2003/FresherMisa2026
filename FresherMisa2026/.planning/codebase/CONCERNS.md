# Codebase Concerns

**Analysis Date:** 2026-04-15

## Tech Debt

**Database Connection Management:**
- Issue: Connection opened but not closed in methods other than Delete/Insert/Update
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs`
- Impact: Resource leak - database connections remain open, leading to connection pool exhaustion under load
- Fix approach: Use `using` statements or explicitly close connections after each query operation

**Exception Handling - Silent Failures:**
- Issue: Empty catch blocks swallow exceptions without logging or rethrowing
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs` (lines 159, 189, 226, 294), `FresherMisa2026.Application\Extensions\SQLExtension.cs` (lines 38-40, 52-54, 66-68, 119-121)
- Impact: Errors are hidden, debugging becomes extremely difficult, data integrity issues go unnoticed
- Fix approach: Add proper logging and either throw meaningful exceptions or handle gracefully

**Validation Logic:**
- Issue: ValidateRequired method sets incorrect error messages - displays "Trùng dữ liệu" (duplicate data) when checking for empty fields
- Files: `FresherMisa2026.Application\Services\BaseService.cs` (lines 143-145)
- Impact: Users receive misleading error messages
- Fix approach: Change error message to indicate required field is empty

**Update Method Error Handling:**
- Issue: Returns success code (200) even when validation fails
- Files: `FresherMisa2026.Application\Services\BaseService.cs` (line 220)
- Impact: Client receives false success response on validation failure
- Fix approach: Set appropriate error code when validation fails

## Known Bugs

**SQL Injection Vulnerability - String Interpolation:**
- Issue: Raw SQL query built using string interpolation without parameterization
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs` (lines 75, 107-117)
- Trigger: Pass malicious values in queries where string interpolation is used
- Workaround: Use parameterized queries exclusively

**Paging Spelling Error:**
- Issue: Method named "GetFilterPaping" instead of "GetFilterPaging"
- Files: `FresherMisa2026.WebAPI\Controllers\BaseController.cs` (line 31)
- Trigger: Route `/api/[controller]/Paging` works but naming is inconsistent
- Workaround: None needed (route still works), but should rename for consistency

**Connection Not Opened:**
- Issue: GetEntities and GetEntityByID assume connection is open but never call `_dbConnection.Open()`
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs` (lines 63-131)
- Trigger: Calling GetEntities or GetEntityByID without calling another method that opens connection first
- Workaround: Ensure Delete/Insert/Update is called before these methods (flawed design)

## Security Considerations

**Hardcoded Database Credentials:**
- Risk: Database password is empty string in connection string
- Files: `FresherMisa2026.WebAPI\appsettings.development.json` (line 10)
- Current mitigation: Only in development config
- Recommendations: Use environment variables or secure secret management in production

**No Authentication/Authorization:**
- Risk: No authentication mechanism implemented; all endpoints are publicly accessible
- Files: `FresherMisa2026.WebAPI\Controllers\BaseController.cs` (line 14 - missing authorization attributes)
- Current mitigation: None
- Recommendations: Implement JWT or other auth mechanism before production

**No Input Validation:**
- Risk: No validation on query parameters (pageSize, pageIndex, search, sort)
- Files: `FresherMisa2026.WebAPI\Controllers\BaseController.cs` (lines 31-52)
- Current mitigation: None
- Recommendations: Add range validation for paging parameters, sanitize search inputs

**Sensitive Data in Error Messages:**
- Risk: Exception messages exposed to clients in production
- Files: `FresherMisa2026.WebAPI\Middlewares\GlobalExceptionMiddleware.cs` (line 44)
- Current mitigation: None visible in code, but DevMessage property exists
- Recommendations: Only show user-friendly messages in production, hide stack traces

**No SQL Injection Protection in Raw Queries:**
- Risk: String interpolation used for building dynamic queries
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs`
- Current mitigation: None
- Recommendations: Use parameterized queries for all dynamic values

## Performance Bottlenecks

**No Connection Pooling Control:**
- Problem: Connection string has no pooling configuration; default MySQL connector pooling may cause issues
- Files: `FresherMisa2026.WebAPI\appsettings.development.json` (line 10)
- Cause: Not explicitly configured
- Improvement path: Add pooling configuration to connection string

**Reflection in Hot Paths:**
- Problem: Heavy reflection usage in MappingDbType and validation
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs` (line 280), `FresherMisa2026.Application\Services\BaseService.cs` (line 98)
- Cause: Using GetProperties() and GetValue() for every entity operation
- Improvement path: Consider compiled expressions or source generators

**No Query Result Caching:**
- Problem: Every request hits database without caching layer
- Files: `FresherMisa2026.Application\Services\BaseService.cs`
- Cause: No caching implemented
- Improvement path: Add Redis or in-memory caching for frequently accessed data

**N+1 Query Potential:**
- Problem: GetFilterPaging uses stored procedure without analyzing query efficiency
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs` (line 252)
- Cause: Using QueryMultiple but potential for unoptimized queries
- Improvement path: Analyze generated SQL, add query logging

## Fragile Areas

**BaseController Generic Constraint:**
- Files: `FresherMisa2026.WebAPI\Controllers\BaseController.cs`
- Why fragile: Takes generic TEntity but controller itself is not generic - inheritance structure is confusing
- Safe modification: Consider making BaseController<T> or extracting to interface
- Test coverage: Minimal

**SQLExtension Global State:**
- Files: `FresherMisa2026.Application\Extensions\SQLExtension.cs`
- Why fragile: Uses static ConcurrentDictionary as global cache - not testable, potential race conditions in testing
- Safe modification: Consider dependency injection for query provider
- Test coverage: None

**ServiceResponse Using Object Types:**
- Files: `FresherMisa2026.Entities\ServiceResponse.cs`
- Why fragile: Data, UserMessage, DevMessage all use object type - no type safety, JSON serialization issues
- Safe modification: Use generic types or proper DTOs
- Test coverage: None

**BaseRepository Open/Close Logic:**
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs`
- Why fragile: Inconsistent connection state management - some methods open, some assume open
- Safe modification: Ensure connection lifecycle is explicit and consistent
- Test coverage: None

## Scaling Limits

**In-Memory Query Cache:**
- Current capacity: ConcurrentDictionary size bounded by memory
- Limit: No built-in cache eviction; grows indefinitely
- Scaling path: Implement cache size limits or use distributed cache

**Single Database Connection:**
- Current capacity: One MySQL connection per repository instance
- Limit: Under load, connection pool may exhaust
- Scaling path: Configure connection pooling, consider read replicas

**No Horizontal Scaling:**
- Current capacity: Stateless API but no scaling configuration
- Limit: Cannot scale horizontally without session management
- Scaling path: Add sticky sessions or distributed cache for state

## Dependencies at Risk

**MySqlConnector:**
- Risk: Using legacy MySqlConnector package directly
- Impact: May have compatibility issues with .NET 10
- Migration plan: Consider switching to Pomelo.EntityFrameworkCore.MySql or verify compatibility

**Swashbuckle:**
- Risk: Version 10.1.7 may not be stable for .NET 10
- Impact: API documentation may not work properly
- Migration plan: Monitor for updates, consider alternative API documentation

**No Logging Framework:**
- Risk: Using Console.WriteLine for logging
- Impact: No structured logging, difficult to filter/search
- Migration plan: Add Microsoft.Extensions.Logging with Serilog or similar

## Missing Critical Features

**Unit Tests:**
- Problem: No test project exists
- Blocks: Cannot verify bug fixes without introducing regressions

**Integration Tests:**
- Problem: No integration test setup
- Blocks: Cannot verify database operations work correctly

**API Versioning:**
- Problem: No API versioning configured
- Blocks: Cannot maintain backward compatibility

**Rate Limiting:**
- Problem: No rate limiting
- Blocks: Vulnerable to DoS attacks

**Health Checks:**
- Problem: No health check endpoint for container orchestration
- Blocks: Cannot monitor service health

**Structured Logging:**
- Problem: Console.WriteLine used instead of proper logging
- Blocks: Cannot search/analyze logs effectively in production

**Request/Response Logging:**
- Problem: No request/response logging middleware
- Blocks: Cannot debug issues in production

**Database Migrations:**
- Problem: Manual SQL scripts, no EF migrations
- Blocks: Version control for schema changes

## Test Coverage Gaps

**BaseRepository:**
- What's not tested: All CRUD operations, connection management, paging
- Files: `FresherMisa2026.Infrastructure\Repositories\BaseRepository.cs`
- Risk: Critical data operations have no verification
- Priority: High

**BaseService:**
- What's not tested: Validation logic, insert/update flows
- Files: `FresherMisa2026.Application\Services\BaseService.cs`
- Risk: Validation may allow invalid data
- Priority: High

**Controllers:**
- What's not tested: All endpoints, error scenarios
- Files: `FresherMisa2026.WebAPI\Controllers\BaseController.cs`
- Risk: API contracts not verified
- Priority: High

**Exception Handling:**
- What's not tested: Middleware exception handling
- Files: `FresherMisa2026.WebAPI\Middlewares\GlobalExceptionMiddleware.cs`
- Risk: Unknown error handling behavior
- Priority: Medium

**SQLExtension:**
- What's not tested: Query loading, caching, normalization
- Files: `FresherMisa2026.Application\Extensions\SQLExtension.cs`
- Risk: Query loading failures not detected
- Priority: Low

---

*Concerns audit: 2026-04-15*