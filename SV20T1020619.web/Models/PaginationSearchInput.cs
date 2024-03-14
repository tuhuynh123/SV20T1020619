namespace SV20T1020619.web.Models
{
    /// <summary>
    /// Đầu vào tìm kiếm dữ liệu để nhận đữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string SearchValue { get; set; } = "";

    }
    /// <summary>
    /// Đầu vào tìm kiếm dùng cho mặt hàng
    /// </summary>
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
    }
}
