using System;
using System.Collections.Generic;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlock
    {
        IList<ISubtitleLine> Lines { get; }
        ISubtitleBlockProperties Properties { get; }

        string ToString();
    }
}
