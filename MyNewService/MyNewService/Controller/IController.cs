﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller
{
    public interface IController
    {
        string ExecuteCommand(CommandEnum commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}
