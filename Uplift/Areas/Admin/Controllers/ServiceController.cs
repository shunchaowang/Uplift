using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public ServiceVM ServiceVM { get; set; }
        public ServiceController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM = new ServiceVM()
            {
                Service = new Service(),
                CategoryList = unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = unitOfWork.Frequency.GetFrequencyListForDropDown()
            };
            if(id != null)
            {
                ServiceVM.Service = unitOfWork.Service.Get(id.GetValueOrDefault());
            }
            return View(ServiceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if(ServiceVM.Service.Id == 0)
                {
                    //TODO: imageUrl
                    unitOfWork.Service.Add(ServiceVM.Service);
                }
                else
                {
                    var objFromDb = unitOfWork.Service.Get(ServiceVM.Service.Id);
                    //TODO: imageUrl
                    unitOfWork.Service.Update(ServiceVM.Service);
                }
                unitOfWork.Save();
                RedirectToAction(nameof(Index));
            }
            else
            {
                ServiceVM.CategoryList = unitOfWork.Category.GetCategoryListForDropDown();
                ServiceVM.FrequencyList = unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(ServiceVM);
            }
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            return Json(new { data = unitOfWork.Service.GetAll(includeProperties: "Category,Frequency") });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = unitOfWork.Service.Get(id);

            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            unitOfWork.Service.Remove(serviceFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion
    }
}