using SV20T1020619.DataLayers;
using SV20T1020619.DataLayers.SQLServer;
using SV20T1020619.DomainModels;

namespace SV20T1020619.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng (Không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List().ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "",
                                              int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice).ToList();
        }
        /// <summary>
        /// Lấy thông tin 1 mặt hàng theo mặt hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Product? GetProduct(int productId) 
        {
            return productDB.Get(productId);
        }
        
        public static int AddProduct(Product data) 
        { 
            return productDB.Add(data);
        }
        public static bool UpdateProduct(Product data) 
        { 
            return productDB.Update(data);
        }
        public static bool DeleteProduct(int productId) 
        { 
            if (productDB.InUsed(productId))
                return false;

            return productDB.Delete(productId);
        }
        public static bool InUsedProduct(int productId)
        {
            return productDB.InUsed(productId);
        }

        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return productDB.ListPhotos(productID).ToList();
        }
        public static ProductPhoto? GetPhoto(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }

        public static List<ProductAttribute> ListAttributes(int productID)
        {
            return productDB.ListAttributes(productID).ToList();
        }
        public static ProductAttribute? GetAttribute(long attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAtribute(data);
        }
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAtribute(data);
        }
        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAtribute(attributeID);
        }
    }
}
