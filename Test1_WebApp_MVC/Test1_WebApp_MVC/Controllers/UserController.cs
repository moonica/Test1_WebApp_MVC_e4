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
            ViewData.Upsert("activeBtn", "btnList");

            var users = _dataService.GetUsers();

            return View("Index", users);
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
            return View("Index");
        }
    }
}
