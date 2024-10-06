using FluentValidation;
using HotelsWebAPI.DAL;
using HotelsWebAPI.Services.HotelService;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();

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

//Ensuring that Db is created and all Migrations are applied
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext != null)
        {
            dbContext.Database.SetCommandTimeout(300);
            dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}

//Implement global error handling
app.UseExceptionHandler(appError =>
{
    appError.Run(async errorContext =>
    {
        errorContext.Response.StatusCode = 500;
        errorContext.Response.ContentType = "application/json";

        var errorContextFeature = errorContext.Features.Get<IExceptionHandlerFeature>();

        //If there are unhandled errors, we log them and user gets a minimal, user friendly message
        //that does not reveal too much unnecessary information 
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
