// <copyright file="InMemoryRepository.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistance
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly ConcurrentDictionary<string, T> _entities = new();
        private readonly PropertyInfo _idProperty;

        public InMemoryRepository()
        {
            // Recherche une propriété Id ou EntityId dans l'entité
            _idProperty = typeof(T).GetProperty("Id") ??
                          typeof(T).GetProperty($"{typeof(T).Name}Id") ??
                          throw new InvalidOperationException($"Entity {typeof(T).Name} must have an Id property");
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_entities.Values.AsEnumerable());
        }

        public Task<T> GetByIdAsync(string id)
        {
            _entities.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public Task<T> GetUniqueAsync()
        {
            return Task.FromResult(_entities.First().Value);
        }

        public Task AddAsync(T entity)
        {
            var id = _idProperty.GetValue(entity)?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                _idProperty.SetValue(entity, id);
            }

            _entities[id] = entity;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            var id = _idProperty.GetValue(entity)?.ToString();
            if (string.IsNullOrEmpty(id) || !_entities.ContainsKey(id))
            {
                throw new KeyNotFoundException($"Entity with id {id} not found");
            }

            _entities[id] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _entities.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task AddAsync(JsonDocument jsonElement)
        {
            throw new NotImplementedException();
        }
    }
}
