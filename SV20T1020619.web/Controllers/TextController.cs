using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020619.web.Controllers
{
    public class TextController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Tự",
                Birthday = new DateTime(1990, 10, 10),
                Salary = 500m
            };
            

            return View(model);
        }
        public IActionResult Save(Models.Person model, string birthdayInput = "") 
        { 
            ;
            //chuyển chuỗi birthdayInput thành giá trị ngày, nếu hợp lệ thì mới dùng giá trị do người dùng nhập
            DateTime? d = null;
            try
            {
                d = DateTime.ParseExact(birthdayInput, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {

            }
            if(d.HasValue)
                model.Birthday = d.Value;
            return Json(model);
        }
    }
}
