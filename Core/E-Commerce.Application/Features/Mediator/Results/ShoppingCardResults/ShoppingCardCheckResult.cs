using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults
{
    public class ShoppingCardCheckResult
    {
        public bool HasShoppingCard { get; set; }
        public string ShoppingCardId { get; set; } // Guid veya string olabilir
    }
}
