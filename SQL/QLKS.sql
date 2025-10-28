--create database QuanLyKhachSan

CREATE TABLE NhomLoaiPhong (
  NhomLoaiID varchar(50) PRIMARY KEY,
  TenNhom nvarchar(100),
  MoTa nvarchar(255)
)
GO

CREATE TABLE LoaiPhongChiTiet (
  LoaiPhongID varchar(50) PRIMARY KEY,
  NhomLoaiID varchar(50),
  TenLoai nvarchar(100),
  MoTa nvarchar(255),
  SucChua int,
  GiaCoBan decimal(18,2),
)
GO

CREATE TABLE Phong (
  PhongID varchar(50) PRIMARY KEY,
  SoPhong nvarchar(30) UNIQUE,
  LoaiPhongID varchar(50),
  TrangThai nvarchar(20),
  GhiChu nvarchar(255)
)
GO

CREATE TABLE BangGiaPhong (
  BangGiaID varchar(50) PRIMARY KEY,
  TenBangGia nvarchar(150),
  TuNgay date,
  DenNgay date
)
GO

CREATE TABLE GiaTheoLoaiPhong (
  BangGiaID varchar(50),
  LoaiPhongID varchar(50),
  DonGia decimal(18,2),
  PRIMARY KEY (BangGiaID, LoaiPhongID)
)
GO

CREATE TABLE KhachHang (
  KhachHangID varchar(50) PRIMARY KEY,
  HoTen nvarchar(150),
  SDT nvarchar(30),
  Email nvarchar(255),
  CongTy nvarchar(150),
  MST nvarchar(50),
  DiaChi nvarchar(255),
  NgayTao datetime2
)
GO

CREATE TABLE DatPhongTong (
  MaDatTong varchar(50) PRIMARY KEY,
  MaCode nvarchar(20) UNIQUE,
  KhachHangID varchar(50),
  TenDaiDien nvarchar(150),
  SDTDaiDien nvarchar(30),
  LaDoan bit,
  TrangThai nvarchar(20),
  TongTienCoc decimal(18,2),
  NgayTao datetime2,
  GhiChu nvarchar(255),
  NhanVienID varchar(50),
  PhongID varchar(50)
)
GO

CREATE TABLE DatPhongChiTiet (
  MaDatChiTiet varchar(50) PRIMARY KEY,
  MaDatTong varchar(50),
  NgayNhan date,
  NgayTra date,
  NguoiLon int,
  TreEm int,
  SoLuongPhong int,
  VAT decimal(5,2),
  ThanhTien decimal(18,2),
  GhiChu nvarchar(255)

)
GO

CREATE TABLE CTHD (
  MaDatChiTiet varchar(50),
  MaHoaDon varchar(50)
)
GO

CREATE TABLE DichVuPhong (
  DichVuID varchar(50) PRIMARY KEY,
  TenDichVu nvarchar(150),
  DonGia decimal(18,2),
  DonVi nvarchar(50),
  HieuLuc bit
)
GO

CREATE TABLE ChiTietDichVu (
  MaChiTietDV varchar(50) PRIMARY KEY,
  MaDatChiTiet varchar(50),
  DichVuID varchar(50),
  SoLuong decimal(18,2),
  DonGiaTaiThoiDiem decimal(18,2),
  NgaySuDung datetime2
)
GO

CREATE TABLE GiaoDichThuChi (
  MaGiaoDich varchar(50) PRIMARY KEY,
  MaDatChiTiet varchar(50),
  SoTien decimal(18,2),
  HinhThuc nvarchar(30),
  LaDatCoc bit,
  GhiChu nvarchar(255),
  NgayGhiNhan datetime2,
  NhanVienID varchar(50)
)
GO

CREATE TABLE HoaDonThanhToan (
  MaHoaDon varchar(50) PRIMARY KEY,
  DaThu decimal(18,2),
  ConLai decimal(18,2),
  NgayLap datetime2
)
GO

