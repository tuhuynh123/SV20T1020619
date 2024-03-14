using SV20T1020619.DataLayers;
using SV20T1020619.DataLayers.SQLServer;
using SV20T1020619.DomainModels;


namespace SV20T1020619.BusinessLayers
{
    public static class UserAccountService
    {
        private static IUserAccountDAL employeeAccountDB;
        static UserAccountService() 
        { 
            employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Kiểm tra đăng nhập của employee
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAccountDB.Authorize(userName, password);
        }
        /// <summary>
        /// Thay đổi mật khẩu của employee
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool ChangePassword(string userName, string oldPassword, string newPassword) 
        { 
            return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }
    }
}
