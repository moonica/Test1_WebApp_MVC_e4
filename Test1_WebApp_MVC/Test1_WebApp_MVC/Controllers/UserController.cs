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

        public IActionResult Index()
        {
            var vm = new UserViewModel<List<User>>(this.GetType().ToString()) { ActionName = "ListUsers", ControllerName = "User" };

            ViewData.Upsert("activeBtn", "btnList");

            vm.UserData = _dataService.GetUsers();

            return View("Index", vm);
        }

        public ActionResult DeleteUser(User user)
        {
            if ((user?.Id ?? -1) < 0)
            {
                var msg = "Unable to delete user, no user ID received";
                _logger.LogWarning(msg);
                ViewData.Upsert("userMsg", msg);
                ViewData.Upsert("userSuccess", false);

                return View("Index");
            }

            _logger.LogInformation("Deleting user " + user?.Id);
            //TODO: if we had authentication, we would record who performed the delete for traceability/auditing            

            if (_dataService.DeleteUser(user.Id))
            {
                ViewData.Upsert("userMsg", $"User {user.Id} deleted");
                ViewData.Upsert("userSuccess", true);
            }
            else
            {
                ViewData.Upsert("userMsg", $"User {user.Id} could not be deleted");
                ViewData.Upsert("userSuccess", false);
            }

            //TODO: keep users in memory (Viewdata?) to avoid refetching. This is cheap enough for an XML file but not for db calls e.g.
            var users = _dataService.GetUsers();

            return View("Index", new UserViewModel<List<User>>() { UserData = users, ActionName = "ListUsers", ControllerName = "User" });
        }        

        public ActionResult EditUser(User user)
        {
            return View("Index", new UserViewModel<List<User>>() { ActionName = "EditUser", UserData = new List<User>() { user } });
        }

        public ActionResult UpdateUser(UserViewModel<User> userViewModel)
        {
            //TODO: validation

            if (_dataService.UpdateUser(userViewModel.UserData))
            {
                ViewData.Set(true, "User Updated");
                return View("Index", new UserViewModel<List<User>>(this.GetType().ToString()) { UserData =_dataService.GetUsers(), ActionName = "ListUsers", ControllerName = "User" });
            }
            else
            {
                _logger.LogWarning("Could not update user", userViewModel.UserData);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
