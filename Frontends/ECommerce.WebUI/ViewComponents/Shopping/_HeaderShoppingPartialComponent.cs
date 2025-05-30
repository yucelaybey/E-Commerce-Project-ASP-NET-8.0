using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents.Shopping
{
    public class _HeaderShoppingPartialComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
