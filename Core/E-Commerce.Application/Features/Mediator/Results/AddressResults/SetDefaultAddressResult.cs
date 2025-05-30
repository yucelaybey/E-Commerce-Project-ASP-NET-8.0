using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.AddressResults
{
    public class SetDefaultAddressResult
    {
        public bool Succeeded { get; set; }  // İşlem başarılı mı?
        public string Error { get; set; }   // Hata mesajı (başarısızsa)

        // Başarılı sonuç üreten static method
        public static SetDefaultAddressResult Success() => new SetDefaultAddressResult { Succeeded = true };

        // Başarısız sonuç üreten static method
        public static SetDefaultAddressResult Failure(string error) => new SetDefaultAddressResult
        {
            Succeeded = false,
            Error = error
        };
    }
}
