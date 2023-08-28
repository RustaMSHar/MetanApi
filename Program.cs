using MetanApi.Models;
using MetanApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StoreDatabaseSettings>(
    builder.Configuration.GetSection("MetanDataBase"));

builder.Services.AddSingleton<ItemsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Add this line for JWT authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
