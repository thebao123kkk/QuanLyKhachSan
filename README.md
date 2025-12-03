# 🏨 Hotel Management System -- Hệ thống Quản lý Khách sạn

> **Phiên bản: Phase 1 -- Desktop Application (.NET/WPF + SQL Server)**\
> Dựa trên tài liệu SRS: **Quản lý Đặt phòng -- Lễ tân -- Trạng thái
> phòng -- Thanh toán -- Báo cáo -- Người dùng**

## 1. Giới thiệu tổng quan

Hệ thống Quản lý Khách sạn được xây dựng nhằm hỗ trợ vận hành khách sạn
ở giai đoạn đầu, tập trung vào:
- Quản lý đặt phòng (Booking)
- Check-in/Check-out
- Trạng thái phòng
- Dịch vụ, minibar
- Thanh toán, hóa đơn VAT
- Báo cáo doanh thu
- Phân quyền người dùng và audit log

## 2. Đối tượng sử dụng

  Vai trò           Quyền hạn
  ----------------- --------------------------------
  **Admin**         Toàn quyền cấu hình & quản trị
  **Lễ tân**        Booking, check-in/out, dịch vụ
  **Buồng phòng**   Cập nhật trạng thái phòng

## 3. Phạm vi

**Trong phạm vi:** Booking, check-in/out, dịch vụ, thanh toán nhiều
phần, báo cáo Excel, phân quyền.
**Ngoài phạm vi:** OTA, POS, mobile app, 2FA.

## 4. Kiến trúc

-   WPF (.NET) --- GUI/BLL/DAL
-   SQL Server --- SP, Trigger, Audit
-   Email service --- gửi xác nhận booking

## 5. Module chính

### 5.1 Booking

-   Booking mẹ/con
-   Giữ phòng --- tự hủy khi hết hạn
-   Đặt cọc nhiều lần

### 5.2 Lễ tân

-   Check-in/out
-   Đổi phòng, gia hạn

### 5.3 Trạng thái phòng

-   Trống / Đang ở / Bẩn / Sạch / Bảo trì

### 5.4 Thanh toán & hóa đơn

-   Bảng giá ngày thường, cuối tuần, lễ
-   Thanh toán nhiều phần
-   In tạm tính, VAT

### 5.5 Báo cáo

-   Doanh thu
-   Công suất phòng
-   Xuất Excel

### 5.6 Người dùng

-   Admin -- Lễ tân -- Buồng phòng
-   Khóa TK sau 5 lần sai
-   Audit log

## 6. Cài đặt (placeholder)

    git clone <repo>
    Chạy script database
    Mở solution và cấu hình chuỗi kết nối

## 7. Liên kết quan trọng (điền sau)

-   Demo
-   SRS
-   ERD
-   Database scripts

## 8. Tác giả (điền sau)

| Họ tên \| MSSV \| Vai trò \| Email \|
