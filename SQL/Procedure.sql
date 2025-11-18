UPDATE Phong
SET TrangThai = N'Trống';
SELECT * FROM Phong WHERE TrangThai = N'Trống';

UPDATE Phong
SET TrangThai = N'Chờ nhận phòng';
SELECT * FROM Phong WHERE TrangThai = N'Đã nhận';

SELECT 
    dpt.MaDatTong,
    dpct.MaDatChiTiet,
    kh.HoTen,
    p.PhongID,
    p.SoPhong,
    lp.TenLoai,
    dpct.NgayNhan,
    dpct.NgayTra,
    dpct.SoLuongPhong,
    dpt.TrangThai
FROM DatPhongTong dpt
JOIN KhachHang kh ON kh.KhachHangID = dpt.KhachHangID
JOIN Phong p ON p.PhongID = dpt.PhongID
JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
JOIN DatPhongChiTiet dpct ON dpct.MaDatTong = dpt.MaDatTong
WHERE kh.HoTen LIKE '%ABC%'       


--Insert khách hàng nếu chưa tồn tại
CREATE OR ALTER PROCEDURE sp_InsertKhachHang
    @HoTen NVARCHAR(150),
    @SDT NVARCHAR(30),
    @Email NVARCHAR(255),
    @CongTy NVARCHAR(150),
    @MST NVARCHAR(50),
    @DiaChi NVARCHAR(255),
    @KhachHangID VARCHAR(50) OUTPUT
AS
BEGIN
    -- Nếu đã tồn tại -> trả về ID cũ
    IF EXISTS (SELECT 1 FROM KhachHang WHERE SDT = @SDT)
    BEGIN
        SELECT @KhachHangID = KhachHangID FROM KhachHang WHERE SDT = @SDT;
        RETURN;
    END

    DECLARE @NewID VARCHAR(50) = 'KH' + RIGHT('0000' + CAST((SELECT COUNT(*) + 1 FROM KhachHang) AS VARCHAR(10)), 4);

    INSERT INTO KhachHang VALUES (@NewID, @HoTen, @SDT, @Email, @CongTy, @MST, @DiaChi, GETDATE());

    SET @KhachHangID = @NewID;
END
--Insert DatPhongTong
CREATE OR ALTER PROCEDURE sp_InsertDatPhongTong
    @KhachHangID VARCHAR(50),
    @TenDaiDien NVARCHAR(150),
    @SDTDaiDien NVARCHAR(30),
    @LaDoan BIT,
    @TongTienCoc DECIMAL(18,2),
    @GhiChu NVARCHAR(255),
    @NhanVienID VARCHAR(50),
    @PhongID VARCHAR(50),

    @MaDatTong VARCHAR(50) OUTPUT
AS
BEGIN
    DECLARE @NewID VARCHAR(50)
    SET @NewID = 'DP_' + RIGHT('0000' + CAST((SELECT COUNT(*) + 1 FROM DatPhongTong) AS VARCHAR(10)), 4);

    DECLARE @MaCode NVARCHAR(20)
    SET @MaCode = 'BK' + CONVERT(VARCHAR(8), GETDATE(), 112) + '_' +
                  RIGHT('0000' + CAST((SELECT COUNT(*) + 1 FROM DatPhongTong) AS VARCHAR(10)), 4);

    INSERT INTO DatPhongTong
    VALUES (@NewID, @MaCode, @KhachHangID, @TenDaiDien, @SDTDaiDien,
            @LaDoan, N'Đã đặt', @TongTienCoc, GETDATE(), @GhiChu,
            @NhanVienID, @PhongID);

    SET @MaDatTong = @NewID;
END
--Insert DatPhongChiTiet
CREATE OR ALTER PROCEDURE sp_InsertDatPhongChiTiet
    @MaDatTong VARCHAR(50),
    @NgayNhan DATE,
    @NgayTra DATE,
    @NguoiLon INT,
    @TreEm INT,
    @SoLuongPhong INT,
    @VAT DECIMAL(5,2),
    @ThanhTien DECIMAL(18,2),
    @GhiChu NVARCHAR(255),
    @MaDatChiTiet VARCHAR(50) OUTPUT
AS
BEGIN
    DECLARE @NewID VARCHAR(50)
    SET @NewID = 'DP_C' + RIGHT('0000' + CAST((SELECT COUNT(*) + 1 FROM DatPhongChiTiet) AS VARCHAR(10)), 4);

    INSERT INTO DatPhongChiTiet
    VALUES (@NewID, @MaDatTong, @NgayNhan, @NgayTra,
            @NguoiLon, @TreEm, @SoLuongPhong, @VAT, @ThanhTien, @GhiChu);

    SET @MaDatChiTiet = @NewID;
END
--Insert PhongMoi (dùng sau)
CREATE OR ALTER PROCEDURE sp_InsertPhong
    @SoPhong NVARCHAR(30),
    @LoaiPhongID VARCHAR(50),
    @PhongID VARCHAR(50) OUTPUT
AS
BEGIN
    SET @PhongID = 'P' + @SoPhong;

    INSERT INTO Phong (PhongID, SoPhong, LoaiPhongID, TrangThai, GhiChu)
    VALUES (@PhongID, @SoPhong, @LoaiPhongID, N'Đang thuê', NULL);
END

SELECT
    dpct.MaDatChiTiet,
    dpt.MaDatTong,
    kh.HoTen,
    p.SoPhong,
    lp.TenLoai AS LoaiPhong,
    dpct.NgayNhan,
    dpct.NgayTra,
    dpct.NguoiLon,
    dpct.TreEm,
    dpct.SoLuongPhong,
    dpct.VAT,
    dpct.ThanhTien,
    dpt.GhiChu
FROM DatPhongChiTiet dpct
JOIN DatPhongTong dpt ON dpct.MaDatTong = dpt.MaDatTong
JOIN KhachHang kh ON kh.KhachHangID = dpt.KhachHangID
JOIN Phong p ON p.PhongID = dpt.PhongID
JOIN LoaiPhongChiTiet lp ON lp.LoaiPhongID = p.LoaiPhongID
ORDER BY dpct.NgayNhan DESC;
