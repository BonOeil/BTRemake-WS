using GameShared.Persistance;
using GameShared.Persistance.Mongo;
using GameShared.Services;
using GameShared.Services.Interfaces;
using MongoDB.Driver;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Définir les attributs du service
            var serviceName = "MonServiceASP";
            var serviceVersion = "1.0.0";
            // Ajouter les services OpenTelemetry
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter())
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter());

            // Configuration de Serilog à partir du fichier appsettings.json
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                //.Enrich.FromLogContext()
                //.WriteTo.Console()
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

            Log.Information("Server started");

            app.Run();
        }
    }
}