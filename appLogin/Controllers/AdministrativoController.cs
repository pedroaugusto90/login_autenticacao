﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appLogin.Controllers
{
    public class AdministrativoController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}