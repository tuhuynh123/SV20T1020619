using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "",int categoryId = 0,int supplierId = 0, decimal minPrice = 0,decimal maxPrice = 0)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "", categoryId, supplierId, minPrice, maxPrice);

            var model = new Models.ProductSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                CategoryId = categoryId,
                SupplierId = supplierId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Data = data
            };
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            var model = new Product()
            {
                ProductID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin mặt hàng";
            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");  
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Product model)
        {
            if (model.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(model);
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = ProductDataService.DeleteProduct(id);
            }

            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        public IActionResult Photo(string id, string method, int photoId = 0) 
        {
            switch (method) 
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhập ảnh của mặt hàng";
                    return View();
                case "delete":
                    //TODO: Xóa ảnh có mã photoId (xóa trực tiếp, không cần phải xác nhận)
                    return RedirectToAction("edit",new { id = id });
                default:
                    return RedirectToAction("index");
            }
        }
        public IActionResult Attribute(String id, String method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhập thuộc tính của mặt hàng";
                    return View();
                case "delete":
                    //TODO: Xóa ảnh có mã photoId (xóa trực tiếp, không cần phải xác nhận)
                    return RedirectToAction("edit", new { id = id });
                default:
                    return RedirectToAction("index");
            }
        }
    }
}
