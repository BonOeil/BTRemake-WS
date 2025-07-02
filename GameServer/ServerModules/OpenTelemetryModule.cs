// <copyright file="OpenTelemetryModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    using OpenTelemetry;
    using OpenTelemetry.Exporter;
    using OpenTelemetry.Metrics;
    using OpenTelemetry.Trace;

    public class OpenTelemetryModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            // Void
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            ConfigureOpenTelemetry(builder);
        }

        public IHostApplicationBuilder ConfigureOpenTelemetry(IHostApplicationBuilder builder)
        {
            builder.Services.Configure<OtlpExporterOptions>(
                    o => o.Headers = $"x-otlp-api-key={Environment.GetEnvironmentVariable("DASHBOARD__OTLP__AUTHMODE")}");

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation()

                        // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                        // .AddGrpcClientInstrumentation()
                        .AddHttpClientInstrumentation();
                })
                /*.WithLogging(logging => logging
                .AddOtlpExporter())*/;

            AddOpenTelemetryExporters(builder);

            return builder;
        }

        private IHostApplicationBuilder AddOpenTelemetryExporters(IHostApplicationBuilder builder)
        {
            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExporter)
            {
                builder.Services.AddOpenTelemetry().UseOtlpExporter();
            }

            // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
            // if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            // {
            //    builder.Services.AddOpenTelemetry()
            //       .UseAzureMonitor();
            // }

            return builder;
        }
    }
}
