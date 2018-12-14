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
        ApplicationDbContext appContext = new ApplicationDbContext();
        TransportLogEntities transpContext = new TransportLogEntities();

        // GET: UserManagement
        //private TransportLogEntities db = new TransportLogEntities();

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
                Id = user.Id,
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
        //// GET: Users/Edit/5

        //public ActionResult Edit(string id)

        //{

        //    if (id == null)

        //    {

        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    }

        //    var aspUsers = transpContext.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();

        //    ViewBag.IDCountry = new SelectList(transpContext.Country.ToList(), "ID", "Name", aspUsers.IDCountry);

        //    ViewBag.Name = new SelectList(appContext.Roles.ToList(), "Name", "Name", aspUsers.AspNetRoles);

        //    return View(aspUsers);

        //}

        //// POST: Users/Edit/5

        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]

        //[ValidateAntiForgeryToken]

        //public ActionResult Edit(AspNetUsers AsNeUs)

        //{

        //    if (ModelState.IsValid)

        //    {

        //        var aspUsers = transpContext.AspNetUsers.Where(x => x.Id == AsNeUs.Id).FirstOrDefault();

        //        if (aspUsers != null)

        //        {

        //            aspUsers = AsNeUs;

        //        }

        //        transpContext.SaveChanges();

        //        return RedirectToAction("Index");

        //    }

        //    return View(AsNeUs);

        //}

        // GET: UserManagement3/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspUsers = transpContext.AspNetUsers.Find(id);
            if (aspUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCountry = new SelectList(transpContext.Country, "ID", "Name", aspUsers.IDCountry);
            ViewBag.Name = new SelectList(transpContext.AspNetRoles, "Id", "Name", aspUsers.AspNetRoles);
            //var role3 = (from r in appContext.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            //ViewBag.Name = new SelectList(appContext.Roles, appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role3.Id))).ToList();
            return View(aspUsers);
        }

        // POST: UserManagement3/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetUsers aspUsers)
        {
            if (ModelState.IsValid)
            {
                transpContext.Entry(aspUsers).State = EntityState.Modified;
                transpContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCountry = new SelectList(transpContext.Country, "ID", "Name", aspUsers.IDCountry);
            ViewBag.Name = new SelectList(transpContext.AspNetRoles, "Id", "Name", aspUsers.AspNetRoles);
            return View(aspUsers);
        }
    }
}