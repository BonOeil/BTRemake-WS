// <copyright file="DebugModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModule
{
    public class DebugModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            // void
        }
    }
}
