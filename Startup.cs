using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using MetanApi.Models;
using MetanApi.Services;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Настройка Serilog
        var logLevel = LogEventLevel.Information;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .WriteTo.Console()
            .CreateLogger();

        // Чтение настроек из конфигурации
        services.Configure<StoreDatabaseSettings>(_configuration.GetSection("MetanDataBase"));

        // Добавление сервисов в контейнер зависимостей
        services.AddSingleton<ItemsService>();
        services.AddSingleton<ImageService>();

        //services.AddCoreAdmin();

        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication(); //аутентификации JWT
        app.UseAuthorization();

        // Настройка политики CORS для разрешения запросов с любых источников
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //app.UseCoreAdminCustomUrl("adminpanel");
    }
}
