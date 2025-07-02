// <copyright file="CorsModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    public class CorsModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            app.UseCors("GameClientPolicy");
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
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
        }
    }
}
