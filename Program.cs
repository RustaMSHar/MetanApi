using Serilog;
using Serilog.Events;
using MetanApi.Models;
using MetanApi.Services;
using System.Text;

var logLevel = LogEventLevel.Information; // ”становле уровень логировани€

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(logLevel)
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StoreDatabaseSettings>(
    builder.Configuration.GetSection("MetanDataBase"));

builder.Services.AddSingleton<ItemsService>();
builder.Services.AddSingleton<ImageService>(); // ƒобавл€ем сервис дл€ работы с изображени€ми

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

// Configure CORS policy to allow requests from any origin.
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.MapControllers();

app.Run();
