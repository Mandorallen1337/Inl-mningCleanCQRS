using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public OperationResult(bool success, string message, T data, string errorMessage)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> SuccessResult(T data, string message = "Operation Successfull")
        {
            return new OperationResult<T>(true, message, data, null);
        }

        public static OperationResult<T> FailureResult(string errorMessage, string message = "Operation Failed")
        {
            return new OperationResult<T>(false, message, default, errorMessage);
        }
    }
}
