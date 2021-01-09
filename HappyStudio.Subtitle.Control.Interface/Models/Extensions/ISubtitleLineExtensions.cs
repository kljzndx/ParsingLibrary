using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.Interface.Models.Extensions
{
    public static class ISubtitleLineExtensions
    {
        public static IEnumerable<ISubtitleLineUi> ToLineUiList(this IEnumerable<ISubtitleLine> lineList)
        {
            return lineList.Select(l => new SubtitleLineUi(l)).ToList();
        }
    }
}
