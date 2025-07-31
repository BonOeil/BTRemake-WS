// <copyright file="PaceService.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using GameShared.Services.Interfaces;

    public enum Pace
    {
        None = 0,
        Minutes,
        Hours,
        Days,
        Months,
    }

    public class PaceService : IPaceService
    {
        public const double MinutesToPace = 1;
        public const double HoursToPace = MinutesToPace / 60;
        public const double DaysToPace = HoursToPace / 24;
        public const double MonthToPace = DaysToPace / 30;

        public double ToPace(double value, Pace pace)
        {
            switch (pace)
            {
                case Pace.Minutes:
                    return MinutesToPace * value;
                case Pace.Hours:
                    return HoursToPace * value;
                case Pace.Days:
                    return DaysToPace * value;
                case Pace.Months:
                    return MonthToPace * value;

                default:
                    throw new NotSupportedException($"{pace} nor supported.");
            }
        }
    }
}
