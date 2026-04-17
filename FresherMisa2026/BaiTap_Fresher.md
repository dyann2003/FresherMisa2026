# Bài Tập Fresher - Dự Án FresherMisa2026

**Mục tiêu:** Review và phân hóa trình độ fresher thông qua các bài tập thực hành trên codebase có sẵn.

**Thời gian:** 3 ngày làm việc

---

## Giới Thiệu Dự Án

Dự án `FresherMisa2026` là WebAPI ASP.NET Core với kiến trúc **Clean Architecture 4 layers**:

```
FresherMisa2026/
├── FresherMisa2026.WebAPI/        # Presentation Layer - API endpoints
├── FresherMisa2026.Application/   # Business Logic Layer - Services
├── FresherMisa2026.Infrastructure/# Data Access Layer - Repositories  
└── FresherMisa2026.Entities/       # Domain Layer - Models
```

### Đặc Điểm Codebase

- **Generic Base Classes:** `BaseController<T>`, `BaseService<T>`, `BaseRepository<T>`
- **Database:** MySQL + Dapper (Stored Procedure)
- **Pattern:** Repository Pattern với Dependency Injection

### Thực Thể Đã Có

| Entity | Controller | Service | Repository | Trạng Thái |
|--------|-----------|---------|-----------|-----------|
| Department | ✅ | ✅ | ✅ | Hoàn chỉnh |
| Employee | ❌ | ❌ | ❌ | Chỉ có Entity |
| Position | ❌ | ❌ | ❌ | Chỉ có Entity |

---

## Cấu Trúc Bài Tập

Bài tập được chia thành **3 cấp độ**, từ cơ bản đến nâng cao. Mỗi cấp độ đánh giá các kỹ năng khác nhau:

### Cấp Độ 1: Beginner (1 điểm)
> **Mục tiêu:** Đánh giá khả năng đọc hiểu code và làm theo pattern có sẵn.

### Cấp Độ 2: Intermediate (2 điểm)
> **Mục tiêu:** Đánh giá khả năng mở rộng tính năng và xử lý validation.

### Cấp Độ 3: Advanced (3 điểm)
> **Mục tiêu:** Đánh giá khả năng tối ưu hóa, refactor và giải quyết vấn đề.

---

## Bài Tập Cấp Độ 1: Beginner

### Task 1.1: Hoàn Thiện CRUD cho Position (1 điểm)

**Yêu cầu:**
- Tạo `PositionRepository` kế thừa `BaseRepository<Position>`
- Tạo `PositionService` kế thừa `BaseService<Position>`
- Tạo `PositionsController` kế thừa `BaseController<Position>`
- Đăng ký DI trong `ServiceExtensions.cs`

**Kiểm tra:**
- [ ] API `GET /api/Positions` trả về danh sách position
- [ ] API `GET /api/Positions/{id}` trả về position theo id
- [ ] API `POST /api/Positions` thêm mới position
- [ ] API `PUT /api/Positions/{id}` cập nhật position
- [ ] API `DELETE /api/Positions/{id}` xóa position

**Tham khảo:** Xem cách implement Department để làm theo pattern tương tự.

---

### Task 1.2: Hoàn Thiện CRUD cho Employee (1 điểm)

**Yêu cầu:**
- Tạo `EmployeeRepository` kế thừa `BaseRepository<Employee>`
- Tạo `EmployeeService` kế thừa `BaseService<Employee>`
- Tạo `EmployeesController` kế thừa `BaseController<Employee>`
- Đăng ký DI trong `ServiceExtensions.cs`

**Kiểm tra:**
- [ ] API `GET /api/Employees` trả về danh sách employee
- [ ] API `GET /api/Employees/{id}` trả về employee theo id
- [ ] API `POST /api/Employees` thêm mới employee
- [ ] API `PUT /api/Employees/{id}` cập nhật employee
- [ ] API `DELETE /api/Employees/{id}` xóa employee

---

