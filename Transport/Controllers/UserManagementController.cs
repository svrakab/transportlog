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
        Models.ApplicationDbContext appContext = new Models.ApplicationDbContext();
        Models.TransportLogEntities transpContext = new Models.TransportLogEntities();

        // GET: UserManagement
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var role = (from r in appContext.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            var users = appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var userVM = users.Select(user => new Models.UserViewModel
            {
                Phone = user.PhoneNumber,
                RoleName = "User"
            }).ToList();


            var role2 = (from r in appContext.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
            var admins = appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role2.Id)).ToList();
            

            var adminVM = admins.Select(user => new Models.UserViewModel
            {
                Phone = user.PhoneNumber,
                RoleName = "Admin"
            }).ToList();




            var model = new Models.GroupedUserViewModel { Users = userVM, Admins = adminVM };

            return View(model);




            using (var DBContext = new TransportLogEntities())
            {
                List<AspNetUsers> rolesList = DBContext.AspNetUsers.ToList();

                ViewBag.Roles = new SelectList(DBContext.AspNetRoles.ToList(), "Id", "Name");

                return View(rolesList);
            }
        }
    }
}