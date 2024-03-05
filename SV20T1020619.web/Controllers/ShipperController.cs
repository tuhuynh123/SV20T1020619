﻿using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;

namespace SV20T1020619.web.Controllers
{
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.ShipperSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);//Models.ShipperSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            var model = new Shipper
            {
                ShipperID = 0,
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin người giao hàng";
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        public IActionResult Save(Shipper model) 
        {
            if (model.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(model);
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST") 
            { 
                bool result = CommonDataService.DeleteShipper(id);
            }

            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}
