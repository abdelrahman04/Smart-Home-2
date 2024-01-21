using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HomeSync.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Razor.TagHelpers;
using HomeSync.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace HomeSync.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public int USERID = 0;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();  
        }
        [HttpPost]
        public IActionResult LogIn(string email,string password)
        {
            return RedirectToAction("LogIn","Users",new {email = email,password = password});
        }
        [HttpPost]
        public IActionResult Register(string email,string fname,string lname,DateTime birth, string password) {
            return RedirectToAction("Register", "Users", new { email = email, fname = fname, lname = lname, birth = birth, pass = password });
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
       
        public IActionResult Register()
        {
           return View();
        }
    }
}
