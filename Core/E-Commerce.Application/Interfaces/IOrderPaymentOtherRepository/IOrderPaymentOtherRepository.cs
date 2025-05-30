using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IOrderPaymentOtherRepository
{
    public interface IOrderPaymentOtherRepository
    {
        Task<int> CreateOrderPaymentOtherAsync(OrderPaymentOther order);
    }
}
