﻿using E_Commerce.Domain.Entities;
using ECommerce.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.IProductRepository
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
    }
}
