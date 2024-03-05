using Microsoft.AspNetCore.Mvc;

namespace SV20T1020619.web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.IsEdit = false;
            return View("Edit");
        }
        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhập thông tin mặt hàng";
            ViewBag.IsEdit = true;
            return View();
        }
        public IActionResult Delete(string id)
        {
            return View();
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
