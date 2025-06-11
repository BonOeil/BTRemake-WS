// <copyright file="ICRUDService.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICRUDService<in T>
    {
        Task Add(T itemToAdd);

        Task Delete(T itemToDelete);

        Task Delete(Guid idToDelete);
    }
}
