using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using SV20T1020619.web.Models;
using System.Buffers;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_Title = "Bổ sung nhà cung cấp";
        const string SUPPLIER_SEARCH = "Supplier_search";

        public IActionResult Index()
        {
            // kiểm tra xem trong session có lưu điều kiện tìm kiếm không.
            // Nếu có thì sử dụng lại điều kiện tìm kiếm ngược lại thì tìm kiếm theo điều kiện mặc định
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""

                };
            }
            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new SupplierSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(SUPPLIER_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = CREATE_Title;
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

            if (string.IsNullOrWhiteSpace(model.SupplierName))
                ModelState.AddModelError(nameof(model.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError(nameof(model.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Province))
                ModelState.AddModelError(nameof(model.Province), "Tỉnh/Thành không được để trống");


            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.SupplierID == 0 ? CREATE_Title : "Cập nhập thông tin khách hàng";
                return View("Edit", model);
            }


            if (model.SupplierID == 0)
            {
                int id = CommonDataService.AddSupplier(model);

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
