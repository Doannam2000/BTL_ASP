using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using MyPham.Models;

namespace MyPham.Controllers
{
    public class TaiKhoanController : Controller
    {
        MyPhamDB db = new MyPhamDB();

        // GET: TaiKhoan
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string email, string matkhau)
        {
            if (ModelState.IsValid)
            {
                var user = db.TaiKhoan.Where(u => u.Email.Equals(email) && u.MatKhau.Equals(matkhau)).ToList();
                if (user.Count() > 0)
                {
                    //add session
                    Session["Email"] = user.FirstOrDefault().Email;
                    Session["idUser"] = user.FirstOrDefault().MaTK;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.error = "Đăng nhập không thành công!";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        //
        //public ActionResult DangKy([Bind(Include = "MaTK,Email,MatKhau,LoaiTaiKhoan,HoTen,DiaChi,SoDienThoai,Anh,TinhTrang,MaQuyen")] TaiKhoan taikhoan)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TaiKhoan.Add(taikhoan);
        //        db.SaveChanges();
        //        return RedirectToAction("DangNhap");
        //    }
        //    return View(taikhoan);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "DangKyCaptcha", "Mã xác nhận không đúng !")]
        public ActionResult DangKy(TaiKhoan dangky)
        {
            if (ModelState.IsValid)
            {
                if (checkEmail(dangky.Email))
                {
                    ViewBag.error = "Email đã tồn tại";
                }
                else
                {
                    var user = new TaiKhoan();
                    user.Email = dangky.Email;
                    user.MatKhau = dangky.MatKhau;
                    user.HoTen = dangky.HoTen;
                    user.LoaiTaiKhoan = "KhachHang";
                    user.DiaChi = dangky.DiaChi;
                    user.SoDienThoai = dangky.SoDienThoai;
                    user.TinhTrang = true;
                    user.MaQuyen = 7;
                    db.TaiKhoan.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return View(dangky);
        }

        private bool checkEmail(string email)
        {
            return db.TaiKhoan.Count(t => t.Email == email) > 0;
        }

        public ActionResult ThongTinTaiKhoan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan nguoidung = db.TaiKhoan.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        public ActionResult SuaTaiKhoan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        // POST: tk/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTaiKhoan([Bind(Include = "MaTK,Email,MatKhau,LoaiTaiKhoan,HoTen,DiaChi,SoDienThoai,Anh,TinhTrang,MaQuyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }
    }
}