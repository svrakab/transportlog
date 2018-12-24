using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Transport.Models;


namespace Transport.Controllers
{
    public class HomeController : Controller
    {
        Models.TransportLogEntities transpContext = new Models.TransportLogEntities();

        //public JsonResult GetLoads()
        //{
        //    List<Load> loadGeneric = transpContext.Load.ToList();

        //var json = JsonConvert.SerializeObject(Loads);


        //    return Json(loadGeneric, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetDocks()
        {

            List<Generic> Docks = transpContext.Dock.Select(st => new Generic
            {
                Id = st.ID,
                Text = st.Name
            }).ToList();


            return Json(Docks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoads()
        {
            List<LoadGeneric2> Loads = transpContext.Load.Select(st => new LoadGeneric2
            {
                LoadNumber = st.LoadNumber,
                NumberOfPallets = st.NumberOfPallets,
                PlannedTime = st.PlannedTime,
                ArivalTime = st.ArivalTime,
                DockOn = st.DockOn,
                DockOff = st.DockOff,
                DepartureTime = st.DepartureTime,
                //PlannedTime = JsonConvert.SerializeObject(st.PlannedTime) ?? "",
                //ArivalTime = JsonConvert.SerializeObject(st.ArivalTime) ?? "",
                //DockOn = JsonConvert.SerializeObject(st.DockOn) ?? "",
                //DockOff = JsonConvert.SerializeObject(st.DockOff) ?? "",
                //DepartureTime = JsonConvert.SerializeObject(st.DepartureTime) ?? "",
                IDStatus = st.IDStatus,
                IDLoadType = st.IDLoadType,
                IDCustomers = st.IDCustomers,
                IDDocks = st.IDDocks,
            }).ToList();

            var json = JsonConvert.SerializeObject(Loads);

            //        return new CustomJsonResult { Data = new {
            //            Loads = transpContext.Load.Select(st => new LoadGeneric
            //            {
            //                LoadNumber = st.LoadNumber,
            //                NumberOfPallets = st.NumberOfPallets,
            //                startDate = st.PlannedTime,
            //                ArivalTime = st.ArivalTime,
            //                DockOn = st.DockOn,
            //                DockOff = st.DockOff,
            //                endDate = st.DepartureTime,
            //                IDStatus = st.IDStatus,
            //                IDLoadType = st.IDLoadType,
            //                IDCustomers = st.IDCustomers,
            //                IDDocks = st.IDDocks,
            //            }).ToList()
            //    }
            //};
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropdowns()
        {
                Dropdowns dropdowns = new Dropdowns()
                {
                    Statuses = transpContext.Status.Select(st => new Generic
                    {
                        Id = st.ID,
                        Text = st.Name
                    }).ToList(),
                    Customers = transpContext.Customer.Select(st => new Generic
                    {
                        Id = st.ID,
                        Text = st.LastName
                    }).ToList(),
                    LoadTypes = transpContext.LoadType.Select(st => new Generic
                    {
                        Id = st.ID,
                        Text = st.Name,
                        Value = st.Time
                    }).ToList()
                };
            
                return Json(dropdowns,JsonRequestBehavior.AllowGet);
        }

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
