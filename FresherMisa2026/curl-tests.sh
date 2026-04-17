# =====================================================
# API Test Collection - FresherMisa2026
# Run: copy and paste into terminal
# Base URL: http://localhost:5000
# =====================================================

BASE_URL="http://localhost:5000/api"

echo "========================================"
echo "DEPARTMENT API TESTS"
echo "========================================"

# 1. Get all departments
echo -e "\n--- 1. Get all departments ---"
curl -X GET "${BASE_URL}/departments" \
  -H "Content-Type: application/json"

# 2. Get department by ID (replace with valid UUID)
echo -e "\n--- 2. Get department by ID ---"
curl -X GET "${BASE_URL}/departments/00000000-0000-0000-0000-000000000000" \
  -H "Content-Type: application/json"

# 3. Get departments with paging
echo -e "\n--- 3. Get departments with paging ---"
curl -X GET "${BASE_URL}/departments/Paging?pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 4. Search departments
echo -e "\n--- 4. Search departments ---"
curl -X GET "${BASE_URL}/departments/Paging?search=phong&pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 5. Sort departments
echo -e "\n--- 5. Sort departments ---"
curl -X GET "${BASE_URL}/departments/Paging?sort=-DepartmentName&pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 6. Get department by code
echo -e "\n--- 6. Get department by code ---"
curl -X GET "${BASE_URL}/departments/Code/PH001" \
  -H "Content-Type: application/json"

# 7. Create new department
echo -e "\n--- 7. Create new department ---"
curl -X POST "${BASE_URL}/departments" \
  -H "Content-Type: application/json" \
  -d '{
    "departmentID": "11111111-1111-1111-1111-111111111111",
    "departmentCode": "TEST001",
    "departmentName": "Phòng Test",
    "description": "Phòng ban test"
  }'

# 8. Update department
echo -e "\n--- 8. Update department ---"
curl -X PUT "${BASE_URL}/departments/11111111-1111-1111-1111-111111111111" \
  -H "Content-Type: application/json" \
  -d '{
    "departmentID": "11111111-1111-1111-1111-111111111111",
    "departmentCode": "TEST001",
    "departmentName": "Phòng Test Updated",
    "description": "Mô tả updated"
  }'

# 9. Delete department
echo -e "\n--- 9. Delete department ---"
curl -X DELETE "${BASE_URL}/departments/11111111-1111-1111-1111-111111111111" \
  -H "Content-Type: application/json"

echo -e "\n\n========================================"
echo "POSITION API TESTS"
echo "========================================"

# 10. Get all positions
echo -e "\n--- 10. Get all positions ---"
curl -X GET "${BASE_URL}/positions" \
  -H "Content-Type: application/json"

# 11. Get position by ID
echo -e "\n--- 11. Get position by ID ---"
curl -X GET "${BASE_URL}/positions/00000000-0000-0000-0000-000000000000" \
  -H "Content-Type: application/json"

# 12. Get positions with paging
echo -e "\n--- 12. Get positions with paging ---"
curl -X GET "${BASE_URL}/positions/Paging?pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 13. Search positions
echo -e "\n--- 13. Search positions ---"
curl -X GET "${BASE_URL}/positions/Paging?search=dev&pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 14. Get position by code
echo -e "\n--- 14. Get position by code ---"
curl -X GET "${BASE_URL}/positions/Code/DEV001" \
  -H "Content-Type: application/json"

# 15. Create new position
echo -e "\n--- 15. Create new position ---"
curl -X POST "${BASE_URL}/positions" \
  -H "Content-Type: application/json" \
  -d '{
    "positionID": "22222222-2222-2222-2222-222222222222",
    "positionCode": "TEST_POS001",
    "positionName": "Vị trí Test"
  }'

