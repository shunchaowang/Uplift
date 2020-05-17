using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                OrderHeader = unitOfWork.OrderHeader.Get(id),
                OrderDetails = unitOfWork.OrderDetail.GetAll(filter: o => o.OrderHeader.Id == id)
            };
            return View(orderViewModel);
        }

        public IActionResult Approve(int id)
        {
            var orderFromDb = unitOfWork.OrderHeader.Get(id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusApproved);
            return View(nameof(Index));
        }

        public IActionResult Reject(int id)
        {
            var orderFromDb = unitOfWork.OrderHeader.Get(id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusRejected);
            return View(nameof(Index));
        }

        #region api calls
        public IActionResult GetAllOrders()
        {
            return Json(new { data = unitOfWork.OrderHeader.GetAll() });
        }

        public IActionResult GetAllPendingOrders()
        {
            return Json(new { data = unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusSubmitted) });
        }
        public IActionResult GetAllApprovedOrders()
        {
            return Json(new { data = unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusApproved) });
        }

        #endregion
    }
}