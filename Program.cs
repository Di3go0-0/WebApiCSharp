using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Connection")
    ?? throw new InvalidOperationException("Connection string 'Connection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddControllers();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register AuthService
builder.Services.AddScoped<AuthService>();
// Register TaskService
// builder.Services.AddScoped<TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();