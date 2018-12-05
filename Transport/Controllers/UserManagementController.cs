using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Models;
using System.Data.Entity;

namespace Transport.Controllers
{
    public class UserManagementController : Controller
    {

        // GET: UserManagement
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            using (var DBContext = new TransportLogEntities())
            {
                List<AspNetUsers> users = DBContext.AspNetUsers.ToList();
                
                ViewBag.Roles = new SelectList(DBContext.AspNetRoles.ToList(), "Id", "Name");

                return View(users);
            }
        }
    }
}