// <copyright file="MongoRepository.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistance.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoClient _mongoClient;

        public MongoRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            var database = mongoClient.GetDatabase("BTRemake-Game");
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<T> GetUniqueAsync()
        {
            return Task.FromResult(_collection.Find(_ => true).First());
        }

        public async Task AddAsync(T entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.CreateVersion7();
            }

            await _collection.InsertOneAsync(entity);
        }

        public async Task AddAsync(JsonDocument jsonElement)
        {
            var database = _mongoClient.GetDatabase("BTRemake-Game");
            var collection = database.GetCollection<BsonDocument>(typeof(T).Name);

            foreach (var itemToInsert in jsonElement.RootElement.EnumerateArray())
            {
                var bsonDocument = itemToInsert.ToBsonDocument();
                await collection.InsertOneAsync(bsonDocument);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(nameof(BaseEntity.Id), entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(nameof(BaseEntity.Id), id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
