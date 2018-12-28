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
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace Transport.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        ApplicationDbContext appContext = new ApplicationDbContext();
        TransportLogEntities transpContext = new TransportLogEntities();

        // GET: UserManagement

        public ActionResult Index()

        {
            var usersWithRoles = (from user in transpContext.AspNetUsers

                                  from userRole in user.AspNetRoles

                                  join role in transpContext.AspNetRoles on userRole.Id equals

                                  role.Id

                                  select new UserViewModel()

                                  {

                                      ID = user.Id,

                                      FirstName = user.FirstName,

                                      LastName = user.LastName,

                                      Address = user.Address,

                                      StreetNumber = user.StreetNumber,

                                      City = user.City,

                                      Country = user.Country.Name, 

                                      Phone = user.PhoneNumber,

                                      RoleName = role.Name,

                                      Username = user.UserName,

                                      Email = user.Email,

                                      Active = user.Active.Value

                                  }).Where(x => x.Active == true);

            return View(usersWithRoles.ToList());

        }


        public ActionResult Edit(string id)

        {
            

            var usersWithRoles = (from user in transpContext.AspNetUsers

                                  from userRole in user.AspNetRoles

                                  join role in transpContext.AspNetRoles on userRole.Id equals

                                  role.Id

                                  select new UserViewModel

                                  {

                                      ID = user.Id,

                                      FirstName = user.FirstName,

                                      LastName = user.LastName,

                                      Address = user.Address,

                                      StreetNumber = user.StreetNumber,

                                      City = user.City,

                                      IDCountry = user.IDCountry,

                                      Phone = user.PhoneNumber,

                                      RoleName = role.Name,

                                      Username = user.UserName,

                                      Email = user.Email,

                                      Active = user.Active.Value

                                  }).Where(x => x.ID == id).FirstOrDefault();

            if (usersWithRoles == null)

            {

                return HttpNotFound();

            }
            ViewBag.Country = new SelectList(transpContext.Country.ToList(), "ID", "Name");
            ViewBag.Roles = new SelectList(transpContext.AspNetRoles.ToList(), "Name", "Name");

            return View(usersWithRoles);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel users)
        {
            ViewBag.Country = new SelectList(transpContext.Country.ToList(), "ID", "Name");
            ViewBag.Roles = new SelectList(transpContext.AspNetRoles.ToList(), "Name", "Name");

            if (ModelState.IsValid)
            {

                AspNetUsers dataModel = transpContext.AspNetUsers.Where(x => x.Id == users.ID).First();
                dataModel.Id = users.ID;
                dataModel.FirstName = users.FirstName;
                dataModel.LastName = users.LastName;
                dataModel.Address = users.Address;
                dataModel.StreetNumber = users.StreetNumber;
                dataModel.City = users.City;
                dataModel.IDCountry = users.IDCountry;
                dataModel.PhoneNumber = users.Phone;
                dataModel.UserName = users.Username;
                dataModel.Email = users.Email;
                dataModel.Active = users.Active;

                var userManager= HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                 
                var userRoles = await userManager.GetRolesAsync(users.ID);
                await userManager.RemoveFromRolesAsync(users.ID, userRoles.ToArray());
                await userManager.AddToRoleAsync(users.ID, users.RoleName);
                transpContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Deleted(string id)
        {
            var usersWithRoles = (from user in transpContext.AspNetUsers

                                  from userRole in user.AspNetRoles

                                  join role in transpContext.AspNetRoles on userRole.Id equals

                                  role.Id

                                  select new UserViewModel()

                                  {

                                      ID = user.Id,

                                      FirstName = user.FirstName,

                                      LastName = user.LastName,

                                      Address = user.Address,

                                      StreetNumber = user.StreetNumber,

                                      City = user.City,

                                      Country = user.Country.Name,

                                      Phone = user.PhoneNumber,

                                      RoleName = role.Name,

                                      Username = user.UserName,

                                      Email = user.Email,

                                      Active = user.Active.Value

                                  }).Where(x => x.Active == false);

            return View(usersWithRoles.ToList());
        }

        
    }
}