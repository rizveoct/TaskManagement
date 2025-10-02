using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Configuration;
using TaskManagement.API.Extensions;
using TaskManagement.Application;
using TaskManagement.Infrastructure;
using TaskManagement.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddSignalRHubs()
    .AddJwtAuthentication(builder.Configuration)
    .ConfigureCors(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(TaskManagement.API.Configuration.CorsOptionsBuilder.DefaultPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();
