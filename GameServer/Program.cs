// <copyright file="Program.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer
{
    using GameShared.Persistence;
    using GameShared.Persistence.Mongo;
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
    using Serilog.Sinks.Elasticsearch;
    using Serilog.Sinks.Grafana.Loki;

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
                // .Enrich.FromLogContext()

                // LOKI SINK
                .WriteTo.GrafanaLoki("http://loki:3100", labels: new[]
                {
                    new LokiLabel { Key = "app", Value = "mon-app-aspnet" },
                    new LokiLabel { Key = "environment", Value = context.HostingEnvironment.EnvironmentName },
                })

                // ELASTICSEARCH SINK
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                {
                    IndexFormat = $"aspnet-logs-{context.HostingEnvironment.EnvironmentName}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1,
                    TemplateName = "aspnet-template",
                    TypeName = "_doc",
                    BatchAction = ElasticOpType.Index,
                    ModifyConnectionSettings = conn => conn.BasicAuthentication(string.Empty, string.Empty), // Pas d'auth en dev
                })

                .Enrich.WithProperty("Application", "mon-app-aspnet")
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("GameClientPolicy", policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("ServerSettings:AllowedOrigins").Get<string[]>() ?? [])
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
            // app.UseSerilogRequestLogging();

            // Configuration pour écouter sur toutes les interfaces réseau
            // app.UseUrls($"http://*:{GetServerPort()}");

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
                var userId = Random.Shared.Next(1, 1000);
                var transactionId = Guid.NewGuid().ToString("N")[..8];

                logger.LogInformation("User {UserId} started transaction {TransactionId}", userId, transactionId);
                logger.LogWarning("Slow query detected for user {UserId} - Duration: {Duration}ms", userId, 1500);
                logger.LogError(
                    "Payment failed for transaction {TransactionId} - Error: {Error}",
                    transactionId,
                    "Insufficient funds");

                // Log structuré avec plusieurs propriétés
                logger.LogInformation("Order processed {@Order}", new
                {
                    OrderId = Random.Shared.Next(1000, 9999),
                    UserId = userId,
                    TransactionId = transactionId,
                    Amount = Random.Shared.Next(10, 500),
                    Currency = "EUR",
                    ProcessedAt = DateTime.UtcNow,
                });

                return Results.Ok(new
                {
                    Message = "Logs envoyés vers Loki ET Elasticsearch !",
                    UserId = userId,
                    TransactionId = transactionId,
                    Timestamp = DateTime.UtcNow,
                });
            });

            app.MapGet("/test-performance", (ILogger<Program> logger) =>
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // Simuler du traitement
                Thread.Sleep(Random.Shared.Next(100, 500));

                stopwatch.Stop();

                logger.LogInformation("Performance test completed in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);

                return Results.Ok(new { ElapsedMs = stopwatch.ElapsedMilliseconds });
            });

            app.MapGet("/test-error", (ILogger<Program> logger) =>
            {
                try
                {
                    throw new InvalidOperationException("Erreur simulée pour test");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erreur capturée dans /test-error");
                    return Results.Problem("Erreur simulée capturée et loggée");
                }
            });

            Log.Information("Server started");

            app.Run();
        }
    }
}