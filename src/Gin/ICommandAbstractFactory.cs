﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheGin
{
    public interface ICommandAbstractFactory {
        ICommand GetReadyToGoInstance();
    }

   

}