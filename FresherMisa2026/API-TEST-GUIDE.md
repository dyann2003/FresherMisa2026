# API Test Scripts - FresherMisa2026

## Files Created

### 1. curl-tests.sh (Linux/Mac/Windows with Git Bash)
```bash
bash curl-tests.sh
```

### 2. api-tests.bat (Windows CMD)
```cmd
api-tests.bat
```

### 3. api-tests.ps1 (Windows PowerShell)
```powershell
.\api-tests.ps1
```

## API Endpoints Summary

### Department APIs
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/departments` | Get all departments |
| GET | `/api/departments/{id}` | Get department by ID |
| GET | `/api/departments/Paging` | Get departments with paging |
| GET | `/api/departments/Code/{code}` | Get department by code |
| POST | `/api/departments` | Create new department |
| PUT | `/api/departments/{id}` | Update department |
| DELETE | `/api/departments/{id}` | Delete department |

### Position APIs
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/positions` | Get all positions |
| GET | `/api/positions/{id}` | Get position by ID |
| GET | `/api/positions/Paging` | Get positions with paging |
| GET | `/api/positions/Code/{code}` | Get position by code |
| POST | `/api/positions` | Create new position |
| PUT | `/api/positions/{id}` | Update position |
| DELETE | `/api/positions/{id}` | Delete position |

### Employee APIs
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees/{id}` | Get employee by ID |
| GET | `/api/employees/Paging` | Get employees with paging |
| GET | `/api/employees/Code/{code}` | Get employee by code |
| GET | `/api/employees/Department/{departmentId}` | Get employees by department |
| GET | `/api/employees/Position/{positionId}` | Get employees by position |
| POST | `/api/employees` | Create new employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |

## Query Parameters for Paging

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| pageIndex | int | 1 | Page number (starts from 1) |
| pageSize | int | 10 | Number of records per page |
| search | string | "" | Search keyword |
| sort | string | "" | Sort field (prefix with - for DESC) |
| searchFields | string | "" | JSON array of fields to search |

## Examples

### Get departments with paging
```bash
curl -X GET "http://localhost:5000/api/departments/Paging?pageSize=10&pageIndex=1"
```

### Search departments
```bash
curl -X GET "http://localhost:5000/api/departments/Paging?search=phong&pageSize=10&pageIndex=1"
```

### Sort by name descending
```bash
curl -X GET "http://localhost:5000/api/departments/Paging?sort=-DepartmentName&pageSize=10&pageIndex=1"
```

### Create new department
```bash
curl -X POST "http://localhost:5000/api/departments" \
  -H "Content-Type: application/json" \
  -d '{"departmentID":"11111111-1111-1111-1111-111111111111","departmentCode":"PH001","departmentName":"Phòng Kế toán","description":"Phòng ban kế toán"}'
```