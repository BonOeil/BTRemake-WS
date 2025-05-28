// <copyright file="ScenarioLoaded.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameShared.Persistance
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> GetUniqueAsync();
        Task AddAsync(T entity);
        Task AddAsync(JsonDocument jsonElement);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
