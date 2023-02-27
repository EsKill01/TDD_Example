using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Extensions;
using TP.NA.UserService.Repository.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>
    {
        var fullname = type.FullName;
        fullname = fullname.Replace("+", ".");
        return fullname;
    });
    options.TagActionsBy(x =>
    {
        return new List<string>() { x.ActionDescriptor.DisplayName };
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.ConfigApp();
builder.WebAppBuilder();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.ConfigureApp();

app.Run();

public partial class Program
{ }