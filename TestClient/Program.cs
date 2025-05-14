// See https://aka.ms/new-console-template for more information
using GameShared.Game;
using GameShared.Game.Entities;
using GameShared.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

var client = new MongoClient("mongodb://localhost:27017/");
//var client = new MongoClient("mongodb://amazing_turing:27017/BTRemake-Game"); //mongodb://localhost:27017/



// Clear DB ? or create new DB as gameName
var dataBase = client.GetDatabase("gameName");

// Read locations

// Read OoB

// Save into DB
// await dataBase.CreateCollectionAsync(nameof(Location));
var collection = dataBase.GetCollection<Location>(nameof(Location));
Location location = new Location("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
await collection.InsertOneAsync(location);
location = new Location("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);
await collection.InsertOneAsync(location);
//await AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
//await AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);






//var serverUrl = "https://localhost:32782/GameHub";
//var connection = new HubConnectionBuilder()
//            .WithUrl(serverUrl)
//            .WithAutomaticReconnect()
//            .Build();


//await connection.StartAsync();

//await connection.InvokeAsync(nameof(LoadScenario), new LoadScenario
//{
//    InstanceName = "test",
//    ScenarioName = "test",
//});