CREATE TABLE VaiTro (
  VaiTroID varchar(50) PRIMARY KEY,
  TenVaiTro nvarchar(100),
  MoTa nvarchar(255),

)
GO

CREATE TABLE Quyen (
  QuyenID varchar(50) PRIMARY KEY,
  TenQuyen nvarchar(100),
  MoTa nvarchar(255),
  NhomQuyen nvarchar(100),

)
GO

CREATE TABLE VaiTro_Quyen (
  VaiTroID varchar(50),
  QuyenID varchar(50),
  PRIMARY KEY (VaiTroID, QuyenID)
)
GO

CREATE TABLE NhanVien (
  NhanVienID varchar(50) PRIMARY KEY,
  HoTen nvarchar(150),
  ChucVu nvarchar(100),
  GioiTinh nvarchar(10),
  NgaySinh date,
  DienThoai nvarchar(30),
  Email nvarchar(255),
  DiaChi nvarchar(255),
  VaiTroID varchar(50),
  TrangThai nvarchar(20),
  NgayTao datetime2
)
GO

CREATE TABLE TaiKhoanHeThong (
  TaiKhoanID varchar(50) PRIMARY KEY,
  TenDangNhap nvarchar(100) UNIQUE,
  MatKhauHash varchar(256),
  VaiTro nvarchar(50),
  TrangThai nvarchar(20),
  Email nvarchar(255),
  NhanVienID varchar(50),
  NgayTao datetime2
)
GO

CREATE TABLE NhatKyHeThong (
  LogID varchar(50) PRIMARY KEY,
  TaiKhoanID varchar(50),
  HanhDong nvarchar(50),
  DoiTuong nvarchar(50),
  MaDoiTuong varchar(50),
  Truoc nvarchar(max),
  Sau nvarchar(max),
  ThoiDiem datetime2,
  GhiChu nvarchar(255)
)
GO

ALTER TABLE LoaiPhongChiTiet ADD FOREIGN KEY (NhomLoaiID) REFERENCES NhomLoaiPhong (NhomLoaiID)
GO

ALTER TABLE Phong ADD FOREIGN KEY (LoaiPhongID) REFERENCES LoaiPhongChiTiet (LoaiPhongID)
GO

ALTER TABLE GiaTheoLoaiPhong ADD FOREIGN KEY (BangGiaID) REFERENCES BangGiaPhong (BangGiaID)
GO

ALTER TABLE GiaTheoLoaiPhong ADD FOREIGN KEY (LoaiPhongID) REFERENCES LoaiPhongChiTiet (LoaiPhongID)
GO

ALTER TABLE DatPhongTong ADD FOREIGN KEY (KhachHangID) REFERENCES KhachHang (KhachHangID)
GO

ALTER TABLE DatPhongTong ADD FOREIGN KEY (PhongID) REFERENCES Phong (PhongID)
GO

ALTER TABLE DatPhongChiTiet ADD FOREIGN KEY (MaDatTong) REFERENCES DatPhongTong (MaDatTong)
GO

ALTER TABLE DatPhongTong ADD FOREIGN KEY (NhanVienID) REFERENCES NhanVien (NhanVienID)
GO

ALTER TABLE CTHD ADD FOREIGN KEY (MaDatChiTiet) REFERENCES DatPhongChiTiet (MaDatChiTiet)
GO

ALTER TABLE CTHD ADD FOREIGN KEY (MaHoaDon) REFERENCES HoaDonThanhToan (MaHoaDon)
GO

ALTER TABLE ChiTietDichVu ADD FOREIGN KEY (MaDatChiTiet) REFERENCES DatPhongChiTiet (MaDatChiTiet)
GO

ALTER TABLE ChiTietDichVu ADD FOREIGN KEY (DichVuID) REFERENCES DichVuPhong (DichVuID)
GO

ALTER TABLE GiaoDichThuChi ADD FOREIGN KEY (MaDatChiTiet) REFERENCES DatPhongChiTiet (MaDatChiTiet)
GO

ALTER TABLE GiaoDichThuChi ADD FOREIGN KEY (NhanVienID) REFERENCES NhanVien (NhanVienID)
GO

