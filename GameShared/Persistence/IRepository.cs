// <copyright file="IRepository.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetUniqueAsync();

        Task AddAsync(T entity);

        Task AddAsync(string jsonFilePath);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}
