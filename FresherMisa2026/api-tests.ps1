# =====================================================
# API Test Scripts - FresherMisa2026 (PowerShell)
# Run: .\api-tests.ps1
# Base URL: http://localhost:5000
# =====================================================

$BASE_URL = "http://localhost:5000/api"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "DEPARTMENT API TESTS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. Get all departments
Write-Host "`n--- 1. Get all departments ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments" -Method Get | ConvertTo-Json

# 2. Get department by ID
Write-Host "`n--- 2. Get department by ID ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/00000000-0000-0000-0000-000000000000" -Method Get | ConvertTo-Json

# 3. Get departments with paging
Write-Host "`n--- 3. Get departments with paging ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/Paging?pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 4. Search departments
Write-Host "`n--- 4. Search departments ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/Paging?search=phong&pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 5. Sort departments
Write-Host "`n--- 5. Sort departments ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/Paging?sort=-DepartmentName&pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 6. Get department by code
Write-Host "`n--- 6. Get department by code ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/Code/PH001" -Method Get | ConvertTo-Json

# 7. Create new department
Write-Host "`n--- 7. Create new department ---" -ForegroundColor Yellow
$body = @{
    departmentID = "11111111-1111-1111-1111-111111111111"
    departmentCode = "TEST001"
    departmentName = "Phong Test"
    description = "Mo ta test"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/departments" -Method Post -Body $body -ContentType "application/json" | ConvertTo-Json

# 8. Update department
Write-Host "`n--- 8. Update department ---" -ForegroundColor Yellow
$body = @{
    departmentID = "11111111-1111-1111-1111-111111111111"
    departmentCode = "TEST001"
    departmentName = "Phong Test Updated"
    description = "Mo ta updated"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/departments/11111111-1111-1111-1111-111111111111" -Method Put -Body $body -ContentType "application/json" | ConvertTo-Json

# 9. Delete department
Write-Host "`n--- 9. Delete department ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/departments/11111111-1111-1111-1111-111111111111" -Method Delete | ConvertTo-Json

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "POSITION API TESTS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 10. Get all positions
Write-Host "`n--- 10. Get all positions ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions" -Method Get | ConvertTo-Json

# 11. Get position by ID
Write-Host "`n--- 11. Get position by ID ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions/00000000-0000-0000-0000-000000000000" -Method Get | ConvertTo-Json

# 12. Get positions with paging
Write-Host "`n--- 12. Get positions with paging ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions/Paging?pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 13. Search positions
Write-Host "`n--- 13. Search positions ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions/Paging?search=dev&pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 14. Get position by code
Write-Host "`n--- 14. Get position by code ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions/Code/DEV001" -Method Get | ConvertTo-Json

# 15. Create new position
Write-Host "`n--- 15. Create new position ---" -ForegroundColor Yellow
$body = @{
    positionID = "22222222-2222-2222-2222-222222222222"
    positionCode = "TEST_POS001"
    positionName = "Vi tri Test"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/positions" -Method Post -Body $body -ContentType "application/json" | ConvertTo-Json

# 16. Update position
Write-Host "`n--- 16. Update position ---" -ForegroundColor Yellow
$body = @{
    positionID = "22222222-2222-2222-2222-222222222222"
    positionCode = "TEST_POS001"
    positionName = "Vi tri Test Updated"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/positions/22222222-2222-2222-2222-222222222222" -Method Put -Body $body -ContentType "application/json" | ConvertTo-Json

# 17. Delete position
Write-Host "`n--- 17. Delete position ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/positions/22222222-2222-2222-2222-222222222222" -Method Delete | ConvertTo-Json

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "EMPLOYEE API TESTS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 18. Get all employees
Write-Host "`n--- 18. Get all employees ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees" -Method Get | ConvertTo-Json

# 19. Get employee by ID
Write-Host "`n--- 19. Get employee by ID ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/00000000-0000-0000-0000-000000000000" -Method Get | ConvertTo-Json

# 20. Get employees with paging
Write-Host "`n--- 20. Get employees with paging ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Paging?pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 21. Search employees
Write-Host "`n--- 21. Search employees ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Paging?search=nguyen&pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 22. Sort employees
Write-Host "`n--- 22. Sort employees ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Paging?sort=-EmployeeName&pageSize=10&pageIndex=1" -Method Get | ConvertTo-Json

# 23. Get employee by code
Write-Host "`n--- 23. Get employee by code ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Code/NV001" -Method Get | ConvertTo-Json

# 24. Get employees by department ID
Write-Host "`n--- 24. Get employees by department ID ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Department/00000000-0000-0000-0000-000000000000" -Method Get | ConvertTo-Json

# 25. Get employees by position ID
Write-Host "`n--- 25. Get employees by position ID ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/Position/00000000-0000-0000-0000-000000000000" -Method Get | ConvertTo-Json

# 26. Create new employee
Write-Host "`n--- 26. Create new employee ---" -ForegroundColor Yellow
$body = @{
    employeeID = "33333333-3333-3333-3333-333333333333"
    employeeCode = "TEST_NV001"
    employeeName = "Nguyen Van Test"
    gender = 1
    dateOfBirth = "1995-01-01"
    phoneNumber = "0123456789"
    email = "test@example.com"
    address = "Ha Noi"
    departmentID = "11111111-1111-1111-1111-111111111111"
    positionID = "22222222-2222-2222-2222-222222222222"
    salary = 10000000
    createdDate = "2026-04-15T00:00:00"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/employees" -Method Post -Body $body -ContentType "application/json" | ConvertTo-Json

# 27. Update employee
Write-Host "`n--- 27. Update employee ---" -ForegroundColor Yellow
$body = @{
    employeeID = "33333333-3333-3333-3333-333333333333"
    employeeCode = "TEST_NV001"
    employeeName = "Nguyen Van Test Updated"
    gender = 1
    dateOfBirth = "1995-01-01"
    phoneNumber = "0987654321"
    email = "testupdated@example.com"
    address = "TP Ho Chi Minh"
    departmentID = "11111111-1111-1111-1111-111111111111"
    positionID = "22222222-2222-2222-2222-222222222222"
    salary = 15000000
    createdDate = "2026-04-15T00:00:00"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$BASE_URL/employees/33333333-3333-3333-3333-333333333333" -Method Put -Body $body -ContentType "application/json" | ConvertTo-Json

# 28. Delete employee
Write-Host "`n--- 28. Delete employee ---" -ForegroundColor Yellow
Invoke-RestMethod -Uri "$BASE_URL/employees/33333333-3333-3333-3333-333333333333" -Method Delete | ConvertTo-Json

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "ALL TESTS COMPLETED" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan