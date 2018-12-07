using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Models;


namespace Transport.Controllers
{
    public class HomeController : Controller
    {
        Models.TransportLogEntities transpContext = new Models.TransportLogEntities();

        public ActionResult Index()
        {
            Models.LoadViewModel model = new Models.LoadViewModel();

            List<Models.Load> loadList = transpContext.Load.ToList();
            List<Models.Dock> dockList = transpContext.Dock.ToList();

            model.LoadList = loadList;
            model.DockList = dockList;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoadViewModel model)
        {

            return View();
        }
    }


}
