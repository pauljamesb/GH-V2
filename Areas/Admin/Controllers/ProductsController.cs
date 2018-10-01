using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Controllers
{

    [Area("Admin")]
    public class ProductsController : Controller
    {







        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnviroment;




        [BindProperty]
        public ProductsViewModel ProdcutsVM { get; set; }

        public ProductsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnviroment = hostingEnvironment;
            ProdcutsVM = new ProductsViewModel()
            {
                ProductTypes = _db.ProductTypes.ToList(),
                MySpecialTags = _db.MySpecialTags.ToList(),
                Products = new Models.Products()
            };
        }


        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.ProductTypes).Include(m => m.MySpecialTags);
            return View(await products.ToListAsync());
        }




        // Get : Products Create
        public IActionResult Create()
        {
            return View(ProdcutsVM);
        }

        // Post: Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        // Below is classic way passing arg to 'CreatePOST', however with [Binding] above, this is not needed now in later vers of dotnet core.
        //public IActionResult CreatePOST(ProductsViewModel productsVM)
        public async Task<IActionResult> CreatePOST()
        {
            if(!ModelState.IsValid)
            {
                return View(ProdcutsVM);
            }

            _db.Products.Add(ProdcutsVM.Products);
            await _db.SaveChangesAsync();

            // Image being saved
            string webRootPath = _hostingEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;



            var productsFromDb = _db.Products.Find(ProdcutsVM.Products.Id);


            if(files.Count!=0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, ProdcutsVM.Products.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProdcutsVM.Products.Id + extension;

            }
            else
            {
                // When user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProdcutsVM.Products.Id + ".jpg");

                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProdcutsVM.Products.Id + ".jpg";

            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }


        //GET : Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProdcutsVM.Products = await _db.Products.Include(m => m.MySpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

            if(ProdcutsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProdcutsVM);
        }







    }
}