﻿using BadgeSpace.Data;
using BadgeSpace.Models;
using BadgeSpace.Models.Enums;
using BadgeSpace.Utils.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BadgeSpace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() => View();

        public IActionResult API() => View();

        [Authorize(Roles = nameof(Roles.STUDENT))]
        public IActionResult Dashboard()
        {
            var (logado, user) = CheckIfUserIsValid.IsUserValid(_context.Users, User);

            if (logado)
            {
                ViewBag.CPF = user!.CPF_CNPJ;
                return View(_context.Students.AsEnumerable());
            }

            return Unauthorized();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}