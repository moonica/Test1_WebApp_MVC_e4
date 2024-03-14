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

            //return View(new UserViewModel<User>() { actionName = "CreateUser", controllerName="Home", userData = new User()});
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //TODO better error handling
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Reset()
        {
            ViewData.Reset("btnAdd");
            return View("Index");
        }

        #region USER OPERATIONS

        public ActionResult CreateUser(UserViewModel<User> userViewModel)
        {
            try
            {
                var userToCreate = userViewModel?.UserData;

                if (userToCreate is null)
                {
                    ViewData.SetState(false, "No valid user data received to add");
                    return View("Index");
                }

                //ASSUMPTION: inputs are sanitized already by the @Html helper class
                if (!userToCreate.IsValid())
                {
                    ViewData.SetState(false, $"Invalid user create input. {userToCreate.ValidationErrors.Flatten(". ")}");
                    return View("Index");
                }

                if (_dataService.CreateUser(userToCreate))
                {
                    ViewData.SetState(true, "User created");
                    return View("Index");

                    //TODO: get business requirement if they want to input many at a time, and stay on the Add page; or go to the List page to see the new value
                    //If we redirect, add focus on the newly inserted record and scroll down
                    //return RedirectToAction("Index", "User");
                }
                else
                {
                    _logger.LogWarning("Could not create user", userViewModel);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                //TODO: investigate why the active button class is not correctly applied to the "Add User" nav button when the user is done being added.
                //TODO: clear form fields on submit; passing in a new blank user as a model for the view isn't doing the trick
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create user", ex);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }

        #endregion USER OPERATIONS

    }
}
