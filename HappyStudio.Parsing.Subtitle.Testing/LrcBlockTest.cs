using NUnit.Framework;
using System.IO;
using System.Linq;
using HappyStudio.Parsing.Subtitle.LRC;

namespace Tests
{
    public class LrcBlockTest
    {
        [Test]
        public void Parse()
        {
            string text = File.ReadAllText("./test.lrc");
            LrcBlock lrcBlock = new LrcBlock(text);
            bool a = lrcBlock.Properties.AllProperties.Count() == 5;
            bool b = lrcBlock.Lines.Count() == 5;
            Assert.AreEqual(a && b, true);
        }
    }
}