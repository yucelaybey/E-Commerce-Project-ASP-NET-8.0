using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.AddressResults
{
    public class GetCreateResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static GetCreateResult<T> SuccessResult(T data, string message = null)
        {
            return new GetCreateResult<T> { Success = true, Data = data, Message = message };
        }

        public static GetCreateResult<T> FailureResult(string message)
        {
            return new GetCreateResult<T> { Success = false, Message = message };
        }
    }
}
