using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ShopServiceApi.Application.Services.Auth;
using ShopServiceApi.Application.Services.Auth.Interfaces;
using ShopServiceApi.Application.Services.Orders;
using ShopServiceApi.Application.Services.Orders.Interface;
using ShopServiceApi.Application.Services.Products;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ====================
// DbContext (PostgreSQL)
// ====================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====================
// Identity
// ====================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ====================
// JWT Authentication
// ====================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsASecretKeyForDevelopment";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ShopServiceApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// ====================
// Controllers + Swagger 10.x
// ====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopServiceApi API",
        Version = "v1",
        Description = "Backend API for ShopServiceApi",
    });
});

// ====================
// CORS (برای Angular / Frontend)
// ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("https://localhost:4200") // آدرس Angular در Dev
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// ====================
// Seed Roles (Admin / Customer)
// ====================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// ====================
// Middleware
// ====================
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopServiceApi API V1");
//        c.RoutePrefix = "swagger"; // دسترسی: https://localhost:5001/swagger
//    });
//}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopServiceApi API V1");
        c.RoutePrefix = string.Empty; // Swagger روی ریشه
    });
}



app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

// ====================
// Map Controllers
// ====================
app.MapControllers();

// ====================
// Example Endpoint
// ====================
app.MapGet("/", () => "ShopServiceApi Backend Running");

// ====================
// Run
// ====================
app.Run();
