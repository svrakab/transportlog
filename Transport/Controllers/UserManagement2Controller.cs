//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Transport.Controllers
//{
//    //public class UserManagement2Controller : Controller
//    //{
//        //Models.ApplicationDbContext context = new Models.ApplicationDbContext();

//        //// GET: UserManagement2
//        //public ActionResult Index()
//        //{
//            //var role = (from r in context.Roles where r.Name.Contains("User") select r).FirstOrDefault();
//            //var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

//            //var userVM = users.Select(user => new Models.UserViewModel
//            //{
//            //    Username = user.UserName,
//            //    Email = user.Email,
//            //    RoleName = "User"
//            //}).ToList();


//            //var role2 = (from r in context.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
//            //var admins = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role2.Id)).ToList();

//            //var adminVM = admins.Select(user => new Models.UserViewModel
//            //{
//            //    Username = user.UserName,
//            //    Email = user.Email,
//            //    RoleName = "Admin"
//            //}).ToList();


//            //var model = new Models.GroupedUserViewModel { Users = userVM, Admins = adminVM };
////            return View(/*model*/);

////        }
////    }
////}
/////var userRoles = Roles.GetAllRoles();
//foreach(var role in userRoles)
//   Roles.RemoveUserFromRole(users.Username,role);


//Roles.AddUserToRole(users.Username, users.RoleName);