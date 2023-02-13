using Microsoft.AspNetCore.Mvc;
using NET6.Models;
using System.Diagnostics;
using UseCase.Config;

namespace NET6.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UseCaseBus _bus;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UseCaseBus bus, ILogger<HomeController> logger)
            : base(bus)
        {
            _bus = bus;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
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