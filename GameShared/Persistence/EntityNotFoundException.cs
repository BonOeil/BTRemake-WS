// <copyright file="EntityNotFoundException.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base()
        {
        }

        public EntityNotFoundException(string? message)
            : base(message)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
