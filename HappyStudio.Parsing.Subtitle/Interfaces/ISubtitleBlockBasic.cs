﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlockBasic
    {
        IEnumerable<ISubtitleLine> Lines { get; }
        string ToString();
    }
}
