using EFC.DataAccess;
using EFC.Repositories;
using GrpcService;
using GrpcService.Services;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IWateringRepository, WateringRepository>();

builder.Services.AddDbContext<PlantifyContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(10081, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcReflectionService();
app.MapGrpcService<UserService>();
app.MapGrpcService<SensorService>();
app.MapGrpcService<PlantService>();
app.MapGrpcService<AuthService>();
app.MapGrpcService<WateringService>();
app.Run();