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

            ViewData.Upsert("activeBtn", "btnAdd");
        }

        public IActionResult Index()
        {
            ViewData.Upsert("activeBtn", "btnAdd");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region USER OPERATIONS
        
        public ActionResult CreateUser(User user)
        {
            //TODO: validation

            if (_dataService.CreateUser(user))
            {                
                return View("Index", new Response<User>() { Success = true, Message = "User Saved"});
            }
            else
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion USER OPERATIONS

    }
}
