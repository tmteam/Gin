﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGin
{
    public class FakeLog : ILog
    {
        public void WriteError(string str) { }

        public void WriteMessage(string str) { }

        public void WriteWarning(string str) { }
    }
}
