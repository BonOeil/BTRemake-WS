// <copyright file="MissionPlan.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameShared.Game.Entities;

    internal class MissionPlan : BaseEntity
    {
        public List<MissionUnit> Units { get; set; }

        public IPosition Target { get; set; }

        public IList<IPosition> InPath { get; set; }

        public IList<IPosition> OutPath { get; set; }
    }
}
