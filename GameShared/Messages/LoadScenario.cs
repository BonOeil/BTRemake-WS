// <copyright file="LoadScenario.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LoadScenario
    {
        required public string ScenarioName { get; set; }

        required public string InstanceName { get; set; }
    }
}
