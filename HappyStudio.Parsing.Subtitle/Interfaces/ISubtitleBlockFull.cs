using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    interface ISubtitleBlockFull : ISubtitleBlockBasic
    {
        ISubtitleBlockProperties Properties { get; }
    }
}
