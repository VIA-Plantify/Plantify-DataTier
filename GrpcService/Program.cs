using EFC.DataAccess;
using EFC.Repositories;
using GrpcService;
using GrpcService.Services;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<ISoilHumidityRepository, SoilHumidityRepository>();

builder.Services.AddDbContext<PlantifyContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcReflectionService();
app.MapGrpcService<UserService>();
app.MapGrpcService<PlantService>();
app.MapGrpcService<SoilHumidityService>();
app.MapGrpcService<AuthService>();
app.Run();