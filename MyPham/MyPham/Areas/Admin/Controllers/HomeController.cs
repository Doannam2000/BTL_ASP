using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLySanPham()
        {
            return View();
        }
        public ActionResult QuanLyDanhMuc()
        {
            return View();
        } public ActionResult QuanLyHoaDon()
        {
            return View();
        }   
        public ActionResult QuanLyTaiKhoan()
        {
            return View();
        }  
        public ActionResult QuanLyPhanQuyen()
        {
            return View();
        }
        public ActionResult CapNhatSanPham()
        {
            return View();
        }       
        public ActionResult ThemSanPham()
        {
            return View();
        }
        public ActionResult ChiTietSanPham()
        {
            return View();
        }
        public ActionResult ThemDanhMuc()
        {
            return View();
        }
        public ActionResult CapNhatDanhMuc()
        {
            return View();
        }
        public ActionResult ThemTaiKhoan()
        {
            return View();
        }
        public ActionResult CapNhatTaiKhoan()
        {
            return View();
        }
        public ActionResult ThemPhanQuyen()
        {
            return View();
        }
        public ActionResult CauHinhQuyen()
        {
            return View();
        }
    }
}