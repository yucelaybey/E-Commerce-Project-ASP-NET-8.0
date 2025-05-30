using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands
{
    public class CreateOrderItemCommand : IRequest
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SalePrice { get; set; }
        public int Stock { get; set; }
        public double DiscountRate { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        public bool SaleSeason { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
    }
}
