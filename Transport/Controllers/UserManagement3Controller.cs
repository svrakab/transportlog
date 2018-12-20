using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Transport.Models;

namespace Transport.Controllers
{
    public class UserManagement3Controller : Controller
    {
        private TransportLogEntities db = new TransportLogEntities();

        Models.ApplicationDbContext appContext = new Models.ApplicationDbContext();
        Models.TransportLogEntities transpContext = new Models.TransportLogEntities();

        // GET: UserManagement3
        public ActionResult Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.Country);
            return View(aspNetUsers.ToList());
        }

        // GET: UserManagement3/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: UserManagement3/Create
        public ActionResult Create()
        {
            ViewBag.IDCountry = new SelectList(db.Country, "ID", "Name");
            return View();
        }

        // POST: UserManagement3/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,/*EmailConfirmed,PasswordHash,SecurityStamp,*/PhoneNumber,/*PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,*/UserName,FirstName,LastName,Address,StreetNumber,City,IDCountry,Active")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return Json(new { success = true, aspNetUsers = aspNetUsers });
                //return RedirectToAction("Index");
            }
            else
            {
                //var err = ModelState.Values.SelectMany(m => m.Errors)
                //                 .Select(e => e.ErrorMessage)
                //                 .ToList();

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

            //ViewBag.IDCountry = new SelectList(db.Country, "ID", "Name", aspNetUsers.IDCountry);
            //return View(aspNetUsers);
        }

        // GET: UserManagement3/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCountry = new SelectList(db.Country, "ID", "Name", aspNetUsers.IDCountry);
            return View(aspNetUsers);
        }

        // POST: UserManagement3/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,LastName,Address,StreetNumber,City,IDCountry,Active")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCountry = new SelectList(db.Country, "ID", "Name", aspNetUsers.IDCountry);
            return View(aspNetUsers);
        }

        // GET: UserManagement3/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: UserManagement3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
