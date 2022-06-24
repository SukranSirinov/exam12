using exam10.DAL;
using exam10.Models;
using exam10.Utilies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace exam10.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public UserController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public async Task< IActionResult> Index()
        {
            List<User> users = await _context.users.ToListAsync();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(User user)
        {
            if (!ModelState.IsValid) return View();
            bool IsExist = _context.users.Any(s => s.Title.ToLower().Trim() == user.Title.ToLower().Trim());
            if (IsExist) return View();
            user.Image = await user.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "img"));
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            User user = _context.users.Find(id);
            if (user == null) return NotFound();
            

            FileExtension.DeleteFile(Path.Combine(_env.WebRootPath, "assets", "img", user.Image));
            _context.users.Remove(user);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));

        }
        
        public IActionResult Update(int id)
        {
            User user = _context.users.Find(id);
            if (user == null) return NotFound();
            return View(user);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id,User user)
        {
            if (user.Id != id) return BadRequest();
            User useritem = _context.users.Find(id);
            if (useritem == null) return NotFound();
            useritem.Title = user.Title;
            useritem.FullName = user.FullName;
            useritem.Image = user.Image;
            useritem.Position = user.Position;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

    }
}
