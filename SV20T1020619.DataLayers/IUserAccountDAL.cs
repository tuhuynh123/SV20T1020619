﻿using SV20T1020619.DomainModels;

namespace SV20T1020619.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu liên quan đến tài khoản người dùng
    /// </summary>
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Xác thực tài khoản người đăng nhập của người dùng.
        /// Hàm trả về thông tin tài khoản nếu xác thực thành công.
        /// Ngược lại trả về null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount? Authorize(string userName, string password);
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}