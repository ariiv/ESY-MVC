using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = new ServiceCollection();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

services.AddControllers();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
