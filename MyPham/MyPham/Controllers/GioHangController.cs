﻿using MyPham.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
                                var update = db.Chi_Tiet_Gio_Hang.Where(s=>s.MaGioHang==MaGH).FirstOrDefault();
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
                        double gia = (Convert.ToDouble(item.sanPham.Gia) - Convert.ToDouble(Convert.ToDouble(item.sanPham.Gia) * (item.sanPham.GiamGia.HasValue ? item.sanPham.GiamGia.Value : 0)));
                        gia = Math.Round(gia / 1000) * 1000;
                        chiTiet.GiaSP = Convert.ToDecimal(gia);
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
                    double gia = (Convert.ToDouble(item.sanPham.Gia) - Convert.ToDouble(Convert.ToDouble(item.sanPham.Gia) * (item.sanPham.GiamGia.HasValue ? item.sanPham.GiamGia.Value : 0)));
                    gia = Math.Round(gia / 1000) * 1000;
                    chiTiet.GiaSP = Convert.ToDecimal(gia); 
                    db.Chi_Tiet_Gio_Hang.Add(chiTiet);
                    db.SaveChanges();
                }
                Session["GioHang"] = list;
            }
            UpdateSL();
            String thongbao = "Đã thêm " + sanpham.TenSP + " số lượng " + SoLuong + " vào giỏ hàng !";
            return RedirectToAction("SanPham", "SanPham", new { id = MaSP , ThongBao = thongbao});
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
                    Session["GioHang"] = giohang;
                    UpdateSL();
                    return RedirectToAction("Index");
                }
                itemSua.soLuong = SoLuong;
                Session["GioHang"] = giohang;
                if (MaGH != -1)
                {
                    var update = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == MaGH && s.MaSP == MaSP).FirstOrDefault();
                    update.SoLuongMua = SoLuong;
                    db.SaveChanges();
                }
            }
            UpdateSL(); 
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
                        if(item.MaSP == MaSP)
                        {
                            db.Chi_Tiet_Gio_Hang.Remove(item);
                            db.SaveChanges();
                        }
                    }
                }
                UpdateSL();
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
            UpdateSL();
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan()
        {
            var list = new List<Gio>();
            var gioHang = Session["GioHang"];
            if (gioHang != null)
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
            var list = new List<Gio>();
            var gioH= Session["GioHang"];
            if (gioH != null)
            {
                list = (List<Gio>)gioH;
            }
            if (Session["idUser"] == null) // check đăng nhập
            {
                if (checkEmail(email)) // check mail đã tồn tại trong hệ thống chưa
                {
                    var user = db.TaiKhoan.Where(u => u.Email.Equals(email) && u.MatKhau.Equals(matkhau)).ToList(); // check mật khẩu
                    if (user.Count() > 0)
                    {
                        Session["Email"] = user.FirstOrDefault().Email;
                        Session["HoTen"] = user.FirstOrDefault().HoTen;
                        Session["Anh"] = user.FirstOrDefault().Anh;
                        Session["idUser"] = user.FirstOrDefault().MaTK;
                    }
                    else
                    {
                        ViewBag.error = "Email đã tồn tại trong tài khoản của hệ thống !!!\nMật khẩu không đúng";
                        return View("ThanhToan");
                    }
                } 
                else // tạo mới
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
                foreach (var item in list)
                {
                    Chi_Tiet_Gio_Hang chi = new Chi_Tiet_Gio_Hang();
                    chi.MaGioHang = gio.MaGioHang;
                    chi.MaSP = item.sanPham.MaSP;
                    chi.SoLuongMua = item.soLuong;
                    double gia = (Convert.ToDouble(item.sanPham.Gia) - Convert.ToDouble(Convert.ToDouble(item.sanPham.Gia) * (item.sanPham.GiamGia.HasValue ? item.sanPham.GiamGia.Value : 0)));
                    gia = Math.Round(gia / 1000) * 1000;
                    chi.GiaSP = Convert.ToDecimal(gia);
                    db.Chi_Tiet_Gio_Hang.Add(chi);
                    db.SaveChanges();
                }
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
            db.SaveChanges();


            foreach (var item in list)
            {
                var sanPham = db.SanPham.Where(s => s.MaSP == item.sanPham.MaSP).FirstOrDefault();
                sanPham.SoLuongTon = sanPham.SoLuongTon - item.soLuong;
                db.SaveChanges();
            }


            GioHang gioHang = new GioHang();
            gioHang.MaTK = (int)Session["idUser"];
            db.GioHang.Add(gioHang);
            db.SaveChanges();

            GioHang gio1 = (GioHang)checkGH(gioHang.MaTK);
            int MaGH = gio1.MaGioHang;
            Session["MaGH"] = MaGH;
            Session["GioHang"] = null;
            Session["SoLuong"] = null;
            ViewBag.HoaDon = hoaDon;
            var HoaDon = db.HoaDon.Where(s => s.MaGioHang == hoaDon.MaGioHang).FirstOrDefault();
            int mahd = HoaDon.MaHD;
            return RedirectToAction("ChiTiet",new { id = mahd });
        }

        private GioHang checkGH(int MaTK)
        {
            var GH = db.GioHang.Where(s => s.MaTK == MaTK);
            foreach (var item in GH)
            {
                var listDH = db.HoaDon.Where(h => h.MaGioHang == item.MaGioHang).FirstOrDefault();
                if(listDH==null)
                {
                    return item;
                }    
            }
            return null;
        }
        private bool checkEmail(string email)
        {
            return db.TaiKhoan.Count(t => t.Email == email) > 0;
        }

        public ActionResult XemHoaDon(int id,int? page)
        {
            List<HoaDon> hd = new List<HoaDon>();
            var giohang = db.GioHang.Where(g => g.MaTK == id);
            if(giohang!=null)
            {
                foreach (var item in giohang)
                {
                    var hoaDon = db.HoaDon.Where(h => h.MaGioHang == item.MaGioHang).FirstOrDefault();
                    if(hoaDon!=null)
                    {
                        hd.Add(hoaDon);
                    }    
                }
            }
            if(hd.Count>0)
            {
                int pageSize = 7;
                int pageNumber = (page ?? 1);
                return View(hd.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Bạn chưa mua hàng lần nào !!!";
            return View();
        }

        public ActionResult ChiTiet(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDon.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            var chiTiet = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == hoaDon.MaGioHang).ToList();
            List<SanPham> sanpham = new List<SanPham>();
            foreach (var item in chiTiet)
            {
                SanPham x = new SanPham();
                x.MaSP = item.MaSP;
                x.Gia = item.GiaSP;
                var sp = db.SanPham.Find(item.MaSP);
                x.AnhSP = sp.AnhSP;
                x.TenSP = sp.TenSP;
                x.SoLuongTon = item.SoLuongMua;
                sanpham.Add(x);
            }
            ViewBag.SanPham = sanpham;
            return View(hoaDon);
        }
        private void UpdateSL()
        {
            var list = (List<Gio>)Session["GioHang"];
            if (list == null)
            {
                Session["SoLuong"] = null;
            }
            else
            {
                Session["SoLuong"] = list.Count();
            }
        }
        public ActionResult XacNhanHang(int id)
        {
            var hoaDon = db.HoaDon.Find(id);
            hoaDon.TinhTrang = "Đã nhận";
            db.SaveChanges();
            int matk = (int)Session["idUser"];
            return RedirectToAction("XemHoaDon", new { id = matk });
        }
        public ActionResult HuyDonHang(int id)
        {
            var hoaDon = db.HoaDon.Find(id);
            hoaDon.TinhTrang = "Đơn hàng đã hủy";
            db.SaveChanges();
            int matk = (int)Session["idUser"];
            return RedirectToAction("XemHoaDon", new { id = matk });
        }
    }
}
