using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test1_WebApp_MVC.Models;
using Test1_WebApp_MVC.Services;
using Test1_WebApp_MVC.DAL;
using Test1_WebApp_MVC.Models.Interfaces;

namespace Test1_WebApp_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserDataService _dataService;

        //NOTE: Conceptually, adding a user is also behaviour that belongs here; however, this simple application has no separate home page so I've left "good enough alone" and made it an action on the home controller

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _dataService = new UserDataService(new XmlUserDataContext(logger));

            ViewData.Reset("btnList");
        }

        public IActionResult Index(string userAction = null)
        {
            var vm = new UserViewModel<List<User>>() { actionName = userAction };

            if ((userAction ?? "ListUsers") == "ListUsers")
            {
                ViewData.Upsert("activeBtn", "btnList");

                vm.userData = _dataService.GetUsers();

                return View("Index", vm);
            }

            return View("Index");
        }

        public ActionResult DeleteUser(User userToDelete)
        {
            if ((userToDelete?.Id ?? -1) < 0)
            {
                var msg = "Unable to delete user, no user ID received";
                _logger.LogWarning(msg);
                ViewData.Upsert("userMsg", msg);
                ViewData.Upsert("userSuccess", false);

                return View("Index");
            }

            _logger.LogInformation("Deleting user " + userToDelete?.Id);
            //TODO: if we had authentication, we would record who performed the delete for traceability/auditing            

            if (_dataService.DeleteUser(userToDelete.Id))
            {
                ViewData.Upsert("userMsg", $"User {userToDelete.Id} deleted");
                ViewData.Upsert("userSuccess", true);
            }
            else
            {
                ViewData.Upsert("userMsg", $"User {userToDelete.Id} could not be deleted");
                ViewData.Upsert("userSuccess", false);
            }

            //TODO: keep users in memory (Viewdata?) to avoid refetching. This is cheap enough for an XML file but not for db calls e.g.
            var users = _dataService.GetUsers();

            return View("Index", users);
        }

        public ActionResult EditUser(User user)
        {
            //TODO: validation

            if (_dataService.UpdateUser(user))
            {
                ViewData.Set(true, "User Updated");
                return View("Index");
            }
            else
            {
                _logger.LogWarning("Could not update user", user);
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
