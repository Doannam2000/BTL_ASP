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
            if(Session["MaGH"] !=null)
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

     
    }
}
