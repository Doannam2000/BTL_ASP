﻿using System;
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
       public ActionResult QuanLyPhanQuyen()
        {
            return View();
        }
        public ActionResult CauHinhQuyen()
        {
            return View();
        }
        public ActionResult ThemPhanQuyen()
        {
            return View();
        }
    }
}