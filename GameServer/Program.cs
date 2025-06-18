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
                new DebugModule(),
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