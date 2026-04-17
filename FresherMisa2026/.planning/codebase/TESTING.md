# Testing Patterns

**Analysis Date:** 2026-04-15

## Test Framework

**Not Detected:** No test project exists in the current solution.

**Expected Framework (based on .NET ecosystem):**
- xUnit, NUnit, or MSTest would be natural choices for this ASP.NET Core application
- Moq or NSubstitute for mocking

**Configuration Files:**
- None detected (no .csproj file for tests, no test runner config)

**Run Commands (expected):**
```bash
dotnet test                      # Run all tests
dotnet test --watch              # Watch mode (if xUnit)
dotnet test --collect:"XPlat Code Coverage"  # Coverage
```

## Test File Organization

**Location:**
- No tests directory detected

**Expected structure based on codebase:**
```
FresherMisa2026.Tests/          # or FresherMisa2026.UnitTests/
├── Services/
│   └── DepartmentServiceTests.cs
├── Controllers/
│   └── DepartmentsControllerTests.cs
├── Repositories/
│   └── BaseRepositoryTests.cs
└── Fixtures/
    └── DepartmentFixture.cs
```

**Naming:**
- Expected pattern: `[ClassName]Tests.cs` or `[ClassName]Test.cs`
- Example: `DepartmentServiceTests.cs`

## Test Structure

**Expected patterns based on service architecture:**

**Unit Test Structure (example):**
```csharp
public class DepartmentServiceTests
{
    private readonly IDepartmentRepository _mockRepository;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        // Setup mock repository
    }

    [Fact]
    public async Task GetEntities_ReturnsEntities()
    {
        // Arrange
        var expected = new List<Department>();
        
        // Act
        var result = await _service.GetEntities();
        
        // Assert
        Assert.Equal(expected, result);
    }
}
```

**Integration Test Structure (example):**
```csharp
public class DepartmentsControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DepartmentsControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/departments");
        response.EnsureSuccessStatusCode();
    }
}
```

## Mocking

**Framework:** Not detected (no Moq, NSubstitute, or FakeItEasy package references)

**Expected patterns:**
- Use `Moq` for interface mocking
- Mock repository interfaces: `Mock<IBaseRepository<Department>>`
- Mock service interfaces: `Mock<IDepartmentSerice>`

**Example expected pattern:**
```csharp
private readonly Mock<IBaseRepository<Department>> _mockRepository;
private readonly DepartmentService _service;

public DepartmentServiceTests()
{
    _mockRepository = new Mock<IBaseRepository<Department>>();
    _service = new DepartmentService(_mockRepository.Object, _mockDepartmentRepository.Object);
}
```

**What to Mock:**
- Repository interfaces (data access)
- External services
- Configuration objects
- Database connections

**What NOT to Mock:**
- Entity classes (Department, BaseModel)
- ServiceResponse, PagingResponse
- Domain logic (should be tested, not mocked)

## Fixtures and Factories

**Test Data:**
- No fixtures detected

**Expected patterns:**
- Inline creation: `new Department { DepartmentCode = "D001", DepartmentName = "IT" }`
- Factory methods: `DepartmentFactory.CreateValid()`
- Test constants class

**Example expected:**
```csharp
public static class DepartmentFixture
{
    public static Department CreateValid()
    {
        return new Department
        {
            DepartmentID = Guid.NewGuid(),
            DepartmentCode = "D001",
            DepartmentName = "Phòng IT"
        };
    }

    public static List<Department> CreateList()
    {
        return new List<Department>
        {
            CreateValid(),
            new Department { DepartmentCode = "D002", DepartmentName = "Phòng Kinh doanh" }
        };
    }
}
```

**Location:**
- Expected: `Tests/Fixtures/` or `Tests/Factories/`

## Coverage

**Requirements:** None enforced

**View Coverage:**
```bash
dotnet test --collect:"XPlat Code Coverage"
# or
dotnet test /p:CollectCoverage=true
```

**Expected minimum for this codebase:**
- Service layer: 70%+
- Repository layer: 50%+ (integration tests)
- Controller layer: 60%+

## Test Types

**Unit Tests:**
- Scope: Individual service methods, validation logic, business rules
- Example: `ValidateRequired`, `ValidateCustom`, `GetDepartmentByCodeAsync`
- Location: `FresherMisa2026.Application.Tests/`

**Integration Tests:**
- Scope: Controller endpoints, repository with actual DB, middleware pipeline
- Example: `GET /api/departments`, `POST /api/departments`
- Location: `FresherMisa2026.IntegrationTests/`

**E2E Tests:**
- Framework: Not used (no Playwright, Selenium, or similar detected)
- Would need: Full API flow testing with test database

## Common Patterns

**Async Testing:**
```csharp
[Fact]
public async Task GetDepartmentByCodeAsync_ValidCode_ReturnsDepartment()
{
    // Arrange
    var code = "D001";
    _mockRepo.Setup(r => r.GetDepartmentByCode(code))
        .ReturnsAsync(new Department { DepartmentCode = code });

    // Act
    var result = await _service.GetDepartmentByCodeAsync(code);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(code, result.DepartmentCode);
}
```

**Error Testing:**
```csharp
[Fact]
public async Task GetDepartmentByCodeAsync_NullDepartment_ThrowsException()
{
    // Arrange
    var code = "INVALID";
    _mockRepo.Setup(r => r.GetDepartmentByCode(code))
        .ReturnsAsync((Department)null!);

    // Act & Assert
    await Assert.ThrowsAsync<Exception>(() => 
        _service.GetDepartmentByCodeAsync(code));
}
```

**Validation Testing:**
```csharp
[Fact]
public void Validate_RequiredFieldMissing_ReturnsFalse()
{
    // Arrange
    var department = new Department { DepartmentCode = "" };

    // Act
    var result = _service.Validate(department);

    // Assert
    Assert.False(result.IsSuccess);
}
```

---

*Testing analysis: 2026-04-15*