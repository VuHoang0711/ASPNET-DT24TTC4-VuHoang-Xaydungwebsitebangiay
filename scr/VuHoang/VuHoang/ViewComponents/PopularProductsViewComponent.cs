using CakeShop.Data;
using CakeShop.ModelsView;
using Microsoft.AspNetCore.Mvc;

namespace CakeShop.ViewComponents
{
    public class PopularProductsViewComponent : ViewComponent
    {
        private readonly CakeshopContext db;
        public PopularProductsViewComponent(CakeshopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.HangHoas
                .OrderByDescending(p => p.SoLanXem)
                .Take(8)
                .Select(lo => new ChiTietHangHoaVM
                {
                    MaLoai = lo.MaLoai,
                    MaHh = lo.MaHh,
                    TenLoai = lo.MaLoaiNavigation != null ? lo.MaLoaiNavigation.TenLoai : "Giày Sneaker",
                    ThuongHieu = lo.MaNccNavigation != null ? lo.MaNccNavigation.TenCongTy : "Sneaker Store",
                    MoTaNgan = lo.MoTaDonVi ?? "Size 36-44 | Unisex",
                    TenHH = lo.TenHh,
                    Hinh = lo.Hinh,
                    DonGia = lo.DonGia,
                    GiamGia = lo.GiamGia,
                    HinhanhSps = lo.HinhanhSps ?? new List<HinhanhSp>(),
                }).ToList();
            return View("TagProduct", data);
        }
    }
}
