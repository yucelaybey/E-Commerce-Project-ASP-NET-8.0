﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries
{
    public class GetTotalQuantityByUserIdQuery : IRequest<int?>
    {
        public string UserId { get; set; }
    }
}
