using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public CartViewModel CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ServiceList = new List<Service>()
            };

        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(unitOfWork.Service.GetFirstOrDefault(s => s.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }
            return View(CartVM);
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(unitOfWork.Service.GetFirstOrDefault(s => s.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }
            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SubmitOrder()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                CartVM.ServiceList = new List<Service>();
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(unitOfWork.Service.GetFirstOrDefault(s => s.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }

            // save order header
            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.Status = SD.StatusSubmitted;
            CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;

            unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            unitOfWork.Save();

            // save order
            foreach (var item in CartVM.ServiceList)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ServiceId = item.Id,
                    ServiceName = item.Name,
                    Price = item.Price,
                    OrderHeaderId = CartVM.OrderHeader.Id
                };

                unitOfWork.OrderDetail.Add(orderDetail);
            }
            unitOfWork.Save();

            HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
            return RedirectToAction("ConfirmOrder", "Cart", new { id = CartVM.OrderHeader.Id });
        }

        public IActionResult ConfirmOrder(int id)
        {
            return View(id);
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }

    }
}