﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Services.Interfaces
{
    public interface IGameManagement
    {
        Task StartScenario(string scenarioName, string gameName);
    }
}
