using Azure;
using Dapper;
using SV20T1020619.DomainModels;
using System.Buffers;
using System.Data;


namespace SV20T1020619.DataLayers.SQLServer
{
    public class ProductDAL : BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                            values(@ProductName,@ProductDescription,@SupplierID,@CategoryID,@Unit,@Price,@Photo,@IsSelling);
                            select @@identity";

                var parameters = new
                {
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierId = data.SupplierID,
                    CategoryId = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddAtribute(ProductAttribute data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductAttributes(ProductID,AttributeName,AttributeValue,DisplayOrder)
                            values(@ProductID,@AttributeName,@AttributeValue,@DisplayOrder);
                            select @@identity";

                var parameters = new
                {
                    ProductId = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductPhotos(ProductID,Photo,Description,DisplayOrder,IsHidden)
                            values(@ProductID,@Description,@DisplayOrder,@IsHidden);
                            select @@identity";

                var parameters = new
                {
                    ProductId = data.ProductID,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Products 
                            where (@searchValue = N'') or (ProductName like @searchValue)";
                var parameters = new 
                { 
                    searchValue = searchValue ?? "",
                    CategoryId = categoryID,
                    SupplierId = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes
                            where ProductId = @ProductId
                            delete from ProductPhotos
                            where ProductId = @ProductId
                            delete from Products 
                            where ProductId = @ProductId";
                var parameters = new
                {
                    ProductId = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAtribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AttributeId = @AttributeId";
                var parameters = new
                {
                    AttributeId = attributeID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long PhotoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where PhotoId = @PhotoId";
                var parameters = new
                {
                    PhotoId = PhotoID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductId = @ProductId";
                var parameters = new
                {
                    Productid = productID
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where AttributeId = @AttributeId";
                var parameters = new
                {
                    AttributeId = attributeID
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where PhotoId = @PhotoId";
                var parameters = new
                {
                    PhotoId = photoID
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM OrderDetails WHERE ProductId = @ProductId)
                        SELECT 1
                    ELSE 
                        SELECT 0";
                var parameters = new { ProductId = productID };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> list = new List<Product>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"with cte as(
                            select  *,
                                    row_number() over(order by ProductName) as RowNumber
                            from    Products
                            where   (@SearchValue = N'' or ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)
                        )
                        select * from cte
                        where   (@PageSize = 0)
                            or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)";
                var parameters = new
                {
                    Page = page,
                    PageSize = pageSize,
                    searchValue = searchValue ?? "",
                    CategoryId = categoryID,
                    SupplierId = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                list = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }

        public List<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> list = new List<ProductAttribute>();
        
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes
                            where ProductId =@ProductId
                            Order by DisplayOrder";
                var parameters = new
                {
                    ProductId = productID,
                };
                list = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> list = new List<ProductPhoto>();
      
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos
                            where PhotoId =@PhotoId
                            Order by DisplayOrder";
                var parameters = new
                {
                    ProductId = productID,
                };
                list = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update Products 
                              set ProductName = @productName,
                                  ProductDescription = @productDescription,
                                  SupplierId = @supplierId,
                                  CategoryId = @categoryId,
                                  Unit = @unit,
                                  Price = @price,
                                  Photo = @photo,
                                  IsSelling = @isSelling
                                  where ProductId = @productId";
                var parameters = new
                {
                    ProductId = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierId = data.SupplierID,
                    CategoryId = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();

            }
            return result;
        }

        public bool UpdateAtribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update ProductAttributes 
                              set ProductId = @productId,
                                  AttributeName = @attributeName,
                                  AttributeValue = @attributeValue,
                                  DisplayOrder = @displayOrder
                                  where AttributeId = @attributeId";
                var parameters = new
                {
                    AttributeId = data.AttributeID,
                    ProductId = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,                
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();

            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update ProductPhotos 
                              set ProductId = @productId,
                                  Photo = @photo,
                                  Description = @description,
                                  DisplayOrder = @displayOrder,                                
                                  IsHidden = @isHidden
                                  where PhotoId = @photoId";
                var parameters = new
                {
                    PhotoId =data.ProductID,
                    ProductId = data.ProductID,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();

            }
            return result;
        }
    }
}
