using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.ExceptionHandle
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; } 
        public T Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> FailResponse(string errorCode, string message)
        {
            return new ApiResponse<T> { Success = false, ErrorCode = errorCode, Message = message };
        }
    }
}
