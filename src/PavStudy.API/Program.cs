using Microsoft.AspNetCore.Builder;
using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Services;
using PavStudy.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbDependencies(builder.Configuration).AddInfrastructureIdentity(builder.Configuration); 
builder.Services.AddDependencyInjection();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.Map("/", () => Results.Redirect("/api"));
app.MapEndpoints();
app.Run();
