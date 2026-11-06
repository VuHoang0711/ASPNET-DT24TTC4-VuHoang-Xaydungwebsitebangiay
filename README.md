# 👟 SNEAKER STORE – Website Bán Giày Thể Thao Chính Hãng (ASP.NET Core 8 MVC)

Hướng dẫn cài đặt và chạy dự án **Sneaker Store** 

---

## 1. Yêu cầu cài đặt
- **Visual Studio 2022** 
- **Microsoft SQL Server** 

---

## 2. Các bước cài đặt nhanh (3 bước)

### Bước 1: Khởi tạo Cơ sở dữ liệu bằng File Script SQL
1. Mở **SQL Server Management Studio (SSMS)** và kết nối vào SQL Server của máy bạn.
2. Import file scriptVuHoang.sql
3. Nhấn **Execute (`F5`)** để chạy 

---

### Bước 2: Cấu hình chuỗi kết nối (`appsettings.json`)
1. Mở file [`appsettings.json`]
2. Tại dòng `ConnectionStrings`, sửa tên `server=...` thành tên SQL Server trên máy của bạn:

```json
"ConnectionStrings": {
  "CakeShop": "server=TÊN_SQL_SERVER_CỦA_BẠN;database=VuHoang;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Bước 3: Chạy dự án
  Nhấn **F5** hoặc **Ctrl + F5** (nút `https`) để chạy trình duyệt.

---

## 3. Thông tin đăng nhập trang Quản trị (Admin)
- **Đường dẫn Admin**: `/Admin/Login`
- **Tài khoản Admin mặc định**:
  - **Username (`MaNV`)**: `lvc`
  - **Password**: nhập bất kỳ (ví dụ: `123456`)

