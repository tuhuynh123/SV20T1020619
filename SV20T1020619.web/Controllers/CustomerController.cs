using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020619.BusinessLayers;
using SV20T1020619.DomainModels;
using SV20T1020619.web.Models;

namespace SV20T1020619.web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_Title = "Bổ sung khách hàng";
        const string CUSTOMER_SEARCH = "Customer_search";// Tên bến session dùng để lưu lại điều kiện tìm kiếm

        public IActionResult Index()
        {
           // kiểm tra xem trong session có lưu điều kiện tìm kiếm không.
           // Nếu có thì sử dụng lại điều kiện tìm kiếm ngược lại thì tìm kiếm theo điều kiện mặc định
           PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH);
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
            var data = CommonDataService.ListOfCustomers(out rowCount, input.Page,input.PageSize, input.SearchValue ?? "");
            var model = new CustomerSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            
            ApplicationContext.SetSessionData(CUSTOMER_SEARCH, input);

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = CREATE_Title;
            var model = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhập thông tin khách hàng";
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);  
        }

        [HttpPost] //Attribute => chỉ nhận dữ liệu gửi lên dưới dạng Post
        public IActionResult Save(Customer model) //int CustomerID, CustomerName,...
        {
            //TODO : Kiểm soát dữ liệu trong model xem có hợp lệ hay không
            //Yêu cầu: tên khách hàng, tên giao dịch, email và tỉnh thành không được để trống
            if (string.IsNullOrWhiteSpace(model.CustomerName))
                ModelState.AddModelError(nameof(model.CustomerName), "Tên không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError(nameof(model.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Province))
                ModelState.AddModelError(nameof(model.Province), "Tỉnh/Thành không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CustomerID == 0 ? CREATE_Title : "Cập nhập thông tin khách hàng";
                return View("Edit",model);
            }    

            if (model.CustomerID == 0) 
            {
                int id = CommonDataService.AddCustomer(model);
                if (id < 0)
                {
                    ModelState.AddModelError("Email", "Email bị trùng");
                    ViewBag.Title = CREATE_Title;
                    return View("Edit",model);
                }

            }
            else
            {
                bool result = CommonDataService.UpdateCustomer(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhập được khách hàng, có thể do email bị trùng");
                    ViewBag.Title = "Cập nhập thông tin khách hàng";
                    return View("Edit", model);
                }    
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST") 
            {
                bool result = CommonDataService.DeleteCustomer(id);
            }

            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}
