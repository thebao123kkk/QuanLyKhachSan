using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using DAL;
namespace BLL
{
    public class QuanLyBLL
    {
        // Đọc file Excel và trả về DataTable của sheet đã chọn
        //public static DataTable ImportExcel(string filePath, string sheetName)
        //{
        //    DataTable dt = new DataTable();
        //    using (XLWorkbook workBook = new XLWorkbook(filePath))
        //    {
        //        // Lấy worksheet theo tên (tương ứng với lựa chọn ComboBox)
        //        IXLWorksheet workSheet = workBook.Worksheet(sheetName);
        //        bool firstRow = true;
        //        //foreach (IXLRow row in workSheet.Rows())
        //        //{
        //        //    if (firstRow)
        //        //    {
        //        //        // Dòng đầu: tạo cột cho DataTable
        //        //        foreach (IXLCell cell in row.Cells())
        //        //        {
        //        //            string header = cell.Value.ToString();
        //        //            if (string.IsNullOrEmpty(header)) break;
        //        //            dt.Columns.Add(header);
        //        //        }
        //        //        firstRow = false;
        //        //    }
        //        //    else
        //        //    {
        //        //        // Các dòng còn lại: thêm DataRow
        //        //        DataRow newRow = dt.NewRow();
        //        //        int i = 0;
        //        //        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
        //        //        {
        //        //            newRow[i] = cell.Value.ToString();
        //        //            i++;
        //        //        }
        //        //        dt.Rows.Add(newRow);
        //        //    }
        //        //}
        //    }
        //    return dt;
        //}
        public static DataTable ImportExcel(string filePath, string sheetName)
        {
            DataTable dt = new DataTable();

            using (XLWorkbook wb = new XLWorkbook(filePath))
            {
                var ws = wb.Worksheet(sheetName);
                bool header = true;

                foreach (var row in ws.RowsUsed())
                {
                    if (header)
                    {
                        foreach (var cell in row.CellsUsed())
                        {
                            string colName = cell.GetString().Trim();
                            if (string.IsNullOrEmpty(colName)) continue;

                            // ĐẶT KIỂU CỘT CHO ĐÚNG
                            Type colType = typeof(string);
                            if (colName == "Từ ngày" || colName == "Đến ngày")
                                colType = typeof(DateTime);

                            dt.Columns.Add(colName, colType);
                        }
                        header = false;
                        continue;
                    }

                    DataRow dr = dt.NewRow();
                    int i = 0;

                    foreach (var cell in row.Cells(1, dt.Columns.Count))
                    {
                        string colName = dt.Columns[i].ColumnName;

                        if (colName == "Từ ngày" || colName == "Đến ngày")
                        {
                            // luôn parse về DateTime (dùng lại ParseExcelDate bạn đã có)
                            dr[i] = ParseExcelDate(cell.Value);
                        }
                        else
                        {
                            dr[i] = cell.GetString().Trim();
                        }

                        i++;
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }




        // Lưu dữ liệu bảng "Phòng" vào CSDL (Insert hoặc Update)
        public static void SavePhong(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                // Giả sử cột đầu là IDPhong, các cột tiếp theo là các trường tương ứng
                string id = row[0].ToString();
                // Ví dụ: cột 1 là TenPhong, cột 2 là LoaiPhong, cột 3 là GiaPhong
                string tenPhong = row.Table.Columns.Count > 1 ? row[1].ToString() : "";
                string loaiPhong = row.Table.Columns.Count > 2 ? row[2].ToString() : "";
                string giaPhong = row.Table.Columns.Count > 3 ? row[3].ToString() : "";
                string ghiChu = row.Table.Columns.Count > 4 ? row[3].ToString() : "";
                if (QuanLyDAL.PhongExists(id))
                {
                    // Nếu đã tồn tại, thực hiện UPDATE
                    QuanLyDAL.UpdatePhong(id, tenPhong, loaiPhong, giaPhong, ghiChu);
                }
                else
                {
                    // Nếu chưa, thực hiện INSERT
                    QuanLyDAL.InsertPhong(id, tenPhong, loaiPhong, giaPhong, ghiChu);
                }
            }
            MessageBox.Show("Đã lưu dữ liệu thành công.", "Thông báo");
        }

        public static void SaveLoaiPhong(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string id = row[0].ToString(); // LoaiPhongID
                string nhomLoaiID = row.Table.Columns.Count > 1 ? row[1].ToString() : "";
                string tenLoai = row.Table.Columns.Count > 2 ? row[2].ToString() : "";
                string moTa = row.Table.Columns.Count > 3 ? row[3].ToString() : "";
                int sucChua = row.Table.Columns.Count > 4 ? Convert.ToInt32(row[4]) : 0;
                decimal giaCoBan = row.Table.Columns.Count > 5 ? Convert.ToDecimal(row[5]) : 0;

                if (QuanLyDAL.LoaiPhongExists(id))
                {
                    QuanLyDAL.UpdateLoaiPhong(id, nhomLoaiID, tenLoai, moTa, sucChua, giaCoBan);
                }
                else
                {
                    QuanLyDAL.InsertLoaiPhong(id, nhomLoaiID, tenLoai, moTa, sucChua, giaCoBan);
                }
            }
            MessageBox.Show("Đã lưu dữ liệu loại phòng.");
        }

        public static void SaveNhomLoaiPhong(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string id = row[0].ToString(); // NhomLoaiID
                string tenNhom = row.Table.Columns.Count > 1 ? row[1].ToString() : "";
                string moTa = row.Table.Columns.Count > 2 ? row[2].ToString() : "";

                if (QuanLyDAL.NhomLoaiPhongExists(id))
                    QuanLyDAL.UpdateNhomLoaiPhong(id, tenNhom, moTa);
                else
                    QuanLyDAL.InsertNhomLoaiPhong(id, tenNhom, moTa);
            }
            MessageBox.Show("Đã lưu dữ liệu nhóm loại phòng.");
        }


