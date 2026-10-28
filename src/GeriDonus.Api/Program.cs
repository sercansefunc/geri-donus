using Microsoft.EntityFrameworkCore;
using GeriDonus.Data.Entities;
using GeriDonus.Data.Repositories;
using GeriDonus.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entity Framework
builder.Services.AddDbContext<GeriDonusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GeriDonus")));

builder.Services.AddScoped<BasvuruRepository>();
builder.Services.AddScoped<BasvuruService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
