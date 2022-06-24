using exam10.DAL;
using exam10.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace exam10.Controllers
{
    public class HomeController : Controller
    {

        private AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task< IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                users = await _context.users.ToListAsync(),
            };
            return View(homeVM);
        }


        

       
    }
}
