﻿using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using System.Buffers;

namespace SV20T1020619.web.Controllers
{
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.SupplierSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);//Models.SupplierSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var model = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin nhà cung cấp";
            var model = CommonDataService.GetSupplier(id);

            if(model == null) 
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Supplier model) 
        { 
            if(model.SupplierID == 0)
            {
                int id = CommonDataService.AddSupplier(model);
                TempData["Message"] = $"Nhà cung cấp với ID {id} đã được thêm thành công.";
            }
            else
            {
                bool result = CommonDataService.UpdateSupplier(model);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteSupplier(id);
            }

            var model = CommonDataService.GetSupplier(id);
            if(model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}