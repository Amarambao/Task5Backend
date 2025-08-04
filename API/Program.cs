using API.Interfaces;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookGeneratorService, BookGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var corsSection = app.Configuration.GetSection("CORS");
var corsChildren = corsSection.GetChildren().ToList();

app.UseCors(options =>
{
    foreach (var child in corsChildren)
    {
        var url = child["URL"];

        options.WithOrigins(url!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();
