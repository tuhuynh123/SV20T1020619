using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using SV20T1020619.web.Models;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_Title = "Bổ sung nhân viên";
        const string EMPLOYEE_SEARCH = "Employee_search";


        public IActionResult Index()
        {

            // kiểm tra xem trong session có lưu điều kiện tìm kiếm không.
            // Nếu có thì sử dụng lại điều kiện tìm kiếm ngược lại thì tìm kiếm theo điều kiện mặc định
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
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
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = CREATE_Title;
            var model = new Employee()
            {
                EmployeeID = 0,
                Photo = "nophoto.png",
                BirthDate = new DateTime(1990, 1, 1)
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin nhân viên";
            var model = CommonDataService.GetEmployee(id);
            if (model == null) 
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(model.Photo))
                model.Photo = "nophoto.png";
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee model, string birthDateInput = "",IFormFile? uploadPhoto = null) 
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError(nameof(model.FullName), "Tên nhân viên không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError(nameof(model.Address), "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? CREATE_Title : "Cập nhập thông tin khách hàng";
                return View("Edit", model);
            }

            // Kiểm tra giá trị của checkbox và cập nhật thuộc tính Model.IsWorking
            model.IsWorking = Request.Form["IsWorking"].Count > 0;
            //Sử lý ngày sinh
            DateTime? d = birthDateInput.ToDateTime();
            if (d.HasValue)
                model.BirthDate = d.Value;
            
            //Xử lý ảnh upload: Nếu có ảnh được upload thì lưu ảnh lên server, gán tên file ảnh đã lưu cho model.Photo
            if (uploadPhoto != null)
            {
                //Tên file lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                //Đường dẫn đến file sẽ lưu trên server (Vd: D\Myweb\Photo)
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath,@"images\employees", fileName);
                
                //Lưu file trên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }    
                //Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }    
            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
                if (id < 0)
                {
                    ModelState.AddModelError("Email", "Email bị trùng");
                    ViewBag.Title = CREATE_Title;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhập được khách hàng,có thể do email bị trùng");
                    ViewBag.Title = "Cập nhập thông tin khách hàng";
                    return View("Edit", model);
                }

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
            if (string.IsNullOrWhiteSpace(model.Photo))
                model.Photo = "nophoto.png";
            return View(model);
        }

    }
}