ALTER TABLE VaiTro_Quyen ADD FOREIGN KEY (VaiTroID) REFERENCES VaiTro (VaiTroID)
GO

ALTER TABLE VaiTro_Quyen ADD FOREIGN KEY (QuyenID) REFERENCES Quyen (QuyenID)
GO

ALTER TABLE NhanVien ADD FOREIGN KEY (VaiTroID) REFERENCES VaiTro (VaiTroID)
GO

ALTER TABLE TaiKhoanHeThong ADD FOREIGN KEY (NhanVienID) REFERENCES NhanVien (NhanVienID)
GO

ALTER TABLE NhatKyHeThong ADD FOREIGN KEY (TaiKhoanID) REFERENCES TaiKhoanHeThong (TaiKhoanID)
GO


--Insert dữ liệu
--NHÂN VIÊN VÀ PHÂN QUYỀN
INSERT INTO VaiTro VALUES
('VT01', N'Quản lý', N'Quản lý toàn bộ hệ thống'),
('VT02', N'Lễ tân', N'Tiếp nhận và xử lý đặt phòng'),
('VT03', N'Buồng phòng', N'Quản lý tình trạng phòng'),
('VT04', N'Kế toán', N'Xử lý hóa đơn và giao dịch');

INSERT INTO Quyen VALUES
('Q01', N'Xem dữ liệu', N'Cho phép xem thông tin', N'Hệ thống'),
('Q02', N'Thêm mới', N'Cho phép thêm dữ liệu', N'Hệ thống'),
('Q03', N'Sửa', N'Cho phép chỉnh sửa dữ liệu', N'Hệ thống'),
('Q04', N'Xóa', N'Cho phép xóa dữ liệu', N'Hệ thống'),
('Q05', N'Duyệt đơn', N'Cho phép duyệt đặt phòng', N'Nghiệp vụ');

INSERT INTO VaiTro_Quyen VALUES
('VT01', 'Q01'), ('VT01', 'Q02'), ('VT01', 'Q03'), ('VT01', 'Q04'), ('VT01', 'Q05'),
('VT02', 'Q01'), ('VT02', 'Q02'), ('VT02', 'Q03'), ('VT02', 'Q05'),
('VT03', 'Q01'), ('VT03', 'Q03'),
('VT04', 'Q01'), ('VT04', 'Q02'), ('VT04', 'Q03');

INSERT INTO NhanVien VALUES
('NV01', N'Trần Thị Mai', N'Quản lý', N'Nữ', '1990-05-10', '0912345678', 'mai.tran@hotel.vn', N'Hà Nội', 'VT01', N'Đang làm', GETDATE()),
('NV02', N'Lê Minh Tuấn', N'Lễ tân', N'Nam', '1998-08-20', '0988111222', 'tuan.le@hotel.vn', N'Hà Nội', 'VT02', N'Đang làm', GETDATE()),
('NV03', N'Phạm Thu Thảo', N'Kế toán', N'Nữ', '1995-12-15', '0909888777', 'thao.pham@hotel.vn', N'Hà Nội', 'VT04', N'Đang làm', GETDATE());

INSERT INTO TaiKhoanHeThong VALUES
('TK01', 'admin', 'E10ADC3949BA59ABBE56E057F20F883E', 'Quản lý', N'Hoạt động', 'mai.tran@hotel.vn', 'NV01', GETDATE()),
('TK02', 'letuan', 'E10ADC3949BA59ABBE56E057F20F883E', 'Lễ tân', N'Hoạt động', 'tuan.le@hotel.vn', 'NV02', GETDATE()),
('TK03', 'thaopham', 'E10ADC3949BA59ABBE56E057F20F883E', 'Kế toán', N'Hoạt động', 'thao.pham@hotel.vn', 'NV03', GETDATE());


-- QUẢN LÝ PHÒNG
-- Nhóm loại phòng
INSERT INTO NhomLoaiPhong VALUES 
('NLP01', N'Phòng Thường', N'Dành cho khách phổ thông'),
('NLP02', N'Phòng VIP', N'Trang bị cao cấp, phục vụ đặc biệt');

