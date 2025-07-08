// <copyright file="MongoRepository.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistence.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CsvHelper;
    using GameShared.Parsers;
    using MongoDB.Driver;

    public class MongoRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BTRemake-Game");
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<T> GetUniqueAsync()
        {
            return Task.FromResult(_collection.Find(_ => true).First());
        }

        public async Task AddAsync(T entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _collection.InsertOneAsync(entity);
        }

        public async Task AddAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<LocationClassMap>();
                var records = csv.GetRecords<T>().ToList();

                await _collection.InsertManyAsync(records);
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
