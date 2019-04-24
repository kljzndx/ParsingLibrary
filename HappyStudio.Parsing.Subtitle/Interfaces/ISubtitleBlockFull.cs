using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlockFull : ISubtitleBlockBasic
    {
        ISubtitleBlockProperties Properties { get; }
    }
}
