﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Transport.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }
    }
}