-- Loại phòng chi tiết
INSERT INTO LoaiPhongChiTiet VALUES
('LP01', 'NLP01', N'Standard', N'Phòng tiêu chuẩn 1 giường đôi', 2, 500000),
('LP02', 'NLP01', N'Superior', N'Phòng 2 giường đơn, view thành phố', 3, 650000),
('LP03', 'NLP02', N'Deluxe', N'Phòng VIP có bồn tắm và minibar', 2, 950000),
('LP04', 'NLP02', N'Suite', N'Phòng Suite cao cấp có phòng khách riêng', 4, 1500000);

-- Phòng cụ thể
INSERT INTO Phong VALUES
('P101', '101', 'LP01', N'Trống', N'Không ghi chú'),
('P102', '102', 'LP02', N'Đang dọn', N'Sắp có khách'),
('P201', '201', 'LP03', N'Đang thuê', N'Khách VIP lưu trú 2 đêm'),
('P301', '301', 'LP04', N'Trống', N'Mới nâng cấp nội thất');

-- Bảng giá phòng
INSERT INTO BangGiaPhong VALUES
('BG01', N'Giá mùa thường', '2025-01-01', '2025-06-30'),
('BG02', N'Giá mùa cao điểm', '2025-07-01', '2025-12-31');

-- Giá theo loại phòng
INSERT INTO GiaTheoLoaiPhong VALUES
('BG01', 'LP01', 500000),
('BG01', 'LP02', 650000),
('BG01', 'LP03', 950000),
('BG01', 'LP04', 1500000),
('BG02', 'LP01', 600000),
('BG02', 'LP02', 800000),
('BG02', 'LP03', 1200000),
('BG02', 'LP04', 1800000);

-- KHÁCH HÀNG
INSERT INTO KhachHang VALUES
('KH01', N'Nguyễn Văn An', '0905123456', 'an.nguyen@example.com', NULL, NULL, N'Hà Nội', GETDATE()),
('KH02', N'Công ty ABC Travel', '0988111222', 'info@abctravel.vn', N'ABC Travel', '0101234567', N'Hồ Chí Minh', GETDATE());

-- Đặt phòng tổng (đã thêm NhanVienID)
INSERT INTO DatPhongTong VALUES
('DP_0001', 'BK20251020_0001', 'KH01', N'Nguyễn Văn An', '0905123456', 0, N'Đã xác nhận', 200000, GETDATE(), N'Khách lẻ', 'NV02', 'P201'),
('DP_0002', 'BK20251020_0002', 'KH02', N'Lê Thị Hạnh', '0988333444', 1, N'Đang xử lý', 1000000, GETDATE(), N'Đặt đoàn 5 phòng', 'NV03', 'P101');

-- ĐẶT PHÒNG CHI TIẾT (KHÔNG CÒN PHONGID)
INSERT INTO DatPhongChiTiet VALUES
('DP_C001', 'DP_0001', '2025-10-20', '2025-10-22', 2, 0, 1, 10.00, 2090000, N'Khách VIP'),
('DP_C002', 'DP_0002', '2025-11-10', '2025-11-15', 2, 1, 5, 8.00, 16250000, N'Đặt đoàn');


--THANH TOÁN VÀ GIAO DỊCH
INSERT INTO HoaDonThanhToan VALUES
('HD001', 2000000, 90000, GETDATE()),
('HD002', 15000000, 1250000, GETDATE());

INSERT INTO GiaoDichThuChi VALUES
('GD01', 'DP_C001', 200000, N'Tiền mặt', 1, N'Cọc giữ phòng', GETDATE(), 'NV02'),
('GD02', 'DP_C001', 1890000, N'Chuyển khoản', 0, N'Thanh toán khi trả phòng', GETDATE(), 'NV02'),
('GD03', 'DP_C002', 1000000, N'Tiền mặt', 1, N'Cọc đoàn', GETDATE(), 'NV03');