## Bài Tập Cấp Độ 2: Intermediate

### Task 2.1: Thêm Validation cho Employee (2 điểm)

**Yêu cầu:**
- Thêm `[IRequired]` attribute cho các trường bắt buộc:
  - `EmployeeCode` - Mã nhân viên
  - `EmployeeName` - Tên nhân viên
  - `DepartmentID` - Phòng ban
  - `PositionID` - Vị trí
- Override method `ValidateCustom()` trong `EmployeeService` để thêm các validation tùy chỉnh:
  - Mã nhân viên không được trùng lặp
  - Email phải đúng định dạng (nếu có)
  - Số điện thoại phải đúng định dạng (nếu có)
  - Ngày sinh phải nhỏ hơn ngày hiện tại

**Kiểm tra:**
- [ ] POST employee thiếu các trường bắt buộc → trả về lỗi 400 với thông báo rõ ràng
- [ ] POST employee có mã trùng lặp → trả về lỗi "Mã nhân viên đã tồn tại"
- [ ] POST employee có email sai định dạng → trả về lỗi "Email không đúng định dạng"

---

### Task 2.2: Tạo API Lọc Employee Theo Điều Kiện (2 điểm)

**Yêu cầu:**
- Thêm endpoint `GET /api/Employees/filter` với các tham số:
  - `departmentId` - Lọc theo phòng ban
  - `positionId` - Lọc theo vị trí
  - `salaryFrom` - Lọc lương từ
  - `salaryTo` - Lọc lương đến
  - `gender` - Lọc theo giới tính (0: Nam, 1: Nữ, 2: Khác)
  - `hireDateFrom` - Ngày vào làm từ
  - `hireDateTo` - Ngày vào làm đến

**Kiểm tra:**
- [ ] Gọi API với `departmentId` hợp lệ → trả về danh sách nhân viên trong phòng ban đó
- [ ] Gọi API với nhiều điều kiện lọc → trả về kết quả đúng

---

### Task 2.3: Thêm Custom Endpoint Cho Department (2 điểm)

**Yêu cầu:**
- Thêm endpoint `GET /api/Departments/{code}/employees` - Lấy danh sách nhân viên theo mã phòng ban
- Thêm endpoint `GET /api/Departments/{code}/employee-count` - Đếm số nhân viên trong phòng ban

**Kiểm tra:**
- [ ] `GET /api/Departments/DV/employees` → trả về danh sách nhân viên phòng ban DV
- [ ] `GET /api/Departments/DV/employee-count` → trả về số lượng nhân viên

---

## Bài Tập Cấp Độ 3: Advanced

### Task 3.1: Refactor BaseRepository - Tối Ưu Performance (3 điểm)

**Vấn đề:** BaseRepository hiện tại mở connection mỗi lần gọi method, có thể gây performance issues.

**Yêu cầu:**
- Refactor để sử dụng connection pooling hiệu quả hơn
- Thêm caching cho các truy vấn `GetEntities`, `GetEntityByID` với thời gian cache 5 phút
- Đánh giá: Sử dụng `IMemoryCache` hoặc `IDistributedCache`

**Kiểm tra:**
- [ ] Review code để đảm bảo không có breaking changes
- [ ] Test performance trước và sau khi refactor
- [ ] Đảm bảo cache được clear khi có thay đổi dữ liệu

---

### Task 3.2: Xử Lý Race Condition trong Validate (3 điểm)

**Vấn đề:** Khi nhiều request thêm mới cùng lúc, validation mã trùng có thể bị race condition.

**Yêu cầu:**
- Sử dụng database-level constraint (unique index) cho `EmployeeCode`
- Xử lý exception khi duplicate xảy ra và trả về message phù hợp
- Đảm bảo transaction được handle đúng cách

**Kiểm tra:**
- [ ] Test gọi API POST 2 request cùng lúc với cùng EmployeeCode
- [ ] Chỉ 1 request thành công, 1 request trả về lỗi "Mã nhân viên đã tồn tại"

