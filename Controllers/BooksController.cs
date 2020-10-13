using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestLibrary.ApplicationDbContext;
using TestLibrary.Canstanta;
using TestLibrary.Models;

namespace TestLibrary.Controllers
{
    [Authorize("Admin,Executive")]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(
                       string sortOrder,
                       string currentFilter,
                       string searchString,
                        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var book = from s in _context.books.Include(b => b.Category)
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.Title.Contains(searchString)
                                       || s.Author.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    book = book.OrderByDescending(s => s.Author);
                    break;
                default:
                    book = book.OrderBy(s => s.Author);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Book>.CreateAsync(book.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            ViewData["categoryID"] = new SelectList(_context.categories, "id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Title,Author,Language,categoryID")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryID"] = new SelectList(_context.categories, "id", "Name", book.categoryID);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["categoryID"] = new SelectList(_context.categories, "id", "Name", book.categoryID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Title,Author,Language,categoryID")] Book book)
        {
            if (id != book.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryID"] = new SelectList(_context.categories, "id", "Name", book.categoryID);
            return View(book);
        }

        private bool BookExists(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var make = _context.books.Find(id);
            if (make == null)
            {
                return NotFound();

            }
            _context.books.Remove(make);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //private bool BookExists(int id)
        //{
        //    return _context.books.Any(e => e.id == id);
        //}
    }
}
