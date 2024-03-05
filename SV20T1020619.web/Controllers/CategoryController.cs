using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;

namespace SV20T1020619.web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.CategorySearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);//Models.CategorySearchResult
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
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
            if(model.CategoryID== 0)
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
