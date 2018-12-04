using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Transport.Controllers
{
    public class DockController : Controller
    {
        // GET: Docks
        public ActionResult Index()
        {
            return View(); 
        }
    }
}