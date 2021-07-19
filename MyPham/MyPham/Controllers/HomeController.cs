using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Controllers
{
    public class HomeController : Controller
    {
        MyPhamDB db = new MyPhamDB();
        public ActionResult Index()
        {
            List<SanPham> SpSon = new List<SanPham>();
            SpSon = db.SanPhams.Where(h=>h.MaDM == 4).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            List<SanPham> SpDuongDa = new List<SanPham>();
            SpDuongDa = db.SanPhams.Where(h => h.MaDM == 1).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            List<SanPham> SpTrangDiem = new List<SanPham>();
            SpTrangDiem = db.SanPhams.Where(h => h.MaDM == 2).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            ViewBag.TrangDiem = SpTrangDiem;
            ViewBag.Son = SpSon;
            ViewBag.DuongDa = SpDuongDa;

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Trang đăng nhập.";
            return View();
        }
        public ActionResult DangKy()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult QuenMatKhau()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult ThongTinTaiKhoan()
        {
            /*try
            {
                var f = dk.ImageFile;
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    var UploadPath = Server.MapPath("~/UserImage/" + FileName);
                    dk.ImagePath = "~/UserImage/" + FileName;
                    f.SaveAs(UploadPath);

                }
                return RedirectToAction("Details", dk);
            }
            catch
            {
                return View();
            }*/
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}