﻿// <copyright file="Program.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

// See https://aka.ms/new-console-template for more information
#pragma warning disable SA1200 // Using directives should be placed correctly
using GameShared.Game;
using GameShared.Game.Entities;
using GameShared.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
#pragma warning restore SA1200 // Using directives should be placed correctly

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>().Build();
var mongoConnectionString = configuration["ConnectionStrings:MongoDb"];

var client = new MongoClient(mongoConnectionString);

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
// await AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
// await AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);

var serverUrl = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:8080";

// var serverUrl = "https://localhost:32790/GameHub";
var connection = new HubConnectionBuilder()
            .WithUrl($"{serverUrl}/GameHub")
            .WithAutomaticReconnect()
            .Build();

await connection.StartAsync();

await connection.InvokeAsync(nameof(LoadScenario), new LoadScenario
{
    InstanceName = "test",
    ScenarioName = "test",
});