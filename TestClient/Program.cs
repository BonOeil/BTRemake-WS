// See https://aka.ms/new-console-template for more information
using GameShared.Game;
using Microsoft.AspNetCore.SignalR.Client;
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

var client = new MongoClient("mongodb://localhost:27017/");
//var client = new MongoClient("mongodb://amazing_turing:27017/BTRemake-Game"); //mongodb://localhost:27017/

var database = client.GetDatabase("BTRemake-Game");

var _collection = database.GetCollection<TurnData>(typeof(TurnData).Name);

await _collection.InsertOneAsync(new TurnData());



//var serverUrl = "https://localhost:7086/gamehub";
//var connection = new HubConnectionBuilder()
//            .WithUrl(serverUrl)
//            .WithAutomaticReconnect()
//            .Build();


//await connection.StartAsync();

//await connection.InvokeAsync("StepTurn");