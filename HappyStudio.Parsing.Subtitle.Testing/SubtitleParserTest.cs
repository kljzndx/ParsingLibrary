using System.IO;
using NUnit.Framework;

namespace HappyStudio.Parsing.Subtitle.Testing
{
    public class SubtitleParserTest
    {
        [Test]
        public void Parse()
        {
            string srtContent = File.ReadAllText("./test.srt");
            string lrcContent = File.ReadAllText("./test.lrc");
            var srt = SubtitleParser.Parse(srtContent);
            var lrc = SubtitleParser.Parse(lrcContent);

            Assert.True(srt != null && lrc != null);
        }
    }
}