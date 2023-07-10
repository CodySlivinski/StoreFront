using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using StoreFront.UI.MVC.Utilities;
using Microsoft.AspNetCore.Hosting;
using X.PagedList;

namespace StoreFront.UI.MVC.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var storeFrontContext = _context.Products.Include(p => p.Category).Include(p => p.ProductStatus).Include(p => p.Restriction).Include(p => p.Supplier);
            return View(await storeFrontContext.ToListAsync());
        }

        //Get: Products/TiledProducts
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(
            string searchTerm, int categoryId = 0, int page = 1)
        {
            int pageSize = 6;

            var storeFrontContext = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Restriction)
                .Include(p => p.Supplier).ToList();
            

            //Category Filter
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                storeFrontContext = storeFrontContext.Where(p => p.CategoryId == categoryId).ToList();

                //Repopulate the dropdown menu with the currently selected category 
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);

                //Persist the selected Category
                ViewBag.Category = categoryId;
            }

            //Search Filter
            if (!String.IsNullOrEmpty(searchTerm))
            {
                ViewBag.SearchTerm = searchTerm;
                searchTerm = searchTerm.ToLower();
                storeFrontContext = storeFrontContext.Where(p =>
                p.Name.ToLower().Contains(searchTerm)
                || p.Supplier.Name.ToLower().Contains(searchTerm)
                || p.Category.CategoryName.ToLower().Contains(searchTerm)
                || p.Description.ToLower().Contains(searchTerm)
                || p.Restriction.PermitNeeded.ToLower().Contains(searchTerm)).ToList();

                ViewBag.NbrResults = storeFrontContext.Count;
            }
            else
            {
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }



            return View(storeFrontContext.ToPagedList(page, pageSize));
        }


        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id, string? prevAction)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Restriction)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            if (prevAction == "Index")
            {
                ViewBag.PrevAction = "Index";
            }
            else
            {
                ViewBag.PrevAction = "TiledProducts";
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "ProductStatus1");
            ViewData["RestrictionId"] = new SelectList(_context.Restrictions, "RestrictionId", "RestrictionType");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,CategoryId,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,RestockLevel,ProductStatusId,SupplierId,RestrictionId,Image,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Create

                //Check to see if an Image was uploaded
                if (product.ProductImage != null)
                {
                    //Check to see if the file type is one that we would like to use
                    // - Retrieve the extension of the uploaded file 
                    string ext = Path.GetExtension(product.ProductImage.FileName);

                    // - Create a list of valid extensions
                    string[] validExts = { ".jpg", ".jpeg", ".gif", ",png" };

                    //- Verify that the uploaded file has an extension matching one of the extensions in the list
                    //above and verify the file size is not too big for our .NET app
                    if (validExts.Contains(ext.ToLower()) && product.ProductImage.Length < 4_194_303)
                    {
                        //Generate a unique file name
                        product.Image = Guid.NewGuid() + ext;

                        //Save the file to the web server (here we will save to the wwwroot/images).
                        //In order to access the wwwroot folder we must add a field to the controller for the 
                        // _webHostEnvironment (see the top of this file for an example).

                        //Retrieve the path to the wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Create a variable for the full image path (this is where we will save the image)
                        //note that the path is not always the same there dumby
                        string fullImagePath = webRootPath + "/img/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //Transfer the file from the request to server memory
                            await product.ProductImage.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500; //Pixels 
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(
                                    fullImagePath, product.Image, img, maxImageSize, maxThumbSize);

                            }
                        }

                    }
                }

                else
                {
                    //If no image was uploaded, assign a defualt filename.
                    //We can then download a defualt image and give it that same filename and copy it to the 
                    //wwwroot/images folder.
                    product.Image = "noimage.png";

                }

                #endregion

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "ProductStatus1", product.ProductStatusId);
            ViewData["RestrictionId"] = new SelectList(_context.Restrictions, "RestrictionId", "RestrictionId", product.RestrictionId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "ProductStatus1", product.ProductStatusId);
            ViewData["RestrictionId"] = new SelectList(_context.Restrictions, "RestrictionId", "RestrictionId", product.RestrictionId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,CategoryId,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,RestockLevel,ProductStatusId,SupplierId,RestrictionId,Image,ProductImage")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region File Upload -Edit

                //Retain the old file name for the image so we can delete it if a new image was uploaded
                //when editting the product.
                string oldImageName = product.Image;

                //Check if the user uploaded a file
                if (product.Image != null)
                {
                    //Get the file's extension 
                    string ext = Path.GetExtension(product.ProductImage.FileName);

                    //List the valid extensions
                    string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    //check the file extension against the list and make sure its not to big
                    if (validExts.Contains(ext.ToLower()) && product.ProductImage.Length < 4_194_303)
                    {
                        //Generate a unique file name 
                        product.Image = Guid.NewGuid() + ext;

                        //Build the file path for where we want to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        string fullPath = webRootPath + "/img/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to the webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.ProductImage.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(
                                    fullPath, product.Image, img, maxImageSize, maxThumbSize);
                            }
                        }

                    }

                }

                #endregion
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "ProductStatus1", product.ProductStatusId);
            ViewData["RestrictionId"] = new SelectList(_context.Restrictions, "RestrictionId", "RestrictionId", product.RestrictionId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStatus)
                .Include(p => p.Restriction)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
