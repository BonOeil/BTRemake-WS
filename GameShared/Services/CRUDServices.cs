// <copyright file="CRUDServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameShared.Persistence;
    using GameShared.Services.Interfaces;

    public class CRUDServices<T> : ICRUDServices<T>
        where T : BaseEntity
    {
        public CRUDServices(IRepository<T> repository)
        {
            Repository = repository;
        }

        public IRepository<T> Repository { get; set; }

        public async Task Add(T itemToAdd)
        {
            await Repository.AddAsync(itemToAdd);
        }

        public async Task Delete(T itemToDelete)
        {
            await Repository.DeleteAsync(itemToDelete.Id);
        }

        public async Task Delete(Guid idToDelete)
        {
            await Repository.DeleteAsync(idToDelete);
        }
    }
}
