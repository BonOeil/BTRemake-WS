// <copyright file="IRepositoryFactory.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistence
{
    public interface IRepositoryFactory
    {
        IRepository<T> Get<T>()
            where T : BaseEntity;
    }
}
