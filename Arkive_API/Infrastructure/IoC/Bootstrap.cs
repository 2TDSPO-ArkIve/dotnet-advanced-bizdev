using Arkive_API.Application.Interfaces;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace Arkive_API.Infrastructure.IoC
{
    public static class Bootstrap
    {
        public static void AddLogApi(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: Path.Combine(AppContext.BaseDirectory, "logs", "api-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext}{NewLine}    {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            services.AddSerilog();
        }

        public static void AddTelemetria(this IServiceCollection services, IConfiguration configuration)
        {
            // Application Insights via OpenTelemetry. Só liga quando há connection string configurada
            // (env de teste / dev sem Azure ficam sem telemetria em vez de quebrar no boot).
            var connString = configuration["ApplicationInsights:ConnectionString"];
            if (!string.IsNullOrWhiteSpace(connString))
            {
                services.AddOpenTelemetry()
                    .UseAzureMonitor(options => options.ConnectionString = connString);
            }
        }

        public static void AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => {
                options.UseOracle(configuration.GetConnectionString("Oracle"));
            });

            services.AddScoped<ICategoriaDoencaRepository, CategoriaDoencaRepository>();
            services.AddScoped<ICategoriaDoencaUseCase, CategoriaDoencaUseCase>();
            services.AddScoped<IEspecieRepository, EspecieRepository>();
            services.AddScoped<IEspecieUseCase, EspecieUseCase>();
            services.AddScoped<IDoencaRepository, DoencaRepository>();
            services.AddScoped<IDoencaUseCase, DoencaUseCase>();
            services.AddScoped<IRacaRepository, RacaRepository>();
            services.AddScoped<IRacaUseCase, RacaUseCase>();
            services.AddScoped<IPredisposicaoRepository, PredisposicaoRepository>();
            services.AddScoped<IPredisposicaoUseCase, PredisposicaoUseCase>();
            services.AddScoped<IFeedbackNPSRepository, FeedbackNPSRepository>();
            services.AddScoped<IFeedbackNPSUseCase, FeedbackNPSUseCase>();
        }
    }
}
