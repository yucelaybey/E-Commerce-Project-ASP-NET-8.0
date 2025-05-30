using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Interfaces.AppUserInterfaces;
using E_Commerce.Application.Interfaces.IAddressRepository;
using E_Commerce.Application.Interfaces.IBestFiveShoppingCardAddProductRepository;
using E_Commerce.Application.Interfaces.IBestSellerProductAndAmountRepository;
using E_Commerce.Application.Interfaces.ICategoryProductRepository;
using E_Commerce.Application.Interfaces.IFavoritesCardItemRepository;
using E_Commerce.Application.Interfaces.IFavoritesCardRepository;
using E_Commerce.Application.Interfaces.ILocationRepository;
using E_Commerce.Application.Interfaces.IOrderItemRepository;
using E_Commerce.Application.Interfaces.IOrderPaymentAddressRepository;
using E_Commerce.Application.Interfaces.IOrderPaymentCardRepository;
using E_Commerce.Application.Interfaces.IOrderPaymentOtherRepository;
using E_Commerce.Application.Interfaces.IOrderRepository;
using E_Commerce.Application.Interfaces.IPaymentCardRepository;
using E_Commerce.Application.Interfaces.IProductCategoryDetailRepository;
using E_Commerce.Application.Interfaces.IProductRepository;
using E_Commerce.Application.Interfaces.ISearchRepository;
using E_Commerce.Application.Interfaces.IShoppingCardGetUserIdRepositories;
using E_Commerce.Application.Interfaces.IShoppingCardItemCheckRepository;
using E_Commerce.Application.Interfaces.IThirtyLastOrderItemTotalAmountRepository;
using E_Commerce.Application.Interfaces.IThirtyLastOrderTotalAmount;
using E_Commerce.Application.Services;
using E_Commerce.Application.Tools;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Persistence.Context;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Persistence.Repositories.AddressRepository;
using E_Commerce.Persistence.Repositories.AppUserRepositories;
using E_Commerce.Persistence.Repositories.BestFiveShoppingCardAddProductRepository;
using E_Commerce.Persistence.Repositories.CategoryProductRepository;
using E_Commerce.Persistence.Repositories.FavoritesCardItemRepository;
using E_Commerce.Persistence.Repositories.FavoritesCardRepository;
using E_Commerce.Persistence.Repositories.LocationRepository;
using E_Commerce.Persistence.Repositories.OrderItemRepository;
using E_Commerce.Persistence.Repositories.OrderPaymentAddressRepository;
using E_Commerce.Persistence.Repositories.OrderPaymentCardRepository;
using E_Commerce.Persistence.Repositories.OrderPaymentOtherRepository;
using E_Commerce.Persistence.Repositories.OrderRepository;
using E_Commerce.Persistence.Repositories.PaymentCardRepository;
using E_Commerce.Persistence.Repositories.ProductCategoryDetailRepository;
using E_Commerce.Persistence.Repositories.ProductRepository;
using E_Commerce.Persistence.Repositories.SearchRepository;
using E_Commerce.Persistence.Repositories.ThirtyLastOrderItemTotalAmountRepository;
using E_Commerce.Persistence.Repositories.ThirtyLastOrderTotalAmountRepository;
using ECommerce.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = JwtTokenDefaults.ValidAudience,
        ValidIssuer = JwtTokenDefaults.ValidIssuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key)),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Add services to the container.
builder.Services.AddScoped<ECommerceContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IThirtyLastOrderTotalAmountRepository, ThirtyLastOrderTotalAmountRepository>();
builder.Services.AddScoped<IThirtyLastOrderItemTotalAmountRepository, ThirtyLastOrderItemTotalAmountRepository>();
builder.Services.AddScoped<IBestSellerProductAndAmountRepository, BestSellerProductAndAmountRepository>();
builder.Services.AddScoped<IBestFiveShoppingCardAddProductRepository, BestFiveShoppingCardAddProductRepository>();
builder.Services.AddScoped<ICategoryProductRepository, CategoryProductRepository>();
builder.Services.AddScoped<IProductCategoryDetailRepository, ProductCategoryDetailRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IHaveAEmailRepository, HaveAEmailRepository>();
builder.Services.AddScoped<IAppUserPasswordResetRepository, AppUserPasswordResetRepository>();
builder.Services.AddScoped<IShoppingCardGetUserIdRepository, ShoppingCardGetUserIdRepository>();
builder.Services.AddScoped<IShoppingCardItemCheckRepository, ShoppingCardItemCheckRepository>();
builder.Services.AddScoped<IFavoritesCardRepository, FavoritesCardRepository>();
builder.Services.AddScoped<IFavoritesCardItemRepository, FavoritesCardItemRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPaymentCardRepository, PaymentCardRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderPaymentCardRepository, OrderPaymentCardRepository>();
builder.Services.AddScoped<IOrderPaymentOtherRepository, OrderPaymentOtherRepository>();
builder.Services.AddScoped<IOrderPaymentAddressRepository, OrderPaymentAddressRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();

builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
