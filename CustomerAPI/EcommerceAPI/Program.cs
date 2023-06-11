//using EcommerceAPI.Logger;
using Ecommerce.DAL.Domain;
using Ecommerce.DAL.DBContext;
using Ecommerce.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CustomerDBContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerConnectionString")));
builder.Services.AddScoped<ICustomerRepository, SQLCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, SQLOrderRepository>();
builder.Host.UseNLog();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
