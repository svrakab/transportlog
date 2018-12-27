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
            List<LoadGeneric> Loads = transpContext.Load.Where(d => d.Deleted == false).Select(st => new LoadGeneric
            {
                LoadNumber = st.LoadNumber,
                NumberOfPallets = st.NumberOfPallets,
                PlannedTime = st.PlannedTime,
                ArivalTime = st.ArivalTime,
                DockOn = st.DockOn,
                DockOff = st.DockOff,
                DepartureTime = st.DepartureTime,
                IDStatus = st.IDStatus,
                IDLoadType = st.IDLoadType,
                IDCustomers = st.IDCustomers,
                IDDocks = st.IDDocks,
                EndDate = st.EndDate,
                Deleted = st.Deleted.Value,
            }).ToList();
            
            foreach (var mc in Loads)
            {
                if (mc.EndDate == null)
                {
                    LoadType LoadTypes = transpContext.LoadType.Where(x => x.ID == mc.IDLoadType).FirstOrDefault();
                    int minutes = LoadTypes.Time;
                    mc.EndDate = mc.PlannedTime.AddMinutes(minutes);
                }
            }

            var json = JsonConvert.SerializeObject(Loads);
            
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropdowns()
        {
                Dropdowns dropdowns = new Dropdowns()
                {
                    Statuses = transpContext.Status.Select(st => new Generic
                    {
                        Id = st.ID,
                        Text = st.Name,
                        Color = st.ColorHex
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
                    load.Deleted = false;

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
                        loadN.PlannedTime = load.PlannedTime;
                        loadN.NumberOfPallets = load.NumberOfPallets;
                        loadN.ArivalTime = load.ArivalTime;
                        loadN.DockOn = load.DockOn;
                        loadN.DockOff = load.DockOff;
                        loadN.DepartureTime = load.DepartureTime;
                        loadN.IDStatus = load.IDStatus;
                        loadN.IDLoadType = load.IDLoadType;
                        loadN.IDCustomers = load.IDCustomers;
                        loadN.IDDocks = load.IDDocks;
                        loadN.EndDate = load.EndDate;
                        loadN.Deleted = load.Deleted;
                    }
                    
                    if (loadN.ArivalTime == null)
                    {
                        loadN.IDStatus = 1;
                    }
                    else if (loadN.DepartureTime != null)
                    {
                        loadN.IDStatus = 3;
                    }
                    else if(loadN.ArivalTime!=null || loadN.DockOn!=null || loadN.DockOff!=null)
                    {
                        loadN.IDStatus = 2;
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
    }
}
