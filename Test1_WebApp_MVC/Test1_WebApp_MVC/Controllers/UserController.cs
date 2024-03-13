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

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _dataService = new UserDataService(new XmlUserDataContext(logger));

            ViewData.Upsert("activeBtn", "btnList");
        }

        public IActionResult Index()
        {
            ViewData.Upsert("activeBtn", "btnList");

            var users = _dataService.GetUsers();

            return View("Index", users);
        }
    }
}
