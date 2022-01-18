using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Core
{
    public static class ErrorConstants
    {
        public const int ErrorRequiredModel = 400;
        public const int EmailExist = 401;
        public const int InvalidGuid = 402;
        public const int NullRequestBody = 403;
        public const int InvalidEmail = 404;

        public const int ServerError = 500;
        public const int RegisterFail = 501;

        public const string ErrorRequiredModelMessage = "Thiếu thông tin bắt buộc";
        public const string EmailExistMessage = "Email đã tồn tại";
        public const string InvalidGuidMessage = "Id không đúng định dạng";
        public const string NullRequestBodyMessage = "Request body trống";
        public const string InvalidEmailMessage = "Email không đúng định dạng";

        public const string ServerErrorMessage = "Đã có lỗi xảy ra";
        public const string RegisterFailMessage = "Đăng ký tài khoản thất bại";
    }
}
