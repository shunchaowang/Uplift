using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public ServiceVM ServiceVM { get; set; }
        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
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
                string webRootPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(ServiceVM.Service.Id == 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    ServiceVM.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    unitOfWork.Service.Add(ServiceVM.Service);
                }
                else
                {
                    var objFromDb = unitOfWork.Service.Get(ServiceVM.Service.Id);

                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        ServiceVM.Service.ImageUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        ServiceVM.Service.ImageUrl = objFromDb.ImageUrl;
                    }
                    unitOfWork.Service.Update(ServiceVM.Service);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
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

            string webRootPath = webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

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