using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id == null) // inserting 
            {
                return View(category);
            }

            category = unitOfWork.Category.Get(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    unitOfWork.Category.Add(category);
                }
                else
                {
                    unitOfWork.Category.Update(category);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

         #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //return Json(new { data = unitOfWork.Category.GetAll() });
            return Json(new { data = unitOfWork.SPCall.ReturnList<Category>(SD.usp_GetAllCategory) });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = unitOfWork.Category.Get(id);
            if(objFromDb == null)
            {
                return Json(new { success = false, Message = "Error while deleting." });
            }

            unitOfWork.Category.Remove(objFromDb);
            unitOfWork.Save();

            return Json(new { success = true, Message = "Delete successful." });
        }

        #endregion
    }
}