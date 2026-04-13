Ecommerce Backend System
Dự án Backend cho hệ thống thương mại điện tử được xây dựng trên nền tảng .NET 10, áp dụng kiến trúc Clean Architecture và các mẫu thiết kế hiện đại nhằm đảm bảo tính mở rộng, bảo trì dễ dàng và hiệu năng cao.

🚀 Công nghệ sử dụng
Runtime: .NET 10

Kiến trúc: Clean Architecture (Domain, Application, Infrastructure, API)

Pattern: CQRS với MediatR

Cơ sở dữ liệu: SQL Server với Entity Framework Core

Xác thực & Phân quyền: JWT Bearer Token

Thanh toán: Tích hợp cổng thanh toán VnPay

Validation: FluentValidation

Documentation: Swagger / OpenAPI

🏗 Cấu trúc dự án (Clean Architecture)
Hệ thống được chia thành 4 lớp chính:

Domain: Chứa các thực thể (Entities), Enums và các quy tắc nghiệp vụ cốt lõi (Cart, Order, Product, User...).

Application: Chứa logic nghiệp vụ, các DTOs, Interfaces và các Handler cho Command/Query (sử dụng MediatR).

Infrastructure: Triển khai các Persistence (SQL Server), Repositories, Migrations và các dịch vụ bên thứ ba (VnPay).

Affiliate.Api: Lớp giao tiếp bên ngoài, cấu hình Dependency Injection, Middleware và các Endpoints.

✨ Các tính năng chính
Quản lý sản phẩm: CRUD sản phẩm, phân trang danh sách.

Giỏ hàng (Cart): Thêm/sửa/xóa sản phẩm, tính toán giá trị giỏ hàng.

Hệ thống Coupon: Áp dụng mã giảm giá (Phần trăm hoặc số tiền cố định) vào giỏ hàng.

Đơn hàng (Order): Quy trình Checkout, quản lý trạng thái đơn hàng.

Thanh toán: Tích hợp quy trình thanh toán qua VnPay Service.

Đánh giá (Review): Cho phép người dùng đánh giá sản phẩm.

Xác thực: Đăng ký, đăng nhập và bảo mật các đầu API bằng JWT.

Loyalty System: Hệ thống xếp hạng thành viên dựa trên lịch sử mua hàng.

🛠 Hướng dẫn cài đặt
Yêu cầu: Đã cài đặt .NET 10 SDK và SQL Server.

Clone dự án:

Bash
git clone [url_cua_ban]
Cấu hình Database:
Thay đổi chuỗi kết nối tại file appsettings.json trong dự án Affiliate.Api:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=EcommerceDb;..."
}
Cập nhật Database (Migrations):
Mở Terminal tại thư mục gốc và chạy:

Bash
dotnet ef database update --project Affiliate.Infrastructure --startup-project Affiliate.Api
Chạy ứng dụng:

Bash
dotnet run --project Affiliate.Api
📖 API Documentation
Sau khi chạy ứng dụng, bạn có thể truy cập Swagger UI để xem chi tiết các đầu API và thử nghiệm:
https://localhost:[port]/swagger/index.html
