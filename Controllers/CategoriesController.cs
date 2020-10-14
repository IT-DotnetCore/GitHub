using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestLibrary.ApplicationDbContext;
using TestLibrary.Models;

namespace TestLibrary.Controllers
{
   // [Authorize("Admin,Executive")]
    public class CategoriesController : Controller
    {
        private readonly LibraryDbContext _context;

        public CategoriesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            var make = _context.categories.Find(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }
        [HttpPost]
        public IActionResult Edit(Category make)
        {
            if (ModelState.IsValid)
            {
                _context.Update(make);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }
      
        // GET: Categories/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var make = _context.categories.Find(id);
            if (make == null)
            {
                return NotFound();
            }
            _context.categories.Remove(make);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
       

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.id == id);
        }
    }
}
