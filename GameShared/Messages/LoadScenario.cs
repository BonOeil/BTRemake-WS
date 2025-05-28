// <copyright file="LoadScenario.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Messages
{
    public class LoadScenario
    {
        public required string ScenarioName { get; set; }
        public required string InstanceName { get; set; }
    }
}
