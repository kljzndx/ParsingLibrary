using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    interface ISubtitleBlockProperties
    {
        string GetProperty(string key);
        void SetProperty(string key, string value);
        string ToString();
    }
}
