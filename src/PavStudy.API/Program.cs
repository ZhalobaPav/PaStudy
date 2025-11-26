using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbDependencies(builder.Configuration).AddInfrastructureIdentity(builder.Configuration); 
builder.Services.AddDependencyInjection();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();   
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeederService>();
    await seeder.SeedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
