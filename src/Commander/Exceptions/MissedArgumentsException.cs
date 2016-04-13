﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander
{
    public class MissedArgumentsException : Exception
    {
        public readonly string[] ArgumentNames;
        public MissedArgumentsException(string[] argumentNames)
        {
            this.ArgumentNames = argumentNames;
        }
    }
}
