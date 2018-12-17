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
    public class UserManagementController : Controller
    {
        ApplicationDbContext appContext = new ApplicationDbContext();
        TransportLogEntities transpContext = new TransportLogEntities();

        // GET: UserManagement
        //[Authorize(Roles = "Admin")]

        public ActionResult Index()

        {

            //var usersWithRoles = (from user in transpContext.AspNetUsers

            //                          //from userRole in user.AspNetRoles

            //                      join role in transpContext.AspNetRoles on user.Id equals

            //                      role.Id
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

                                      //IDCountry = user.IDCountry,

                                      Country = user.Country.Name,

                                      Phone = user.PhoneNumber,

                                      RoleName = role.Name,

                                      RoleID = role.Id,

                                      Username = user.UserName,

                                      Email = user.Email,

                                      Active = user.Active.Value

                                  }).ToList();

            return View(usersWithRoles);

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

                                      Country = user.Country.Name,

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
            ViewBag.IDCountry = new SelectList(transpContext.Country, "ID", "Name", usersWithRoles.IDCountry);
            ViewBag.Name = new SelectList(transpContext.AspNetRoles, "Id", "Name", usersWithRoles.RoleName);
            return View(usersWithRoles);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel users)
        {
            if (ModelState.IsValid)
            {

                AspNetUsers dataModel = transpContext.AspNetUsers.Where(x => x.Id == users.ID).First();
                dataModel.Id = users.ID;
                dataModel.FirstName = users.FirstName;
                dataModel.LastName = users.LastName;
                dataModel.Address = users.Address;
                dataModel.StreetNumber = users.StreetNumber;
                dataModel.City = users.City;
                //dataModel.Country = users.Country.Name;
                dataModel.PhoneNumber = users.Phone;
                dataModel.UserName = users.Username;
                dataModel.Email = users.Email;
                dataModel.Active = users.Active;

                //var userRoles = Roles.GetAllRoles();
                //foreach(var role in userRoles)
                //   Roles.RemoveUserFromRole(users.Username,role);


                //Roles.AddUserToRole(users.Username, users.RoleName);


               var userManager= HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                 
                var userRoles = await userManager.GetRolesAsync(users.ID);
                await userManager.RemoveFromRolesAsync(users.ID, userRoles.ToArray());
                await userManager.AddToRoleAsync(users.ID, users.RoleName);
                transpContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }


        //GET: UserManagement3/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var aspUsers = transpContext.AspNetUsers.Find(id);
        //    if (aspUsers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.IDCountry = new SelectList(transpContext.Country, "ID", "Name", aspUsers.IDCountry);
        //    ViewBag.RoleId = new SelectList(transpContext.AspNetRoles, "UserId", "RoleId", aspUsers.AspNetRoles);
        //    //var role3 = (from r in appContext.Roles where r.Name.Contains("User") select r).FirstOrDefault();
        //    //ViewBag.Name = new SelectList(appContext.Roles, appContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role3.Id))).ToList();
        //    return View(aspUsers);
        //}

        //// POST: UserManagement3/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(AspNetUsers aspUsers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        transpContext.Entry(aspUsers).State = EntityState.Modified;
        //        transpContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.IDCountry = new SelectList(transpContext.Country, "ID", "Name", aspUsers.IDCountry);
        //    ViewBag.Name = new SelectList(transpContext.AspNetRoles, "Id", "Name", aspUsers.AspNetRoles);
        //    return View(aspUsers);
        //}
    }
}