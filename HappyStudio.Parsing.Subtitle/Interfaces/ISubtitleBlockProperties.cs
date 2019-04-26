using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleBlockProperties
    {
        IEnumerable<KeyValuePair<string, string>> AllProperties { get; }

        string GetProperty(string key);
        void SetProperty(string key, string value);
        string ToString();

        /// <param name="firstOutputs">property keys that output first</param>
        string ToString(params string[] firstOutputs);
    }
}
