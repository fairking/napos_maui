﻿using System;

namespace Napos.Core.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException() : base() { }

        public ForbidException(string message) : base(message) { }
    }
}
