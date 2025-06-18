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
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            var serverModules = new List<IServerModule>()
            {
                new SwaggerModule(),
                new LoggingModule(),
                new OpenTelemetryModule(),
                new DebugModule(),
                new CorsModule(),
                new ControllerSupportModule(),
                new HubModule(),
            };

            var builder = WebApplication.CreateBuilder(args);

            serverModules.ForEach(module => module.PreBuild(builder));

            builder.Services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
            builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));
            builder.Services.AddScoped<ITurnServices, TurnServices>();
            builder.Services.AddScoped<IGameManagement, GameManagement>();
            builder.Services.AddScoped(typeof(ICRUDService<>), typeof(CRUDService<>));

            var app = builder.Build();

            serverModules.ForEach(module => module.PostBuild(app));

            // Configuration pour écouter sur toutes les interfaces réseau
            // app.UseUrls($"http://*:{GetServerPort()}");

            app.UseRouting();
            app.MapGet("/", () => "Hello World!");

            Log.Information("Server started");

            app.Run();
        }
    }
}