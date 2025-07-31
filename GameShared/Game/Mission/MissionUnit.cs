// <copyright file="MissionUnit.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using GameShared.Game.OOB;
    using GameShared.Persistence;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class MissionUnit : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unit ids. Link to <seealso cref="PlaneSquadron" />.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public IList<Guid> PlaneIds { get; set; } = new List<Guid>();

        required public GPSPosition Position { get; set; }

        public double Orientation { get; set; }

        public ushort Altitude { get; set; }
    }
}
