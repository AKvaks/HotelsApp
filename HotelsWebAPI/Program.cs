using FluentValidation;
using HotelsWebAPI.DAL;
using HotelsWebAPI.Entities;
using HotelsWebAPI.Services.HotelService;
using HotelsWebAPI.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//GetConnectionString method will return null if DefaultConnection string cannot be found
//and in that case exception will be thrown
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");

// Add services to the container.
// Add ApplicationDbContext with NetTopologySuite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString,
            x => x.UseNetTopologySuite());
});

//Add MediatR
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

//Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

//Add HotelService
builder.Services.AddScoped<IHotelService, HotelService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Enabling descriptions for methods in Swagger
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HotelsWebAPI",
        Version = "v1",
        Description = "This is a .NET Core Web API designed as part of a technical interview.",
    });
});

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

//Ensuring that database is created and all Migrations are applied
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext != null)
        {
            dbContext.Database.SetCommandTimeout(300);
            dbContext.Database.Migrate();

            //Enables easy seeding of database
            //await SeedHotels(dbContext).ConfigureAwait(false);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}

//Implements global error handling
app.UseExceptionHandler(appError =>
{
    appError.Run(async errorContext =>
    {
        errorContext.Response.StatusCode = 500;
        errorContext.Response.ContentType = "application/json";

        var errorContextFeature = errorContext.Features.Get<IExceptionHandlerFeature>();

        //If there are unhandled exceptions, we log them and user gets a minimal, user friendly message
        //that does not reveal too much information
        if (errorContextFeature != null)
        {
            Console.WriteLine($"Error: {errorContextFeature.Error}");

            await errorContext.Response.WriteAsJsonAsync(new
            {
                StatusCore = errorContext.Response.StatusCode,
                Message = "Internal Server Error"
            });
        }
    });
});

app.Run();

static async Task SeedHotels(ApplicationDbContext _dbContext)
{
    if (await _dbContext.Hotels.AnyAsync().ConfigureAwait(false) == false)
    {
        List<Hotel> hotels = new List<Hotel>()
        {
            new Hotel { HotelName = "Esplanade Zagreb Hotel 1", Price = 200m, Location = GeoUtils.CreatePoint(45.80740559761584, 15.975518240751384) },
            new Hotel { HotelName = "Esplanade Zagreb Hotel 2", Price = 300m, Location = GeoUtils.CreatePoint(45.80740559761584, 15.975518240751384) },
            new Hotel { HotelName = "Esplanade Zagreb Hotel 3", Price = 150m, Location = GeoUtils.CreatePoint(45.80740559761584, 15.975518240751384) },
            new Hotel { HotelName = "Hotel Croatia 1", Price = 118m, Location = GeoUtils.CreatePoint(45.81646837913312, 15.9543182559186) },
            new Hotel { HotelName = "Hotel Croatia 2", Price = 100m, Location = GeoUtils.CreatePoint(45.81646837913312, 15.9543182559186) },
            new Hotel { HotelName = "Hotel Croatia 3", Price = 150m, Location = GeoUtils.CreatePoint(45.81646837913312, 15.9543182559186) },
            new Hotel { HotelName = "Sheraton Zagreb Hotel 1", Price = 130.52m, Location = GeoUtils.CreatePoint(45.807133668188975, 15.984587653040613) },
            new Hotel { HotelName = "Sheraton Zagreb Hotel 2", Price = 129.99m, Location = GeoUtils.CreatePoint(45.807133668188975, 15.984587653040613) },
            new Hotel { HotelName = "Sheraton Zagreb Hotel 3", Price = 130.05m, Location = GeoUtils.CreatePoint(45.807133668188975, 15.984587653040613) },
            new Hotel { HotelName = "The Westin Zagreb 1", Price = 109m, Location = GeoUtils.CreatePoint(45.80691893299671, 15.966249221299845) },
            new Hotel { HotelName = "The Westin Zagreb 2", Price = 109.99m, Location = GeoUtils.CreatePoint(45.80691893299671, 15.966249221299845) },
            new Hotel { HotelName = "The Westin Zagreb 3", Price = 3000m, Location = GeoUtils.CreatePoint(45.80691893299671, 15.966249221299845) },
            new Hotel { HotelName = "Hotel Antunovic 1", Price = 130m, Location = GeoUtils.CreatePoint(45.79755982747399, 15.898817787145727) },
            new Hotel { HotelName = "Hotel Antunovic 2", Price = 150m, Location = GeoUtils.CreatePoint(45.79755982747399, 15.898817787145727) },
            new Hotel { HotelName = "Hotel Antunovic 3", Price = 125m, Location = GeoUtils.CreatePoint(45.79755982747399, 15.898817787145727) },
            new Hotel { HotelName = "Palace Hotel 1", Price = 69.99m, Location = GeoUtils.CreatePoint(45.808662113917215, 15.977727633752567) },
            new Hotel { HotelName = "Palace Hotel 2", Price = 70m, Location = GeoUtils.CreatePoint(45.808662113917215, 15.977727633752567) },
            new Hotel { HotelName = "Palace Hotel 3", Price = 68m, Location = GeoUtils.CreatePoint(45.808662113917215, 15.977727633752567) },
            new Hotel { HotelName = "Hotel Dubovnik 1", Price = 200m, Location = GeoUtils.CreatePoint(45.81272448782883, 15.976518707236014) },
            new Hotel { HotelName = "Hotel Dubovnik 2", Price = 140m, Location = GeoUtils.CreatePoint(45.81272448782883, 15.976518707236014) },
            new Hotel { HotelName = "Hotel Dubovnik 3", Price = 128m, Location = GeoUtils.CreatePoint(45.81272448782883, 15.976518707236014) },
            new Hotel { HotelName = "Hotel Laguna 1", Price = 63m, Location = GeoUtils.CreatePoint(45.803943303449614, 15.958401120032823) },
            new Hotel { HotelName = "Hotel Laguna 2", Price = 59m, Location = GeoUtils.CreatePoint(45.803943303449614, 15.958401120032823) },
            new Hotel { HotelName = "Hotel Laguna 3", Price = 150m, Location = GeoUtils.CreatePoint(45.803943303449614, 15.958401120032823) },
        };

        await _dbContext.Hotels.AddRangeAsync(hotels);
        await _dbContext.SaveChangesAsync();
    }
}