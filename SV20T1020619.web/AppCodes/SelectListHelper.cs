using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020619.BusinessLayers;

namespace SV20T1020619.web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh/thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Province()
        {
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh thành --"
            });

            foreach (var item in CommonDataService.ListOfProvinces())
            {
                List.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName,
                });
            }
            return List;
        }
    }
}
