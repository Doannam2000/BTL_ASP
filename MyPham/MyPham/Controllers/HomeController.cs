using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult SanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.Find(int.Parse(id));
            if (sp == null)
            {
                return HttpNotFound();
            }
            int madm = db.SanPhams.Find(int.Parse(id)).MaDM;
            ViewBag.ma = madm;

            List<SanPham> Sp = new List<SanPham>();
            Sp = db.SanPhams.Where(h => h.MaDM.Equals(madm)).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            ViewBag.sp = Sp;


            List<DanhMucSP> s = new List<DanhMucSP>();
            s = db.DanhMucSPs.Where(h => h.MaDM == madm).ToList();
            ViewBag.TenDM = s[0].TenDM;

            return View(sp);
        }

        public ActionResult XemSanPhamTheoDanhMuc( string id )
        {
            List<SanPham> sanpham = new List<SanPham>();
            if(id == null)
            {
               
            } else
            {
                sanpham = db.SanPhams.Where(s => s.MaDM.ToString().Equals(id)).Select(s => s).ToList();

            }
            return View(sanpham);
        }

        public ActionResult GioHang()
        {
            return View();
        }
    }
}