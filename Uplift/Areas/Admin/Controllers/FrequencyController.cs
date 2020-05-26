using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public FrequencyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null)
            {
                return View(new Frequency());
            }
            Frequency frequency = unitOfWork.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    unitOfWork.Frequency.Add(frequency);
                }
                else
                {
                    unitOfWork.Frequency.Update(frequency);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = unitOfWork.Frequency.GetAll() });
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = unitOfWork.Frequency.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            unitOfWork.Frequency.Remove(objFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion
    }
}