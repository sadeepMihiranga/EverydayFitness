using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WorkoutService.Repositories;
using WorkoutService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database connection string
builder.Services.AddDbContext<WorkoutContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EverydayFitnessCS")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Interfaces and service implementations
builder.Services.AddScoped<IWorkoutService, WorkoutServiceImpl>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeServiceImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
