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
        required public List<MissionUnit> Units { get; set; }

        required public IPosition Target { get; set; }

        required public IList<IPosition> InPath { get; set; }

        required public IList<IPosition> OutPath { get; set; }
    }
}
