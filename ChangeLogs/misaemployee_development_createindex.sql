
CREATE INDEX idx_employee_department 
ON employee(DepartmentID);

CREATE INDEX idx_employee_position 
ON employee(PositionID);

CREATE UNIQUE INDEX idx_employee_code 
ON employee(EmployeeCode);