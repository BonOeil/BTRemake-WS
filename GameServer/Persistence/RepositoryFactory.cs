// <copyright file="RepositoryFactory.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Persistence
{
    using GameShared.Persistence;

    public class RepositoryFactory : IRepositoryFactory
    {
        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }

        private IServiceProvider Services { get; set; }

        public IRepository<T> Get<T>()
            where T : BaseEntity
        {
            return Services.GetService<IRepository<T>>() ?? throw new NullReferenceException();
        }
    }
}
