using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPham.Models;

namespace MyPham.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private MyPhamDB db = new MyPhamDB();

        // GET: Admin/SanPhams
        public ActionResult Index()
        {
            var sanPham = db.SanPham.Include(s => s.DanhMucSP);
            return View(sanPham.ToList());
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,Gia,ThuongHieu,SoLuongTon,XuatXu,DungTich,HangSX,TrongLuong,ChatLieu,CongDung,ThanhPhan,HuongDanSD,QuyCachDongGoi,GiamGia,MaDM,AnhSP")] SanPham sanPham)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    sanPham.AnhSP = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/images/SanPham/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Gia,ThuongHieu,SoLuongTon,XuatXu,DungTich,HangSX,TrongLuong,ChatLieu,CongDung,ThanhPhan,HuongDanSD,QuyCachDongGoi,GiamGia,MaDM,AnhSP")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/images/SanPham/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    //else
                    //{
                    //    string UploadPath = Server.MapPath("~/wwwroot/image/" + sanPham.AnhSP);
                    //}
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult DeleteConfirmedCustom(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
