using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using SV20T1020619.web.Models;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_Title = "Bổ sung người giao hàng";
        const string SHIPPER_SEARCH = "Shipper_search";

        public IActionResult Index()
        {
            // kiểm tra xem trong session có lưu điều kiện tìm kiếm không.
            // Nếu có thì sử dụng lại điều kiện tìm kiếm ngược lại thì tìm kiếm theo điều kiện mặc định
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = CREATE_Title;
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
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError(nameof(model.ShipperName), "Tên người giao hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Số điện thoại không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? CREATE_Title : "Cập nhập thông tin khách hàng";
                return View("Edit", model);
            }


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
