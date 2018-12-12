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
    public class UserManagementViewsController : Controller
    {
        private TransportLogEntities db = new TransportLogEntities();

        // GET: UserManagementViews
        public ActionResult Index()
        {
            return View(db.UserManagementView.ToList());
        }

        // GET: UserManagementViews/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserManagementView userManagementView = db.UserManagementView.Find(id);
            if (userManagementView == null)
            {
                return HttpNotFound();
            }
            return View(userManagementView);
        }

        // GET: UserManagementViews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserManagementViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Address,StreetNumber,City,Active,Name,Roles,Id")] UserManagementView userManagementView)
        {
            if (ModelState.IsValid)
            {
                db.UserManagementView.Add(userManagementView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userManagementView);
        }

        // GET: UserManagementViews/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserManagementView userManagementView = db.UserManagementView.Find(id);
            if (userManagementView == null)
            {
                return HttpNotFound();
            }
            return View(userManagementView);
        }

        // POST: UserManagementViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Address,StreetNumber,City,Active,Name,Roles,Id")] UserManagementView userManagementView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userManagementView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userManagementView);
        }

        // GET: UserManagementViews/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserManagementView userManagementView = db.UserManagementView.Find(id);
            if (userManagementView == null)
            {
                return HttpNotFound();
            }
            return View(userManagementView);
        }

        // POST: UserManagementViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserManagementView userManagementView = db.UserManagementView.Find(id);
            db.UserManagementView.Remove(userManagementView);
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
