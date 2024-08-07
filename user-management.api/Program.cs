using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using user_management.api.Extensions;
using user_management.api.Middlewares;
using user_management.core;
using user_management.domain.Entities;
using user_management.infrastructure;
using user_management.infrastructure.Seeder;
using user_management.infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureApplicationDatabase(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddApplicationCore();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//Configure logger
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Seq(context.Configuration["Serilog:WriteTo:0:Args:serverUrl"]));

var app = builder.Build();
Log.Information("Application is starting");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.ConfigureExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationContext>();
        var appuser = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        context.Database.Migrate();
        await DataSeeder.SeedData(context, appuser, roleManager);

    }
    catch (Exception ex)
    {
        // Log errors or handle exceptions
        Console.WriteLine(ex.Message);
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
