using Schivei.Shop.Cart.Features;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ABaseController).Assembly)
    .AddControllersAsServices()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddRepos();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(new FileInfo(typeof(ABaseController).Assembly.Location).Directory!.FullName, "Schivei.Shop.Cart.Features.xml"), true);
    options.IncludeXmlComments(Path.Combine(new FileInfo(typeof(ICartRepository).Assembly.Location).Directory!.FullName, "Schivei.Shop.Cart.Infrastructure.xml"), true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
