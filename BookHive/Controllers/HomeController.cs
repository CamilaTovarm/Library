using BookHive.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookHive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult MainLoans()
        {
            return View();
        }
        public IActionResult MainReturns()
        {
            return View();
        }
        public IActionResult MainUser()
        {
            return View();
        }
        public IActionResult MainBi()
        {
            return View();
        }
        public IActionResult MainAut()
        {
            return View();
        }
        public IActionResult MainEdi()
        {
            return View();
        }
        public IActionResult MainMul()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
