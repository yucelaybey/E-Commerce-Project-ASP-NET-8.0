using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.ProductDtos
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private ResultDto(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static ResultDto<T> Success(T value) => new ResultDto<T>(true, value, null);
        public static ResultDto<T> Failure(string error) => new ResultDto<T>(false, default, error);
    }
}
