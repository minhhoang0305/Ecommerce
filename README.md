🚀 Ecommerce Backend System (.NET 10)

Backend cho hệ thống thương mại điện tử, xây dựng trên .NET 10, áp dụng Clean Architecture + các best practices hiện đại nhằm đảm bảo:

Dễ mở rộng (scalable)
Dễ bảo trì (maintainable)
Hiệu năng ổn định (high performance)
🧱 Tech Stack
Thành phần	Công nghệ
Runtime	.NET 10
Architecture	Clean Architecture
Pattern	CQRS + MediatR
Database	SQL Server + EF Core
Authentication	JWT Bearer
Payment	VnPay
Validation	FluentValidation
API Docs	Swagger / OpenAPI
🏗️ Kiến trúc hệ thống

Dự án được chia thành 4 layer chính:

1. Domain
Chứa Entities (Product, Order, Cart, User...)
Enums
Business rules (logic cốt lõi)
2. Application
DTOs
Interfaces
Command / Query (CQRS)
Handlers (MediatR)
3. Infrastructure
Database (EF Core, SQL Server)
Repositories
Migrations
Tích hợp dịch vụ ngoài (VnPay...)
4. API (Affiliate.Api)
Controllers / Endpoints
Middleware
Dependency Injection
Config hệ thống
⚙️ Tính năng chính
🛍️ Product
CRUD sản phẩm
Phân trang danh sách
🛒 Cart
Thêm / sửa / xoá sản phẩm
Tính tổng tiền
🎟️ Coupon
Áp dụng mã giảm giá:
% hoặc số tiền cố định
📦 Order
Checkout
Quản lý trạng thái:
CREATED
PAID
COMPLETED
💳 Payment
Tích hợp thanh toán qua VnPay
⭐ Review
Người dùng đánh giá sản phẩm
🔐 Authentication
Đăng ký / đăng nhập
Bảo mật bằng JWT
🎖️ Loyalty System
Xếp hạng user theo lịch sử mua hàng
⚡ Setup & Run
1. Yêu cầu
.NET 10 SDK
SQL Server
2. Clone project
git clone <your-repo-url>
cd <your-project>
3. Cấu hình Database

Mở file:

appsettings.json

Sửa connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=EcommerceDb;Trusted_Connection=True;"
}
4. Migration Database
dotnet ef database update
5. Run project
dotnet run
6. Swagger

Sau khi chạy:

https://localhost:<port>/swagger
📌 Best Practices áp dụng
Clean Architecture (tách biệt rõ layer)
CQRS (tách read/write)
Dependency Injection
Validation riêng bằng FluentValidation
Tách business logic khỏi controller
Dễ test (unit test friendly)
