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
            Models.GroupHomeViewModel model = new Models.GroupHomeViewModel();

            List<Models.Load> loadList = transpContext.Load.ToList();
            List<Models.Dock> dockList = transpContext.Dock.ToList();

            model.LoadList = loadList;
            model.DockList = dockList;

            return View(model);
        }

        public ActionResult Create()
        {
            using (var DBContext = new TransportLogEntities())
            {
                ViewBag.StatusList = new SelectList(DBContext.Status.ToList(), "ID", "Name");

                ViewBag.CustomerList = new SelectList(DBContext.Customer.ToList(), "ID", "FirstName");

                ViewBag.DockList = new SelectList(DBContext.Dock.ToList(), "ID", "Name");

                ViewBag.LoadTypeList = new SelectList(DBContext.LoadType.ToList(), "ID", "Name");


                return View();
            }
        }

        [HttpPost]
        public JsonResult Create(Load load)
        {
            using (var DBContext = new TransportLogEntities())
            {
                if (ModelState.IsValid)
                {


                    DBContext.Load.Add(load);
                    DBContext.SaveChanges();

                    return Json(new { success = true, artikli = load });

                }
                else
                {
                    var errorList = ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );
                    return Json(new
                    {
                        success = false,
                        error = errorList.Where(c => c.Value.Count() > 0)
                    });
                }
            }
        }


        [HttpPost]
        public JsonResult Edit(Load load)
        {


            using (var DBContext = new TransportLogEntities())
            {
                if (ModelState.IsValid)
                {
                    var loadN = DBContext.Load.Where(x => x.LoadNumber == load.LoadNumber).FirstOrDefault();
                    if (loadN != null)
                    {
                        loadN.ArivalTime = load.ArivalTime;
                        loadN.DockOn = load.DockOn;
                        loadN.DockOff = load.DockOff;
                    }
                    DBContext.SaveChanges();

                    return Json(new { success = true, artikli = load });
                    
                }
                else
                {
                    var errorList = ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );
                    return Json(new
                    {
                        success = false,
                        error = errorList.Where(c => c.Value.Count() > 0)
                    });
                }
            }
        }


    
    
       


        [HttpPost]
        public ActionResult Index(GroupHomeViewModel model)
        {

            return View();
        }
    }


}
