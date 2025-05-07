/**/

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configuration pour �couter sur toutes les interfaces r�seau
            //app.UseUrls($"http://*:{GetServerPort()}");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("GameClientPolicy");

            app.MapHub<GameHub>("/gamehub");
            app.MapGet("/", () => "Hello World!");

            Console.WriteLine("Le serveur de jeu est d�marr�!");

            app.Run();
        }

        private static int GetServerPort()
        {
            // R�cup�rer le port depuis la configuration ou utiliser la variable d'environnement
            // pour les d�ploiements en production
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return config.GetValue<int>("ServerSettings:Port");
        }
    }
}