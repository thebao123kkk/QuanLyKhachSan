select * from TaiKhoanHeThong
select * from NhatKyHeThong
select * from VaiTro
select * from VaiTro_Quyen
select * from Quyen
select * from NhanVien

update NhanVien set ChucVu = N'Buồng phòng' where NhanVienID = 'NV03'
update NhanVien set VaiTroID = 'VT03' where NhanVienID = 'NV03'
update TaiKhoanHeThong set VaiTro = N'Buồng phòng' where TaiKhoanID = 'TK03'
delete from VaiTro_Quyen where VaiTroID = 'VT04'
delete from VaiTro where VaiTroID = 'VT04'


--Đổi bảng nhật ký
DROP TABLE IF EXISTS NhatKyHeThong;
GO

CREATE TABLE NhatKyHeThong (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    TaiKhoanID VARCHAR(50),
    TenDangNhap NVARCHAR(100),
    HanhDong NVARCHAR(50),
    NoiDung NVARCHAR(MAX),
    ThoiDiem DATETIME DEFAULT GETDATE()
);
GO

--Proc ghi log
CREATE OR ALTER PROCEDURE sp_GhiLog
    @TaiKhoanID VARCHAR(50),
    @TenDangNhap NVARCHAR(100),
    @HanhDong NVARCHAR(50),
    @NoiDung NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO NhatKyHeThong (TaiKhoanID, TenDangNhap, HanhDong, NoiDung)
    VALUES (@TaiKhoanID, @TenDangNhap, @HanhDong, @NoiDung);
END
GO

-- Đổi tên cột VaiTro thành VaiTroID
EXEC sp_rename 'TaiKhoanHeThong.VaiTro', 'VaiTroID', 'COLUMN';
GO
ALTER TABLE TaiKhoanHeThong
ALTER COLUMN VaiTroID varchar(50) NOT NULL;
GO
-- Thêm ràng buộc khóa ngoại tới bảng VaiTro
ALTER TABLE TaiKhoanHeThong ADD CONSTRAINT FK_TaiKhoanHeThong_VaiTro FOREIGN KEY (VaiTroID) REFERENCES VaiTro(VaiTroID);
GO

select * from VaiTro
select * from NhanVien
select * from TaiKhoanHeThong

update TaiKhoanHeThong set VaiTroID = 'VT01' where TaiKhoanID = 'TK01'
update TaiKhoanHeThong set VaiTroID = 'VT02' where TaiKhoanID = 'TK02'
update TaiKhoanHeThong set VaiTroID = 'VT03' where TaiKhoanID = 'TK03'

------------------------------------------
CREATE OR ALTER TRIGGER trg_AutoCreateAccount
ON NhanVien
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @NhanVienID VARCHAR(50),
        @Email NVARCHAR(255),
        @VaiTroID VARCHAR(50),
        @TaiKhoanID VARCHAR(50),
        @TenDangNhap NVARCHAR(100),
        @BaseUser NVARCHAR(100);

    -- Lấy dữ liệu nhân viên mới được thêm
    SELECT 
        @NhanVienID = NhanVienID,
        @Email = Email,
        @VaiTroID = VaiTroID
    FROM INSERTED;

    ------------------------------------------
    -- 1️⃣ Sinh TaiKhoanID tự động
    ------------------------------------------
    SET @TaiKhoanID = 'TK' + RIGHT('0000' + CAST((SELECT COUNT(*) + 1 
                         FROM TaiKhoanHeThong) AS VARCHAR(10)), 4);

    ------------------------------------------
    -- 2️⃣ Tạo username dựa trên email
    ------------------------------------------
    SET @BaseUser = LOWER(LEFT(@Email, CHARINDEX('@', @Email) - 1));
    SET @TenDangNhap = @BaseUser;

    -- Nếu username trùng → tự sinh số phía sau
    IF EXISTS (SELECT 1 FROM TaiKhoanHeThong WHERE TenDangNhap = @TenDangNhap)
    BEGIN
        SET @TenDangNhap = @BaseUser + CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR(5));
    END

    ------------------------------------------
    -- 3️⃣ Tạo tài khoản hệ thống
    -- Mật khẩu mặc định: 123456 (MD5 = E10ADC3949BA59ABBE56E057F20F883E)
    ------------------------------------------
    INSERT INTO TaiKhoanHeThong
    (
        TaiKhoanID, TenDangNhap, MatKhauHash, VaiTroID, TrangThai, Email,
        NhanVienID, NgayTao, Khoa
    )
    VALUES
    (
        @TaiKhoanID,
        @TenDangNhap,
        'E10ADC3949BA59ABBE56E057F20F883E',
        @VaiTroID,
        N'Hoạt động',
        @Email,
        @NhanVienID,
        GETDATE(),
        0
    );
END


