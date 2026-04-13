# 🚀 Ecommerce Backend System (.NET 10)

Backend cho hệ thống thương mại điện tử xây dựng bằng **.NET 10**, áp dụng **Clean Architecture** + best practices hiện đại.

---

## ✨ Mục tiêu

- Scalability → dễ mở rộng  
- Maintainability → dễ bảo trì  
- Performance → hiệu năng ổn định  

---

## 🧰 Tech Stack

- Runtime: .NET 10  
- Architecture: Clean Architecture  
- Pattern: CQRS + MediatR  
- Database: SQL Server + EF Core  
- Auth: JWT Bearer  
- Payment: VnPay  
- Validation: FluentValidation  
- Docs: Swagger / OpenAPI  

---

## 🏗️ Architecture Overview

    src/
    ├── Domain          → Entities, Enums, Business Rules
    ├── Application     → DTOs, Interfaces, CQRS, Handlers
    ├── Infrastructure  → EF Core, Repositories, External Services
    └── API             → Controllers, Middleware, DI, Config

---

## ⚙️ Features

### 🛍️ Product
- CRUD sản phẩm  
- Phân trang  

### 🛒 Cart
- Thêm / sửa / xoá sản phẩm  
- Tính tổng tiền  

### 🎟️ Coupon
- Giảm giá theo:
  - %
  - số tiền cố định  

### 📦 Order
- Checkout  
- Trạng thái:
  - CREATED  
  - PAID  
  - COMPLETED  

### 💳 Payment
- Thanh toán qua VnPay  

### ⭐ Review
- Đánh giá sản phẩm  

### 🔐 Authentication
- Đăng ký / đăng nhập  
- JWT bảo mật API  

### 🏆 Loyalty
- Xếp hạng user theo lịch sử mua hàng  

---

## ⚡ Setup & Run

### 1. Requirements

- .NET 10 SDK  
- SQL Server  

---

### 2. Clone project

```bash
git clone <your-repo>
cd <project-folder>
```

---

### 3. Config Database

Mở file:

```bash
appsettings.json
```

Sửa connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=EcommerceDb;Trusted_Connection=True;"
  }
}
```

---

### 4. Migration

```bash
dotnet ef database update
```

---

### 5. Run

```bash
dotnet run
```

---

### 6. Swagger

    https://localhost:<port>/swagger

---

## 🧠 Best Practices

- Clean Architecture (tách layer rõ ràng)  
- CQRS (tách read/write)  
- Dependency Injection  
- FluentValidation (validate riêng)  
- Tách business logic khỏi controller  
- Unit test friendly  
