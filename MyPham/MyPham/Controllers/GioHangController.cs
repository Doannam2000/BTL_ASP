using MyPham.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Controllers
{
    public class GioHangController : Controller
    {
        MyPhamDB db = new MyPhamDB();

        // GET: GioHang
        public ActionResult Index()
        {
            var list = new List<Gio>();
            var gioHang = Session["GioHang"];
            if (list != null)
            {
                list = (List<Gio>)gioHang;
            }
            return View(list);
        }
        public ActionResult ThemSP(int MaSP, int SoLuong)
        {
            int MaGH = -1;
            if (Session["MaGH"] != null)
            {
                MaGH = (int)Session["MaGH"];
            }
            var sanpham = db.SanPham.Find(MaSP);
            var gioHang = Session["GioHang"];
            if (gioHang != null)
            {
                var list = (List<Gio>)gioHang;
                if (list.Exists(x => x.sanPham.MaSP == MaSP))
                {
                    foreach (var item in list)
                    {
                        if (item.sanPham.MaSP == MaSP)
                        {
                            item.soLuong += SoLuong;
                            if (MaGH != -1)
                            {
                                var update = db.Chi_Tiet_Gio_Hang.Find(MaGH);
                                update.SoLuongMua = item.soLuong;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var item = new Gio();
                    item.sanPham = sanpham;
                    item.soLuong = SoLuong;
                    list.Add(item);
                    if (MaGH != -1)
                    {
                        Chi_Tiet_Gio_Hang chiTiet = new Chi_Tiet_Gio_Hang();
                        chiTiet.MaGioHang = MaGH;
                        chiTiet.MaSP = item.sanPham.MaSP;
                        chiTiet.SoLuongMua = SoLuong;
                        chiTiet.GiaSP = (Convert.ToInt32(item.sanPham.Gia) - Convert.ToInt32(Convert.ToInt32(item.sanPham.Gia) * (item.sanPham.GiamGia.HasValue ? item.sanPham.GiamGia.Value : 0)));
                        db.Chi_Tiet_Gio_Hang.Add(chiTiet);
                        db.SaveChanges();
                    }
                }
                Session["GioHang"] = list;
            }
            else
            {
                var item = new Gio();
                item.sanPham = sanpham;
                item.soLuong = SoLuong;
                var list = new List<Gio>();
                list.Add(item);
                if (MaGH != -1)
                {
                    Chi_Tiet_Gio_Hang chiTiet = new Chi_Tiet_Gio_Hang();
                    chiTiet.MaGioHang = MaGH;
                    chiTiet.MaSP = item.sanPham.MaSP;
                    chiTiet.SoLuongMua = SoLuong;
                    chiTiet.GiaSP = (Convert.ToInt32(item.sanPham.Gia) - Convert.ToInt32(Convert.ToInt32(item.sanPham.Gia) * (item.sanPham.GiamGia.HasValue ? item.sanPham.GiamGia.Value : 0)));
                    db.Chi_Tiet_Gio_Hang.Add(chiTiet);
                    db.SaveChanges();
                }
                Session["GioHang"] = list;
            }
            return RedirectToAction("SanPham", "SanPham", new { id = MaSP });
        }

        public RedirectToRouteResult SuaSoLuong(int MaSP, int SoLuong)
        {

            int MaGH = -1;
            if (Session["MaGH"] != null)
            {
                MaGH = (int)Session["MaGH"];
            }
            List<Gio> giohang = Session["GioHang"] as List<Gio>;
            var itemSua = giohang.FirstOrDefault(m => m.sanPham.MaSP == MaSP);
            if (itemSua != null)
            {
                if (SoLuong == 0)
                {
                    if (MaGH != -1)
                    {
                        var item = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == MaGH && s.MaSP == MaSP).FirstOrDefault();
                        db.Chi_Tiet_Gio_Hang.Remove(item);
                        db.SaveChanges();
                    }
                    giohang.Remove(itemSua);
                    return RedirectToAction("Index");
                }
                itemSua.soLuong = SoLuong;
                if (MaGH != -1)
                {
                    var update = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == MaGH && s.MaSP == MaSP).FirstOrDefault();
                    update.SoLuongMua = SoLuong;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }

        public RedirectToRouteResult XoaKhoiGio(int MaSP)
        {
            int MaGH = -1;
            if (Session["MaGH"] != null)
            {
                MaGH = (int)Session["MaGH"];
            }
            List<Gio> giohang = Session["GioHang"] as List<Gio>;
            if (MaSP == -1)
            {
                Session["GioHang"] = null;
                if (MaGH != -1)
                {
                    var XoaGH = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == MaGH).ToList();
                    foreach (var item in XoaGH)
                    {
                        db.Chi_Tiet_Gio_Hang.Remove(item);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            var itemXoa = giohang.FirstOrDefault(m => m.sanPham.MaSP == MaSP);
            if (itemXoa != null)
            {
                if (MaGH != -1)
                {
                    var item = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == MaGH && s.MaSP == MaSP).FirstOrDefault();
                    db.Chi_Tiet_Gio_Hang.Remove(item);
                    db.SaveChanges();
                }
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan()
        {
            var list = new List<Gio>();
            var gioHang = Session["GioHang"];
            if (list != null)
            {
                list = (List<Gio>)gioHang;
            }
            if (Session["idUser"] != null)
            {
                int MaTK = (int)Session["idUser"];
                TaiKhoan taiKhoan = db.TaiKhoan.Find(MaTK);
                ViewBag.TaiKhoan = taiKhoan;
            }
            return View(list);
        }

        public ActionResult XacNhanThanhToan(string email, string HoTen, string diachi, string sodienthoai, string GhiChu, string matkhau)
        {
            if (Session["idUser"] == null)
            {
                if (checkEmail(email))
                {
                    var user = db.TaiKhoan.Where(u => u.Email.Equals(email) && u.MatKhau.Equals(matkhau)).ToList();
                    if (user.Count() > 0)
                    {
                        Session["Email"] = user.FirstOrDefault().Email;
                        Session["HoTen"] = user.FirstOrDefault().HoTen;
                        Session["Anh"] = user.FirstOrDefault().Anh;
                        Session["idUser"] = user.FirstOrDefault().MaTK;
                    }
                }
                else
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk.HoTen = HoTen;
                    tk.Email = email;
                    tk.DiaChi = diachi;
                    tk.MaQuyen = 3;
                    tk.MatKhau = matkhau;
                    tk.TinhTrang = true;
                    db.TaiKhoan.Add(tk);
                    db.SaveChanges();
                    Session["idUser"] = db.TaiKhoan.Where(t => t.Email == email).FirstOrDefault().MaTK;
                    Session["Email"] = tk.Email;
                    Session["HoTen"] = tk.HoTen;
                    Session["Anh"] = tk.Anh;
                }
                GioHang gh = new GioHang();
                gh.MaTK = (int)Session["idUser"];
                db.GioHang.Add(gh);
                db.SaveChanges();
                int MaTK1 = (int)Session["idUser"];
                GioHang gio = db.GioHang.Where(g => g.MaTK == MaTK1).FirstOrDefault();
                Session["MaGH"] = gio.MaGioHang;
            }
            int MaTK = (int)Session["idUser"];
            HoaDon hoaDon = new HoaDon();
            hoaDon.DiaChi = diachi;
            hoaDon.GhiChu = GhiChu;
            hoaDon.HinhThucThanhToan = "Thanh toán khi nhận hàng";
            hoaDon.HinhThucVanChuyen = "Mặc định";
            hoaDon.HoTen = HoTen;
            hoaDon.MaGioHang = (int)Session["MaGH"];
            hoaDon.NgayTao = DateTime.Now;
            hoaDon.SoDienThoai = sodienthoai;
            hoaDon.TinhTrang = "Đang chờ";
            db.HoaDon.Add(hoaDon);
            var XoaGH = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == hoaDon.MaGioHang).ToList();
            foreach (var item in XoaGH)
            {
                db.Chi_Tiet_Gio_Hang.Remove(item);
            }
            db.SaveChanges();
            return View("Index");
        }

        private bool checkEmail(string email)
        {
            return db.TaiKhoan.Count(t => t.Email == email) > 0;
        }
    }
}