# 16. Update position
echo -e "\n--- 16. Update position ---"
curl -X PUT "${BASE_URL}/positions/22222222-2222-2222-2222-222222222222" \
  -H "Content-Type: application/json" \
  -d '{
    "positionID": "22222222-2222-2222-2222-222222222222",
    "positionCode": "TEST_POS001",
    "positionName": "Vị trí Test Updated"
  }'

# 17. Delete position
echo -e "\n--- 17. Delete position ---"
curl -X DELETE "${BASE_URL}/positions/22222222-2222-2222-2222-222222222222" \
  -H "Content-Type: application/json"

echo -e "\n\n========================================"
echo "EMPLOYEE API TESTS"
echo "========================================"

# 18. Get all employees
echo -e "\n--- 18. Get all employees ---"
curl -X GET "${BASE_URL}/employees" \
  -H "Content-Type: application/json"

# 19. Get employee by ID
echo -e "\n--- 19. Get employee by ID ---"
curl -X GET "${BASE_URL}/employees/00000000-0000-0000-0000-000000000000" \
  -H "Content-Type: application/json"

# 20. Get employees with paging
echo -e "\n--- 20. Get employees with paging ---"
curl -X GET "${BASE_URL}/employees/Paging?pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 21. Search employees
echo -e "\n--- 21. Search employees ---"
curl -X GET "${BASE_URL}/employees/Paging?search=nguyen&pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 22. Sort employees
echo -e "\n--- 22. Sort employees ---"
curl -X GET "${BASE_URL}/employees/Paging?sort=-EmployeeName&pageSize=10&pageIndex=1" \
  -H "Content-Type: application/json"

# 23. Get employee by code
echo -e "\n--- 23. Get employee by code ---"
curl -X GET "${BASE_URL}/employees/Code/NV001" \
  -H "Content-Type: application/json"

# 24. Get employees by department ID
echo -e "\n--- 24. Get employees by department ID ---"
curl -X GET "${BASE_URL}/employees/Department/00000000-0000-0000-0000-000000000000" \
  -H "Content-Type: application/json"

# 25. Get employees by position ID
echo -e "\n--- 25. Get employees by position ID ---"
curl -X GET "${BASE_URL}/employees/Position/00000000-0000-0000-0000-000000000000" \
  -H "Content-Type: application/json"

# 26. Create new employee
echo -e "\n--- 26. Create new employee ---"
curl -X POST "${BASE_URL}/employees" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeID": "33333333-3333-3333-3333-333333333333",
    "employeeCode": "TEST_NV001",
    "employeeName": "Nguyen Van Test",
    "gender": 1,
    "dateOfBirth": "1995-01-01",
    "phoneNumber": "0123456789",
    "email": "test@example.com",
    "address": "Hà Nội",
    "departmentID": "11111111-1111-1111-1111-111111111111",
    "positionID": "22222222-2222-2222-2222-222222222222",
    "salary": 10000000,
    "createdDate": "2026-04-15T00:00:00"
  }'

# 27. Update employee
echo -e "\n--- 27. Update employee ---"
curl -X PUT "${BASE_URL}/employees/33333333-3333-3333-3333-333333333333" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeID": "33333333-3333-3333-3333-333333333333",
    "employeeCode": "TEST_NV001",
    "employeeName": "Nguyen Van Test Updated",
    "gender": 1,
    "dateOfBirth": "1995-01-01",
    "phoneNumber": "0987654321",
    "email": "testupdated@example.com",
    "address": "TP Hồ Chí Minh",
    "departmentID": "11111111-1111-1111-1111-111111111111",
    "positionID": "22222222-2222-2222-2222-222222222222",
    "salary": 15000000,
    "createdDate": "2026-04-15T00:00:00"
  }'

# 28. Delete employee
echo -e "\n--- 28. Delete employee ---"
curl -X DELETE "${BASE_URL}/employees/33333333-3333-3333-3333-333333333333" \
  -H "Content-Type: application/json"

echo -e "\n\n========================================"
echo "ALL TESTS COMPLETED"
echo "========================================"