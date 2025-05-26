using GameShared.Persistance;
using GameShared.Persistance.Mongo;
using GameShared.Services;
using GameShared.Services.Interfaces;
using MongoDB.Driver;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureOpenTelemetry();

            // Configuration de Serilog à partir du fichier appsettings.json
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                //.Enrich.FromLogContext()
                //.WriteTo.Console()
                .WriteTo.GrafanaLoki("http://loki:3100", labels: new[]
        {
            new LokiLabel { Key = "app", Value = "mon-app-aspnet" },
            new LokiLabel { Key = "environment", Value = context.HostingEnvironment.EnvironmentName }
        })
                );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("GameClientPolicy", policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("ServerSettings:AllowedOrigins").Get<string[]>())
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
            builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));
            builder.Services.AddScoped<ITurnServices, TurnServices>();
            builder.Services.AddScoped<IGameManagement, GameManagement>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Middleware de logging des requêtes HTTP
            //app.UseSerilogRequestLogging();

            // Configuration pour écouter sur toutes les interfaces réseau
            //app.UseUrls($"http://*:{GetServerPort()}");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("GameClientPolicy");

            app.MapHub<TestHub>($"/{nameof(TestHub)}");
            app.MapHub<GameHub>($"/{nameof(GameHub)}");
            app.MapGet("/", () => "Hello World!");

            // Exemple de logs pour tester
            app.MapGet("/test-logs", (ILogger<Program> logger) =>
            {
                logger.LogInformation("Test log Information");
                logger.LogWarning("Test log Warning");
                logger.LogError("Test log Error avec données: {UserId}", 123);

                return Results.Ok("Logs envoyés vers Loki !");
            });

            Log.Information("Server started");

            app.Run();
        }
    }
}