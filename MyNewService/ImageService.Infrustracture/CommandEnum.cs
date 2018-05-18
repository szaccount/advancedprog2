﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// commands enum
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        CloseCommand,
        CloseGuiClient,
        LogCommand,
        GetConfigCommand
    }
}