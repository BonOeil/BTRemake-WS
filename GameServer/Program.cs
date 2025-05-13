/**/

using GameShared.Persistance;
using GameShared.Persistance.Mongo;
using GameShared.Services;
using GameShared.Services.Interfaces;
using MongoDB.Driver;

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

            builder.Services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
            builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));
            builder.Services.AddScoped<ITurnServices, TurnServices>();
            builder.Services.AddScoped<IGameManagement, GameManagement>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configuration pour écouter sur toutes les interfaces réseau
            //app.UseUrls($"http://*:{GetServerPort()}");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("GameClientPolicy");

            app.MapHub<TestHub>($"/{nameof(TestHub)}");
            app.MapHub<GameHub>($"/{nameof(GameHub)}");
            app.MapGet("/", () => "Hello World!");

            Console.WriteLine("Le serveur de jeu est démarré!");

            app.Run();
        }
    }
}