using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaBox.Client2.Models;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client2.Controllers
{
  public class HomeController : Controller
  {
    public static readonly UserRepository _up = new UserRepository();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel login)
    {
      if(ModelState.IsValid) 
      { 
        if(_up.Check(login.Username, login.Password) != null) { return RedirectToAction("PizzaHome"); }
      }
      return View("Login", login);
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
