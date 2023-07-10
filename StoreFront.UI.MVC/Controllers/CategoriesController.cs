using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;


namespace StoreFront.UI.MVC.Controllers
{
    [Keyless]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly StoreFrontContext _context;

        public CategoriesController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'StoreFrontContext.Categories'  is null.");
        }

        [AcceptVerbs("POST")]
        public JsonResult AjaxDelete(int id)
        {
            Category category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();

            string confirmMessage = $"Deleted {category.CategoryName} from the database!";
            return Json(new { id = id, message = confirmMessage });
        }

        public PartialViewResult CategoryDetails(int id)
        {
            Category category = _context.Categories.Find(id);

            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxCreate(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return Json(category);
        }

        [HttpGet]
        public PartialViewResult CategoryEdit(int id)
        {
            Category category = _context.Categories.Find(id);
            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();

            return Json(category);
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
