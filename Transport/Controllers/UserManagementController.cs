using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transport.Models;
using System.Data.Entity;
using System.Net;

namespace Transport.Controllers
{
    public class UserManagementController : Controller
    {
        Models.ApplicationDbContext appContext = new Models.ApplicationDbContext();
        Models.TransportLogEntities transpContext = new Models.TransportLogEntities();

        // GET: UserManagement
        private TransportLogEntities db = new TransportLogEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var role = (from r in appContext.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            var users = appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();


            var userVM = users.Select(user => new Models.UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                StreetNumber = user.StreetNumber,
                City = user.City,
                IDCountry = user.IDCountry,
                Country = (transpContext.Country.Where(x => x.ID == user.IDCountry).Select(x => x.Name).FirstOrDefault().ToString() == null) ? "" : transpContext.Country.Where(x => x.ID == user.IDCountry).Select(x => x.Name).FirstOrDefault().ToString(),
                Phone = user.PhoneNumber,
                RoleName = "User",
                Email = user.Email,
                Active = user.Active
            }).ToList();

            

            var role2 = (from r in appContext.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
            var admins = appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role2.Id)).ToList();


            var adminVM = admins.Select(user => new Models.UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                StreetNumber = user.StreetNumber,
                City = user.City,
                IDCountry = user.IDCountry,
                Country = (transpContext.Country.Where(x => x.ID == user.IDCountry).Select(x => x.Name).FirstOrDefault().ToString() == null) ? "" : transpContext.Country.Where(x => x.ID == user.IDCountry).Select(x => x.Name).FirstOrDefault().ToString(),
                Phone = user.PhoneNumber,
                RoleName = "Admin",
                Email = user.Email,
                Active = user.Active
            }).ToList();




            var model = new Models.GroupedUserViewModel { Users = userVM, Admins = adminVM };

            return View(model);

        }
        //public ActionResult Edit(string ID)
        //{
        //    {
        //        var users = Transport.Models.AspNetUsers.where
        //        ViewBag.IDCountry = new SelectList(transpContext.Country.ToList(), "ID", "Name");
        //        return View();
        //    }

        //}
    }
}