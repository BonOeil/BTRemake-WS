// <copyright file="BaseEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GameShared.Game.Entities
{
    public abstract class BaseEntity
    {
        public ObjectId Id { get; set; }

        protected BaseEntity() 
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}
