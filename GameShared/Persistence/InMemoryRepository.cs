// <copyright file="InMemoryRepository.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistence
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly ConcurrentDictionary<Guid, T> _entities = new ConcurrentDictionary<Guid, T>();

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_entities.Values.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            if (!_entities.TryGetValue(id, out var entity))
            {
                throw new EntityNotFoundException();
            }

            return Task.FromResult(entity);
        }

        public Task<T> GetUniqueAsync()
        {
            return Task.FromResult(_entities.First().Value);
        }

        public Task AddAsync(T entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            _entities[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task AddAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            if (entity.Id == Guid.Empty || !_entities.ContainsKey(entity.Id))
            {
                throw new KeyNotFoundException($"Entity with id {entity.Id} not found");
            }

            _entities[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _entities.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
