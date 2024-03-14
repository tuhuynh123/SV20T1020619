using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using SV20T1020619.web.Models;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]   
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_Title = "Bổ sung loại hàng";
        const string CATEFORY_SEARCH = "Category_search";

        public IActionResult Index()
        {

            // kiểm tra xem trong session có lưu điều kiện tìm kiếm không.
            // Nếu có thì sử dụng lại điều kiện tìm kiếm ngược lại thì tìm kiếm theo điều kiện mặc định
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEFORY_SEARCH);
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
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(CATEFORY_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = CREATE_Title;
            var model = new Category()
            {
                CategoryID = 0,
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCategory(id);

            if (model == null)                      
                return RedirectToAction("Index");
            
            ViewBag.Title = "Cập nhập thông tin loại hàng";
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Category model) 
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                ModelState.AddModelError(nameof(model.CategoryName), "Tên loại hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError(nameof(model.Description), "Mô tả không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? CREATE_Title : "Cập nhập thông tin khách hàng";
                return View("Edit", model);
            }

            if (model.CategoryID== 0)
            {
                int id = CommonDataService.AddCategory(model);
            }
            else
            {
                bool result = CommonDataService.UpdateCategory(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST") 
            { 
                bool result = CommonDataService.DeleteCategory(id);
            }
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}
