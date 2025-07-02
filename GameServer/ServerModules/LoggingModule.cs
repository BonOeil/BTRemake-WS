// <copyright file="LoggingModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    using Serilog;
    using Serilog.Sinks.Elasticsearch;

    public class LoggingModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            // Middleware de logging des requêtes HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSerilogRequestLogging();
            }
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            // Configuration de Serilog à partir du fichier appsettings.json
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                // .Enrich.FromLogContext()

                // ELASTICSEARCH SINK
                /*.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                {
                    IndexFormat = $"aspnet-logs-{context.HostingEnvironment.EnvironmentName}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1,
                    TemplateName = "aspnet-template",
                    TypeName = "_doc",
                    BatchAction = ElasticOpType.Index,
                    ModifyConnectionSettings = conn => conn.BasicAuthentication(string.Empty, string.Empty), // Pas d'auth en dev
                })*/

                .Enrich.WithProperty("Application", "mon-app-aspnet")
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName));
        }
    }
}
