using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MySpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MySpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.MySpecialTags.ToList());
        }


        // GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        // POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MySpecialTags mySpecialTags)
        {
            if (ModelState.IsValid)
            {
                _db.Add(mySpecialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mySpecialTags);
        }







        // GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mySpecialTags = await _db.MySpecialTags.FindAsync(id);
            if (mySpecialTags == null)
            {
                return NotFound();
            }

            return View(mySpecialTags);
        }

        // POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MySpecialTags mySpecialTags)
        {
            if (id != mySpecialTags.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(mySpecialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mySpecialTags);
        }





        // GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mySpecialTags = await _db.MySpecialTags.FindAsync(id);
            if (mySpecialTags == null)
            {
                return NotFound();
            }

            return View(mySpecialTags);
        }





        // GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mySpecialTags = await _db.MySpecialTags.FindAsync(id);
            if (mySpecialTags == null)
            {
                return NotFound();
            }

            return View(mySpecialTags);
        }

        // POST Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mySpecialTags = await _db.MySpecialTags.FindAsync(id);
            _db.MySpecialTags.Remove(mySpecialTags);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}