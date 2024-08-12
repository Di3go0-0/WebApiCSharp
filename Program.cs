using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Create a connection string to the database
var connectionString = builder.Configuration.GetConnectionString("Connection")
    ?? throw new InvalidOperationException("Connection string 'Connection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddControllers();

// Register AuthService
builder.Services.AddScoped<AuthService>();

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

app.Run();