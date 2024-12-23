using appData.Models;
using appData.Repository;
using appData.Repository.EnityRepository;
using appData.Repository.Interface;
using appAPI.Service;
using appAPI.Service.EntityService;
using appAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using appData.Utils.Enitty;
using AutoMapper;
using appData.ModelsDTO.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Lấy cấu hình JWT từ appsettings.json
var jwtSection = builder.Configuration.GetSection("Jwt");

// Cấu hình dịch vụ xác thực JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],   // Địa chỉ issuer trong appSettings
            ValidAudience = jwtSection["Audience"], // Địa chỉ audience trong appSettings
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSection["SecretKey"])), // Khóa bí mật
            ClockSkew = TimeSpan.Zero // Đặt ClockSkew = 0 để giảm độ trễ khi kiểm tra token
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });


// Thêm dịch vụ phân quyền nếu cần
builder.Services.AddAuthorization(options =>
{
    // Thêm chính sách phân quyền (nếu cần)
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});


builder.Services.AddControllers();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", b => b.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<FIS_AuthenContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));


builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseRouting();
// Cấu hình pipeline xử lý HTTP request
app.UseAuthentication();  // Middleware xác thực
app.UseAuthorization();   // Middleware phân quyền

app.MapControllers();

app.Run();