---

### Task 3.3: Thêm Paging Cho Employee Filter (3 điểm)

**Vấn đề:** Task 2.2 trả về tất cả kết quả, không có phân trang.

**Yêu cầu:**
- Refactor endpoint filter để hỗ trợ paging
- Thêm parameters: `pageSize`, `pageIndex`
- Response trả về `PagingResponse<Employee>`

**Kiểm tra:**
- [ ] `GET /api/Employees/filter?departmentId=xxx&pageSize=10&pageIndex=1` → trả về 10 bản ghi đầu tiên
- [ ] Response chứa thông tin `Total`, `PageSize`, `PageIndex`, `Data`

---

### Task 3.4: Tối Ưu SQL Query với Index (3 điểm)

**Yêu cầu:**
- Phân tích các truy vấn trong codebase
- Đề xuất và tạo các index cần thiết cho bảng Employee:
  - Index trên `DepartmentID` (cho filter theo phòng ban)
  - Index trên `PositionID` (cho filter theo vị trí)
  - Index trên `EmployeeCode` (cho tìm kiếm theo mã)
  - Composite index cho các truy vấn thường dùng

**Deliverable:**
- File SQL tạo index
- Giải thích tại sao cần các index này

---

## Tiêu Chí Chấm Điểm

### Cấp Độ 1: Beginner
| Tiêu chí | Điểm tối đa | Mô tả |
|----------|-------------|-------|
| Code chạy được | 0.5 | API hoạt động đúng chức năng cơ bản |
| Đúng pattern | 0.5 | Tuân thủ architecture và convention của project |

### Cấp Độ 2: Intermediate
| Tiêu chí | Điểm tối đa | Mô tả |
|----------|-------------|-------|
| Validation đầy đủ | 0.75 | Xử lý hết các trường hợp validation |
| Error handling tốt | 0.5 | Trả về response chính xác cho từng loại lỗi |
| Performance chấp nhận được | 0.75 | Không có N+1 query, không truy vấn thừa |

### Cấp Độ 3: Advanced
| Tiêu chí | Điểm tối đa | Mô tả |
|----------|-------------|-------|
| Giải thích rõ ràng | 1.0 | Nêu rõ lý do và cách tiếp cận |
| Không có bugs | 1.0 | Code không có logic errors |
| Tối ưu thực sự | 1.0 | Cải thiện được performance thực tế |

---

## Hướng Dẫn Nộp Bài

1. **Tạo branch mới:** `feature/fresher-task-{student-name}`
2. **Commit theo từng task:**
   - `feat(task-1.1): implement position CRUD`
   - `feat(task-1.2): implement employee CRUD`
   - ...
3. **Tạo Pull Request** và assign cho reviewer

---

## Tài Liệu Tham Khảo

- [ARCHITECTURE.md](./.planning/codebase/ARCHITECTURE.md) - Mô tả kiến trúc
- [STRUCTURE.md](./.planning/codebase/STRUCTURE.md) - Cấu trúc thư mục
- [CONVENTIONS.md](./.planning/codebase/CONVENTIONS.md) - Quy ước đặt tên

---

## Câu Hỏi Ôn Tập (Dành Cho Interviewer)

### Cấp Độ 1
- Repository Pattern là gì? Tại sao sử dụng?
- DI trong .NET Core hoạt động như thế nào?
- Sự khác biệt giữa `IBaseRepository` và `BaseRepository`?

### Cấp Độ 2
- Validation ở tầng nào là tốt nhất? Tại sao?
- Sự khác biệt giữa `[IRequired]` và DataAnnotations?
- Tại sao cần validate ở cả Client và Server?

### Cấp Độ 3
- Caching strategy nào phù hợp cho API?
- Race condition xảy ra khi nào và cách xử lý?
- Index trong SQL hoạt động như thế nào?

---

**Chúc các bạn hoàn thành bài tập tốt!**