        public static void SaveDichVuPhong(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string id = row[0].ToString(); // DichVuID
                string tenDV = row.Table.Columns.Count > 1 ? row[1].ToString() : "";
                decimal donGia = row.Table.Columns.Count > 2 ? Convert.ToDecimal(row[2]) : 0;
                string donVi = row.Table.Columns.Count > 3 ? row[3].ToString() : "";
                bool hieuLuc = row.Table.Columns.Count > 4 && (row[4].ToString() == "1" || row[4].ToString().ToLower() == "true");

                if (QuanLyDAL.DichVuExists(id))
                    QuanLyDAL.UpdateDichVu(id, tenDV, donGia, donVi, hieuLuc);
                else
                    QuanLyDAL.InsertDichVu(id, tenDV, donGia, donVi, hieuLuc);
            }
            MessageBox.Show("Đã lưu dữ liệu dịch vụ phòng.");
        }


        //public static void SaveMaGiamGia(DataTable dt)
        //{
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        string id = row[0].ToString(); // MGGID
        //        DateTime tuNgay = Convert.ToDateTime(row[1]);
        //        DateTime denNgay = Convert.ToDateTime(row[2]);
        //        int phanTram = Convert.ToInt32(row[3]);

        //        if (QuanLyDAL.MaGiamGiaExists(id))
        //            QuanLyDAL.UpdateMaGiamGia(id, tuNgay, denNgay, phanTram);
        //        else
        //            QuanLyDAL.InsertMaGiamGia(id, tuNgay, denNgay, phanTram);
        //    }
        //    MessageBox.Show("Đã lưu dữ liệu mã giảm giá.");
        //}
        public static void SaveMaGiamGia(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string id = row["Mã giảm giá"].ToString().Trim();
                if (string.IsNullOrEmpty(id))
                    throw new Exception("MGGID không được rỗng");

                DateTime tu = ParseExcelDate(row["Từ ngày"]);
                DateTime den = ParseExcelDate(row["Đến ngày"]);
                int ptg = Convert.ToInt32(row["Phần trăm giảm giá"]);

                if (tu > den)
                    throw new Exception($"Mã {id}: Từ ngày > Đến ngày");

                if (ptg < 0 || ptg > 100)
                    throw new Exception($"Mã {id}: Phần trăm giảm giá {ptg} không hợp lệ.");

                if (QuanLyDAL.MaGiamGiaExists(id))
                    QuanLyDAL.UpdateMaGiamGia(id, tu, den, ptg);
                else
                    QuanLyDAL.InsertMaGiamGia(id, tu, den, ptg);
            }

            MessageBox.Show("Đã lưu dữ liệu mã giảm giá.");
        }

        private static DateTime ParseExcelDate(object value)
        {
            if (value == null || value == DBNull.Value)
                throw new Exception("Giá trị ngày bị trống.");

            // 1. Ô Excel kiểu DateTime
            if (value is DateTime dt)
                return dt.Date;

            // 2. Ô Excel kiểu số serial (OADate)
            if (double.TryParse(value.ToString(), out double oa))
                return DateTime.FromOADate(oa).Date;

            // 3. Chuỗi – chuẩn hóa & cắt khoảng trắng
            string s = value.ToString().Trim();
            if (string.IsNullOrEmpty(s))
                throw new Exception("Giá trị ngày bị trống.");

            // thử với các format hay dùng
            string[] formats = new[]
            {
        "dd/MM/yyyy", "d/M/yyyy",
        "dd-MM-yyyy", "d-M-yyyy",
        "yyyy-MM-dd",
        "MM/dd/yyyy", "M/d/yyyy"
    };

            if (DateTime.TryParseExact(
                    s,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsed))
            {
                return parsed.Date;
            }

            // fallback: parse theo culture Việt Nam
            if (DateTime.TryParse(s, new CultureInfo("vi-VN"), DateTimeStyles.None, out parsed))
                return parsed.Date;

            throw new Exception($"Không thể chuyển đổi ngày từ '{s}'.");
        }





    }
}
