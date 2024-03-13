using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test1_WebApp_MVC.Models;
using Test1_WebApp_MVC.Services;
using Test1_WebApp_MVC.DAL;
using Test1_WebApp_MVC.Models.Interfaces;

namespace Test1_WebApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDataService _dataService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataService = new UserDataService(new XmlUserDataContext(logger));

            ViewData.Reset("btnAdd");
        }

        public IActionResult Index()
        {
            ViewData.Upsert("activeBtn", "btnAdd");

            return View(new UserViewModel<User>() { actionName = "CreateUser", controllerName="Home", userData = new User()});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //TODO fix error handling
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region USER OPERATIONS
        
        public ActionResult CreateUser(UserViewModel<User> userViewModel)
        {
            //TODO: validation

            if (_dataService.CreateUser(userViewModel.userData))
            {
                ViewData.Set(true, "User created");
                return View("Index", new UserViewModel<User>() { actionName = "CreateUser", controllerName = "Home", userData = new User() });

                //TODO: get business requirement if they want to input many at a time, and stay on the Add page; or go to the List page to see the new value
                //If we redirect, add focus on the newly inserted record and scroll down
                //return RedirectToAction("Index", "User");
            }
            else
            {
                _logger.LogWarning("Could not create user", userViewModel);
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            //TODO: investigate why the active button class is not correctly applied to the "Add User" nav button when the user is done being added.
            //TODO: clear form fields on submit; passing in a new blank user as a model for the view isn't doing the trick
        }

        #endregion USER OPERATIONS

    }
}
