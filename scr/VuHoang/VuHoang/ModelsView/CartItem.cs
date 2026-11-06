namespace CakeShop.ModelsView
{
    public class CartItem
    {
        public int MaHh { get; set; }
        public string Hinh { get; set; } = string.Empty;
        public string TenHH { get; set; } = string.Empty;
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double GiamGia { get; set; }
        public string KichCo { get; set; } = "41";
        public string MauSac { get; set; } = "Phối màu Tiêu chuẩn";
        public double ThanhTien => SoLuong * (DonGia - GiamGia);
    }
}
