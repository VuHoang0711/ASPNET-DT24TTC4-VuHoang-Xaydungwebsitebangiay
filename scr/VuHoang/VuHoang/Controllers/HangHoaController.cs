using CakeShop.Data;
using CakeShop.Models;
using CakeShop.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EcommerceWeb.Controllers
{
    public class HangHoaController : Controller
    {
        public readonly CakeshopContext db;
        public HangHoaController(CakeshopContext context)
        {
            db = context;
        }

        private IQueryable<HangHoa> ApplyFiltersAndSort(
            IQueryable<HangHoa> hangHoas,
            int? loai,
            string? query,
            string? thuonghieu,
            string? gioitinh,
            string? size,
            string? mucgia,
            string? trangthai,
            string? sort)
        {
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
                ViewBag.Loai = loai;
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim();
                hangHoas = hangHoas.Where(p =>
                    p.TenHh.Contains(q) ||
                    (p.TenAlias != null && p.TenAlias.Contains(q)) ||
                    (p.MoTaDonVi != null && p.MoTaDonVi.Contains(q)) ||
                    (p.MoTa != null && p.MoTa.Contains(q)) ||
                    (p.MaLoaiNavigation != null && p.MaLoaiNavigation.TenLoai.Contains(q)) ||
                    (p.MaNccNavigation != null && p.MaNccNavigation.TenCongTy.Contains(q)));
                ViewBag.Query = q;
            }

            if (!string.IsNullOrWhiteSpace(thuonghieu))
            {
                var brand = thuonghieu.Trim();
                hangHoas = hangHoas.Where(p =>
                    p.MaNcc == brand ||
                    p.TenHh.Contains(brand) ||
                    (p.MaNccNavigation != null && p.MaNccNavigation.TenCongTy.Contains(brand)));
                ViewBag.ThuongHieu = brand;
            }

            if (!string.IsNullOrWhiteSpace(gioitinh))
            {
                var g = gioitinh.Trim();
                if (g.Equals("Nam", StringComparison.OrdinalIgnoreCase))
                {
                    hangHoas = hangHoas.Where(p => p.MoTaDonVi != null && (p.MoTaDonVi.Contains("Nam") || p.MoTaDonVi.Contains("Unisex")));
                }
                else if (g.Equals("Nu", StringComparison.OrdinalIgnoreCase) || g.Equals("Nữ", StringComparison.OrdinalIgnoreCase))
                {
                    hangHoas = hangHoas.Where(p => p.MoTaDonVi != null && (p.MoTaDonVi.Contains("Nữ") || p.MoTaDonVi.Contains("Unisex")));
                }
                else if (g.Equals("Unisex", StringComparison.OrdinalIgnoreCase))
                {
                    hangHoas = hangHoas.Where(p => p.MoTaDonVi != null && p.MoTaDonVi.Contains("Unisex"));
                }
                ViewBag.GioiTinh = g;
            }

            if (!string.IsNullOrWhiteSpace(size))
            {
                var s = size.Trim();
                hangHoas = hangHoas.Where(p => p.MoTaDonVi != null && (p.MoTaDonVi.Contains("Size") || p.MoTaDonVi.Contains(s)));
                ViewBag.Size = s;
            }

            if (!string.IsNullOrWhiteSpace(mucgia))
            {
                switch (mucgia)
                {
                    case "duoi-2tr":
                        hangHoas = hangHoas.Where(p => (p.DonGia - p.GiamGia) < 2000000);
                        break;
                    case "2tr-3tr":
                        hangHoas = hangHoas.Where(p => (p.DonGia - p.GiamGia) >= 2000000 && (p.DonGia - p.GiamGia) <= 3000000);
                        break;
                    case "3tr-4tr":
                        hangHoas = hangHoas.Where(p => (p.DonGia - p.GiamGia) > 3000000 && (p.DonGia - p.GiamGia) <= 4000000);
                        break;
                    case "tren-4tr":
                        hangHoas = hangHoas.Where(p => (p.DonGia - p.GiamGia) > 4000000);
                        break;
                }
                ViewBag.MucGia = mucgia;
            }

            if (!string.IsNullOrWhiteSpace(trangthai))
            {
                switch (trangthai)
                {
                    case "khuyenmai":
                        hangHoas = hangHoas.Where(p => p.GiamGia > 0);
                        break;
                    case "banchay":
                        hangHoas = hangHoas.Where(p => p.SoLanXem >= 1200);
                        break;
                    case "moi":
                        hangHoas = hangHoas.OrderByDescending(p => p.NgaySx);
                        break;
                }
                ViewBag.TrangThai = trangthai;
            }

            ViewBag.Sort = sort;
            hangHoas = sort switch
            {
                "price_asc" => hangHoas.OrderBy(p => p.DonGia - p.GiamGia),
                "price_desc" => hangHoas.OrderByDescending(p => p.DonGia - p.GiamGia),
                "newest" => hangHoas.OrderByDescending(p => p.NgaySx).ThenByDescending(p => p.MaHh),
                "bestseller" => hangHoas.OrderByDescending(p => p.SoLanXem),
                "rating" => hangHoas.OrderByDescending(p => p.SoLanXem).ThenBy(p => p.DonGia),
                _ => hangHoas.OrderBy(p => p.MaHh)
            };

            return hangHoas;
        }

        public IActionResult Index(
            int? loai,
            int? page,
            string? query,
            string? thuonghieu,
            string? gioitinh,
            string? size,
            string? mucgia,
            string? trangthai,
            string? sort)
        {
            var hangHoas = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .AsQueryable();

            int pageSize = 9;
            int pageNumber = page ?? 1;

            hangHoas = ApplyFiltersAndSort(hangHoas, loai, query, thuonghieu, gioitinh, size, mucgia, trangthai, sort);

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "Size 36-44 | Unisex",
                TenLoai = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "Giày Thể Thao",
                ThuongHieu = p.MaNccNavigation != null ? p.MaNccNavigation.TenCongTy : "Sneaker Store",
                Giamgia = p.GiamGia,
                SoLanXem = p.SoLanXem
            }).ToPagedList(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Search(
            string? query,
            int? page,
            int? loai,
            string? thuonghieu,
            string? gioitinh,
            string? size,
            string? mucgia,
            string? trangthai,
            string? sort)
        {
            var hangHoas = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .AsQueryable();

            int pageSize = 9;
            int pageNumber = page ?? 1;

            hangHoas = ApplyFiltersAndSort(hangHoas, loai, query, thuonghieu, gioitinh, size, mucgia, trangthai, sort);

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "Size 36-44 | Unisex",
                TenLoai = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "Giày Thể Thao",
                ThuongHieu = p.MaNccNavigation != null ? p.MaNccNavigation.TenCongTy : "Sneaker Store",
                Giamgia = p.GiamGia,
                SoLanXem = p.SoLanXem
            }).ToPagedList(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .Include(p => p.HinhanhSps)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy mẫu giày có mã {id}";
                return Redirect("/404");
            }

            var related = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .Where(p => p.MaHh != id && (p.MaLoai == data.MaLoai || p.MaNcc == data.MaNcc))
                .OrderByDescending(p => p.SoLanXem)
                .Take(4)
                .Select(p => new HangHoaVM
                {
                    MaHh = p.MaHh,
                    TenHH = p.TenHh,
                    DonGia = p.DonGia,
                    Hinh = p.Hinh ?? "",
                    MoTaNgan = p.MoTaDonVi ?? "Size 36-44 | Unisex",
                    TenLoai = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "Giày Thể Thao",
                    ThuongHieu = p.MaNccNavigation != null ? p.MaNccNavigation.TenCongTy : "Sneaker Store",
                    Giamgia = p.GiamGia,
                    SoLanXem = p.SoLanXem
                }).ToList();

            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? "Size 36-44 | Unisex",
                MaLoai = data.MaLoai,
                GiamGia = data.GiamGia,
                TenLoai = data.MaLoaiNavigation?.TenLoai ?? "Giày Thể Thao",
                ThuongHieu = data.MaNccNavigation?.TenCongTy ?? "Sneaker Store",
                SoLuongTon = 25,
                DiemDanhGia = 5,
                HinhanhSps = data.HinhanhSps?.Select(h => new HinhanhSp
                {
                    Id = h.Id,
                    MaHh = h.MaHh,
                    HinhAnhPhu = h.HinhAnhPhu
                }).ToList() ?? new List<HinhanhSp>(),
                SanPhamLienQuan = related
            };
            return View(result);
        }
    }
}
