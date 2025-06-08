// <copyright file="BaseEntity.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Bson;

    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = ObjectId.GenerateNewId();
        }

        public ObjectId Id { get; set; }
    }
}
