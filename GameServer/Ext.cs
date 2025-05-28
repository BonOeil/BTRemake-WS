// <copyright file="Ext.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer
{
    using OpenTelemetry;
    using OpenTelemetry.Exporter;
    using OpenTelemetry.Logs;
    using OpenTelemetry.Metrics;
    using OpenTelemetry.Trace;

    public static class Ext
    {
        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
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

            builder.AddOpenTelemetryExporters();

            return builder;
        }

        private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
        {
            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            // if (useOtlpExporter)
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
