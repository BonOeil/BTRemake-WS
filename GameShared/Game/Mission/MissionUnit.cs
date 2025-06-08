// <copyright file="MissionUnit.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MissionUnit
    {
        public string Squadron { get; set; }

        public string Role { get; set; }

        public string PlaneType { get; set; }

        public ushort PlaneCount { get; set; }

        public ushort Altitude { get; set; }
    }
}
