using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registering northwind context to the DIC
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration["default"]));
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"));

//Adding service DIC
builder.Services.AddScoped<IServiceSupplier, SupplierService>();
//optional
builder.Services.AddControllersWithViews()
.AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //returns exceptions as html
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
