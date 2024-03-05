using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;

namespace SV20T1020619.web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.EmployeeSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);//Models.EmployeeSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var model = new Employee()
            {
                EmployeeID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin nhân viên";
            var model = CommonDataService.GetEmployee(id);
            if (model == null) 
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee model) 
        {
            // Kiểm tra giá trị của checkbox và cập nhật thuộc tính Model.IsWorking
            model.IsWorking = Request.Form["IsWorking"].Count > 0;
            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
                
            }    
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if(Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
            }    

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index"); 
                
            return View(model);
        }

    }
}
