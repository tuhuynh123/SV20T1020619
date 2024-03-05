using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020619.BusinessLayers
{
    /// <summary>
    /// khởi tạo và lưu trũ các thông tin cấu hình của BusinessLayyer
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi thông số kết nối csdl
        /// </summary>
        public static string ConnectionString { get; set; } = "";
        /// <summary>
        /// Hàm khởi tạo cấu hình cho Businesslayer
        /// (Hàm này phải được gọi trước khi chạy ứng dụng)
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}
