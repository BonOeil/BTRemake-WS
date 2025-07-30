// <copyright file="MissionPlan.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using System;
    using System.Collections.Generic;
    using GameShared.Persistence;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class MissionPlan : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unit ids. Link to <seealso cref="MissionUnit" />.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        required public List<Guid> UnitIds { get; set; }

        required public GPSPosition Target { get; set; }

        required public IList<GPSPosition> InPath { get; set; }

        required public IList<GPSPosition> OutPath { get; set; }
    }
}
