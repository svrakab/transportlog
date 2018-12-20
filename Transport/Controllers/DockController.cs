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
    [Authorize(Roles = "Admin")]
    public class DockController : Controller
    {
        private TransportLogEntities db = new TransportLogEntities();

        // GET: Docks
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Dock.ToList());
        }

        // GET: Docks/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dock dock = db.Dock.Find(id);
            if (dock == null)
            {
                return HttpNotFound();
            }
            return View(dock);
        }

        // GET: Docks/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Docks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Name,DockNumber,Description")] Dock dock)
        {
            if (ModelState.IsValid)
            {
                db.Dock.Add(dock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dock);
        }

        // GET: Docks/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dock dock = db.Dock.Find(id);
            if (dock == null)
            {
                return HttpNotFound();
            }
            return View(dock);
        }

        // POST: Docks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,DockNumber,Description")] Dock dock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dock);
        }

        // GET: Docks/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dock dock = db.Dock.Find(id);
            if (dock == null)
            {
                return HttpNotFound();
            }
            return View(dock);
        }

        // POST: Docks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Dock dock = db.Dock.Find(id);
            db.Dock.Remove(dock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
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
