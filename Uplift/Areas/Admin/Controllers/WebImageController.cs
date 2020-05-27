using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Uplift.DataAccess.Data;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImageController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public WebImageController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var image = new WebImage();
            if (id == null) // inserting 
            {
                return View(image);
            }

            image = dbContext.WebImage.SingleOrDefault(i => i.Id == id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImage image)
        {
            if (ModelState.IsValid)
            {
                byte[] p1 = null;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    using var fs1 = files[0].OpenReadStream();
                    using var ms1 = new MemoryStream();
                    fs1.CopyTo(ms1);
                    p1 = ms1.ToArray();
                }
                image.Picture = p1;

                if (image.Id == 0)
                {
                    dbContext.WebImage.Add(image);
                }
                else
                {
                    var imageFromDb = dbContext.WebImage.Where(w => w.Id == image.Id).FirstOrDefault();
                    imageFromDb.Name = image.Name;

                    if (files.Count > 0)
                    {
                        imageFromDb.Picture = image.Picture;
                    }
                }
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = dbContext.WebImage.ToList() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = dbContext.WebImage.Find(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, Message = "Error while deleting." });
            }

            dbContext.WebImage.Remove(objFromDb);
            dbContext.SaveChanges();

            return Json(new { success = true, Message = "Delete successful." });
        }

        #endregion
    }
}