🏨 Hotel Management System – Hệ thống Quản lý Khách sạn

Phiên bản: Phase 1 – Desktop Application (.NET/WPF + SQL Server)
Dựa trên tài liệu SRS: Quản lý Đặt phòng – Lễ tân – Trạng thái phòng – Thanh toán – Báo cáo – Người dùng

📌 1. Giới thiệu tổng quan

Hệ thống Quản lý Khách sạn được xây dựng nhằm hỗ trợ vận hành khách sạn ở giai đoạn đầu, tập trung vào việc:

Quản lý đặt phòng (Booking)

Quản lý khách đoàn

Quy trình Check-in/Check-out

Quản lý dịch vụ, minibar, giặt ủi

Theo dõi trạng thái phòng

Thanh toán, đặt cọc, tạm tính, hóa đơn VAT

Báo cáo doanh thu, công suất phòng

Phân quyền người dùng và audit log

Hệ thống được thiết kế cho quy mô vừa và nhỏ, phù hợp mô hình Lễ tân – Buồng phòng – Quản lý, hỗ trợ vận hành nhanh, chính xác và đảm bảo kiểm soát nghiệp vụ.

📌 2. Đối tượng sử dụng
Vai trò	Quyền hạn chính
Quản lý (Admin)	Toàn quyền: cấu hình giá, quản lý người dùng, xem báo cáo, duyệt đổi phòng, thay đổi trạng thái phòng.
Lễ tân	Tạo/sửa/xóa booking, check-in/out, ghi nhận dịch vụ, in hóa đơn, quản lý khách đoàn.
Buồng phòng	Cập nhật tình trạng phòng (Bẩn → Sạch), báo minibar, báo hỏng hóc/bảo trì.
📌 3. Phạm vi hệ thống (Phase 1)

Trong phạm vi:

Đặt phòng (booking mẹ – con)

Check-in / Check-out có xác nhận buồng phòng

Quản lý dịch vụ phòng

Tính giá theo bảng giá (ngày thường, cuối tuần, ngày lễ)

Giữ phòng (Hold) tự động hủy khi hết hạn

Đặt cọc nhiều lần, thanh toán nhiều phần

Xuất hóa đơn & hóa đơn VAT nội bộ

Báo cáo doanh thu và công suất phòng (Excel)

Phân quyền người dùng

Audit log toàn bộ thao tác

Ngoài phạm vi (giai đoạn sau):

Tích hợp Booking.com / Agoda (OTA)

POS/QR thanh toán trực tiếp

Ứng dụng mobile cho buồng phòng

Kéo–thả lịch timeline

2FA, OTP đăng nhập

Hóa đơn điện tử kết nối Thuế

📌 4. Kiến trúc tổng quan

Frontend: WPF (.NET 8 / .NET 7) – MVVM/3-layer (GUI – BLL – DAL)

Backend: SQL Server (Stored Procedures, Trigger, Audit Logs)

Encryption: Hash mật khẩu (SHA256/BCrypt tùy cập nhật), khóa account sau 5 lần sai

Reporting: Export Excel + Print Preview hóa đơn

Email Service: Gửi xác nhận booking tự động

📌 5. Các module chính trong hệ thống
🟦 5.1. Module Đặt phòng (Booking)

Tạo/sửa/xóa booking

Giữ phòng tự động hủy khi hết hạn

Đặt cọc % hoặc số tiền

Gửi email xác nhận

Quản lý booking đoàn (mẹ – con)

Danh mục yêu cầu đặc biệt (thêm giường, tầng cao…)

🟧 5.2. Lễ tân (Front Desk)

Check-in sớm / Check-out trễ (tính phí tự động)

Đổi phòng (cùng loại tự đổi, lên hạng cần phê duyệt)

Gia hạn theo kiểm tra phòng trống

Nhập dịch vụ phòng theo thông báo buồng phòng

🟩 5.3. Trạng thái phòng

Trống – Đang ở – Bẩn – Sạch – Bảo trì

Tự động đổi trạng thái sau check-out

Buồng phòng cập nhật Bẩn → Sạch

Quản lý bảo trì phòng

🟨 5.4. Thanh toán & Hóa đơn

Tính giá theo bảng giá theo mùa

Áp mã giảm giá (% hoặc số tiền)

Thanh toán nhiều phần (partial payment)

In phiếu tạm tính / hóa đơn nội bộ / VAT

🟫 5.5. Báo cáo – Thống kê

Doanh thu theo ngày/tháng/quý/năm

Công suất phòng theo loại phòng

Top dịch vụ sử dụng nhiều nhất

Xuất Excel

🟥 5.6. Người dùng & Phân quyền

Vai trò: Admin – Lễ tân – Buồng phòng

Khóa tài khoản sau 5 lần nhập sai

Audit Log toàn hệ thống

📌 6. Mô hình dữ liệu (ERD rút gọn)

Bao gồm các bảng chính:

Phong, LoaiPhong

BookingMe, BookingCon, BookingYeuCau

BangGia, BangGiaChiTiet

KhachHang

SuDungDichVu

ThuChi, HoaDon

NguoiDung, AuditLog

BaoTriPhong

👉 Chi tiết đầy đủ xem trong thư mục /docs/ERD hoặc file SRS.

📌 7. Quy trình nghiệp vụ chính
🟦 Đặt phòng

Lễ tân tạo booking → hệ thống sinh mã → gửi email xác nhận

Nếu là “giữ phòng” → đếm ngược thời hạn → hết hạn auto-Hủy

🟧 Check-in

Chọn booking → chọn phòng Sạch → chuyển sang Đang ở

Tính phí check-in sớm (nếu có)

🟩 Trong kỳ lưu trú

Buồng phòng báo minibar → lễ tân nhập → tính tiền

Đổi phòng, gia hạn theo điều kiện

🟥 Check-out

Buồng phòng xác nhận minibar/thiệt hại → lễ tân mới được check-out

Phòng chuyển sang Bẩn → buồng phòng dọn → Sạch

📌 8. Cài đặt & chạy chương trình (placeholder)
git clone <repo-url>

# Import database
- Mở SQL Server
- Chạy script trong /database/schema.sql
- Import sample data nếu cần

# Mở solution
- Chạy QuanLyKhachSan.sln
- Cấu hình chuỗi kết nối (DbConfigWindow)


(Bạn có thể yêu cầu mình viết phần này tự động thiệt hoàn chỉnh khi code xong.)

📌 9. Cấu trúc thư mục dự kiến (chừa chỗ)
/src
  /GUI
  /BLL
  /DAL
/database
/docs
  /SRS.pdf
  /ERD.png
/releases
README.md

📌 10. Liên kết quan trọng (chừa chỗ)

🔗 Demo video: (điền sau)

🔗 Tài liệu SRS đầy đủ: (điền sau)

🔗 Slide báo cáo: (điền sau)

🔗 Database scripts: (điền sau)

📌 11. Tác giả (chừa chỗ)
Họ tên	MSSV	Vai trò	Liên hệ
Điền tên tại đây	MSSV	Developer / Phân tích hệ thống	email@domain.com
📌 12. Giấy phép sử dụng (License) – tùy chọn

Mặc định MIT License hoặc để trống nếu là project học tập.
