using CakeShop.Data;

namespace CakeShop.ModelsView
{
    public class HangHoaVM
    {
        public int MaHh { get; set; }

        public string TenHH { get; set; } = string.Empty;

        public string Hinh { get; set; } = string.Empty;

        public double DonGia { get; set; }

        public string MoTaNgan { get; set; } = string.Empty;

        public string TenLoai { get; set; } = string.Empty;

        public string ThuongHieu { get; set; } = string.Empty;

        public double Giamgia { get; set; }

        public int SoLanXem { get; set; }
    }

    public class ChiTietHangHoaVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; } = string.Empty;
        public string Hinh { get; set; } = string.Empty;
        public double DonGia { get; set; }
        public double GiamGia { get; set; }
        public string MoTaNgan { get; set; } = string.Empty;
        public int MaLoai { get; set; }
        public string TenLoai { get; set; } = string.Empty;
        public string ThuongHieu { get; set; } = string.Empty;
        public string ChiTiet { get; set; } = string.Empty;
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
        public ICollection<HinhanhSp> HinhanhSps { get; set; } = new List<HinhanhSp>();
        public List<HangHoaVM> SanPhamLienQuan { get; set; } = new List<HangHoaVM>();
    }
}
