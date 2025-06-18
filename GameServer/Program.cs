// <copyright file="Program.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer
{
    using GameServer.ServerModule;
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

    public class Program
    {
        public static void Main(string[] args)
        {
            var serverModules = new List<IServerModule>()
            {
                new SwaggerModule(),
                new LoggingModule(),
                new ControllerSupportModule(),
            };

            var builder = WebApplication.CreateBuilder(args);

            serverModules.ForEach(module => module.PreBuild(builder));

            builder.ConfigureOpenTelemetry();

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
            builder.Services.AddScoped(typeof(ICRUDService<>), typeof(CRUDService<>));

            builder.Services.AddSignalR();

            var app = builder.Build();

            serverModules.ForEach(module => module.PostBuild(app));

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