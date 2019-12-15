using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlock
    {
        IEnumerable<ISubtitleLine> Lines { get; }
        ISubtitleBlockProperties Properties { get; }

        string ToString();
    }
}