-- Liên kết chi tiết đặt phòng - hóa đơn
INSERT INTO CTHD VALUES
('DP_C001', 'HD001'),
('DP_C002', 'HD002');

--DỊCH VỤ PHONG
INSERT INTO DichVuPhong VALUES
('DV01', N'Ăn sáng buffet', 100000, N'Suất', 1),
('DV02', N'Giặt ủi', 50000, N'Kg', 1),
('DV03', N'Spa thư giãn', 300000, N'Giờ', 1),
('DV04', N'Minibar', 150000, N'Gói', 1);
--.-
INSERT INTO ChiTietDichVu VALUES
('CTDV01', 'DP_C001', 'DV03', 1, 300000, '2025-10-21 10:00'),
('CTDV02', 'DP_C001', 'DV01', 2, 100000, '2025-10-21 08:00'),
('CTDV03', 'DP_C002', 'DV02', 5, 50000, '2025-11-12 15:00');

--LOG
INSERT INTO NhatKyHeThong VALUES
('LOG001', 'TK02', N'Thêm', N'Đặt phòng', 'DP_0001', NULL, N'{TongTienCoc:200000}', GETDATE(), N'Tạo đơn đặt phòng lẻ'),
('LOG002', 'TK03', N'Cập nhật', N'Hóa đơn', 'HD001', N'{ConLai:90000}', N'{ConLai:0}', GETDATE(), N'Khách đã thanh toán đủ'),
('LOG003', 'TK01', N'Xóa', N'Phòng', 'P102', N'{TrangThai:"Đang dọn"}', NULL, GETDATE(), N'Xóa phòng do trùng mã');


/* ================================================
   🔁 SELECT TOÀN BỘ BẢNG TRONG DATABASE 
   ================================================ */
   /* ==========================================
   🔍 XEM TOÀN BỘ DỮ LIỆU TRONG DATABASE
   Dự án: QuanLyKhachSan
   ========================================== */

-- 1️⃣ Quản lý phòng
SELECT * FROM NhomLoaiPhong;
SELECT * FROM LoaiPhongChiTiet;
SELECT * FROM Phong;
SELECT * FROM BangGiaPhong;
SELECT * FROM GiaTheoLoaiPhong;

-- 2️⃣ Khách hàng & đặt phòng
SELECT * FROM KhachHang;
SELECT * FROM DatPhongTong;
SELECT * FROM DatPhongChiTiet;
SELECT * FROM CTHD;

-- 3️⃣ Dịch vụ
SELECT * FROM DichVuPhong;
SELECT * FROM ChiTietDichVu;

-- 4️⃣ Thanh toán & giao dịch
SELECT * FROM HoaDonThanhToan;
SELECT * FROM GiaoDichThuChi;

-- 5️⃣ Nhân viên & phân quyền
SELECT * FROM VaiTro;
SELECT * FROM Quyen;
SELECT * FROM VaiTro_Quyen;
SELECT * FROM NhanVien;
SELECT * FROM TaiKhoanHeThong;

-- 6️⃣ Nhật ký hệ thống
SELECT * FROM NhatKyHeThong;


/*

-- Bước 1: Xóa tất cả FOREIGN KEY trong database
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'ALTER TABLE [' + s.name + '].[' + t.name + '] DROP CONSTRAINT [' + fk.name + '];' + CHAR(13)
FROM sys.foreign_keys fk
JOIN sys.tables t ON fk.parent_object_id = t.object_id
JOIN sys.schemas s ON t.schema_id = s.schema_id;

EXEC sp_executesql @sql;
PRINT N'✅ Tất cả khóa ngoại (FOREIGN KEY) đã được xóa.';


-- Bước 2: Xóa tất cả TABLE trong database
SET @sql = N'';

SELECT @sql += N'DROP TABLE [' + s.name + '].[' + t.name + '];' + CHAR(13)
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id;

EXEC sp_executesql @sql;
PRINT N'✅ Tất cả bảng (TABLE) đã được xóa hoàn toàn.'; */
