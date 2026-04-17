@echo off
REM =====================================================
REM API Test - FresherMisa2026 (Windows Batch)
REM Run directly in CMD or PowerShell
REM Base URL: http://localhost:5000
REM =====================================================

set BASE_URL=http://localhost:5000/api

echo ========================================
echo DEPARTMENT API TESTS
echo ========================================

echo.
echo --- 1. Get all departments ---
curl -s -X GET "%BASE_URL%/departments"

echo.
echo --- 2. Get department by ID ---
curl -s -X GET "%BASE_URL%/departments/00000000-0000-0000-0000-000000000000"

echo.
echo --- 3. Get departments with paging ---
curl -s -X GET "%BASE_URL%/departments/Paging?pageSize=10^&pageIndex=1"

echo.
echo --- 4. Search departments ---
curl -s -X GET "%BASE_URL%/departments/Paging?search=phong^&pageSize=10^&pageIndex=1"

echo.
echo --- 5. Sort departments ---
curl -s -X GET "%BASE_URL%/departments/Paging?sort=-DepartmentName^&pageSize=10^&pageIndex=1"

echo.
echo --- 6. Get department by code ---
curl -s -X GET "%BASE_URL%/departments/Code/PH001"

echo.
echo --- 7. Create new department ---
curl -s -X POST "%BASE_URL%/departments" -H "Content-Type: application/json" -d "{\"departmentID\":\"11111111-1111-1111-1111-111111111111\",\"departmentCode\":\"TEST001\",\"departmentName\":\"Phong Test\",\"description\":\"Mo ta test\"}"

echo.
echo --- 8. Update department ---
curl -s -X PUT "%BASE_URL%/departments/11111111-1111-1111-1111-111111111111" -H "Content-Type: application/json" -d "{\"departmentID\":\"11111111-1111-1111-1111-111111111111\",\"departmentCode\":\"TEST001\",\"departmentName\":\"Phong Test Updated\",\"description\":\"Mo ta updated\"}"

echo.
echo --- 9. Delete department ---
curl -s -X DELETE "%BASE_URL%/departments/11111111-1111-1111-1111-111111111111"

echo.
echo ========================================
echo POSITION API TESTS
echo ========================================

echo.
echo --- 10. Get all positions ---
curl -s -X GET "%BASE_URL%/positions"

echo.
echo --- 11. Get position by ID ---
curl -s -X GET "%BASE_URL%/positions/00000000-0000-0000-0000-000000000000"

echo.
echo --- 12. Get positions with paging ---
curl -s -X GET "%BASE_URL%/positions/Paging?pageSize=10^&pageIndex=1"

echo.
echo --- 13. Search positions ---
curl -s -X GET "%BASE_URL%/positions/Paging?search=dev^&pageSize=10^&pageIndex=1"

echo.
echo --- 14. Get position by code ---
curl -s -X GET "%BASE_URL%/positions/Code/DEV001"

echo.
echo --- 15. Create new position ---
curl -s -X POST "%BASE_URL%/positions" -H "Content-Type: application/json" -d "{\"positionID\":\"22222222-2222-2222-2222-222222222222\",\"positionCode\":\"TEST_POS001\",\"positionName\":\"Vi tri Test\"}"

echo.
echo --- 16. Update position ---
curl -s -X PUT "%BASE_URL%/positions/22222222-2222-2222-2222-222222222222" -H "Content-Type: application/json" -d "{\"positionID\":\"22222222-2222-2222-2222-222222222222\",\"positionCode\":\"TEST_POS001\",\"positionName\":\"Vi tri Test Updated\"}"

echo.
echo --- 17. Delete position ---
curl -s -X DELETE "%BASE_URL%/positions/22222222-2222-2222-2222-222222222222"

echo.
echo ========================================
echo EMPLOYEE API TESTS
echo ========================================

echo.
echo --- 18. Get all employees ---
curl -s -X GET "%BASE_URL%/employees"

echo.
echo --- 19. Get employee by ID ---
curl -s -X GET "%BASE_URL%/employees/00000000-0000-0000-0000-000000000000"

echo.
echo --- 20. Get employees with paging ---
curl -s -X GET "%BASE_URL%/employees/Paging?pageSize=10^&pageIndex=1"

echo.
echo --- 21. Search employees ---
curl -s -X GET "%BASE_URL%/employees/Paging?search=nguyen^&pageSize=10^&pageIndex=1"

echo.
echo --- 22. Sort employees ---
curl -s -X GET "%BASE_URL%/employees/Paging?sort=-EmployeeName^&pageSize=10^&pageIndex=1"

echo.
echo --- 23. Get employee by code ---
curl -s -X GET "%BASE_URL%/employees/Code/NV001"

echo.
echo --- 24. Get employees by department ID ---
curl -s -X GET "%BASE_URL%/employees/Department/00000000-0000-0000-0000-000000000000"

echo.
echo --- 25. Get employees by position ID ---
curl -s -X GET "%BASE_URL%/employees/Position/00000000-0000-0000-0000-000000000000"

echo.
echo --- 26. Create new employee ---
curl -s -X POST "%BASE_URL%/employees" -H "Content-Type: application/json" -d "{\"employeeID\":\"33333333-3333-3333-3333-333333333333\",\"employeeCode\":\"TEST_NV001\",\"employeeName\":\"Nguyen Van Test\",\"gender\":1,\"dateOfBirth\":\"1995-01-01\",\"phoneNumber\":\"0123456789\",\"email\":\"test@example.com\",\"address\":\"Ha Noi\",\"departmentID\":\"11111111-1111-1111-1111-111111111111\",\"positionID\":\"22222222-2222-2222-2222-222222222222\",\"salary\":10000000,\"createdDate\":\"2026-04-15T00:00:00\"}"

echo.
echo --- 27. Update employee ---
curl -s -X PUT "%BASE_URL%/employees/33333333-3333-3333-3333-333333333333" -H "Content-Type: application/json" -d "{\"employeeID\":\"33333333-3333-3333-3333-333333333333\",\"employeeCode\":\"TEST_NV001\",\"employeeName\":\"Nguyen Van Test Updated\",\"gender\":1,\"dateOfBirth\":\"1995-01-01\",\"phoneNumber\":\"0987654321\",\"email\":\"testupdated@example.com\",\"address\":\"TP Ho Chi Minh\",\"departmentID\":\"11111111-1111-1111-1111-111111111111\",\"positionID\":\"22222222-2222-2222-2222-222222222222\",\"salary\":15000000,\"createdDate\":\"2026-04-15T00:00:00\"}"

echo.
echo --- 28. Delete employee ---
curl -s -X DELETE "%BASE_URL%/employees/33333333-3333-3333-3333-333333333333"

echo.
echo ========================================
echo ALL TESTS COMPLETED
echo ========================================
pause