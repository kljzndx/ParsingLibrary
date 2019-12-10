using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlock
    {
        IList<ISubtitleLine> Lines { get; }
        ISubtitleBlockProperties Properties { get; }

        string ToString();
    }
}
