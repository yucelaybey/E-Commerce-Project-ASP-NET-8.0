using E_Commerce.Persistence.Context;
using ECommerce.WebUI.SendEmailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// AntiForgery token için
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie
    (JwtBearerDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/Login/GirisYap/";
        opt.LogoutPath = "/Login/Logout/";
        opt.AccessDeniedPath = "/Erisim/ErisimKisitlamasi";
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        opt.Cookie.Name = "ECommerceJwt";
    });

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ECommerceContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "Anasayfa",
    defaults: new { controller = "Main", action = "Anasayfa" });

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Main", action = "Anasayfa" });

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Anasayfa/");
        return;
    }
    await next();
});

app.Run();
