using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HappyStudio.Parsing.Subtitle.SRT;
using System.IO;
using System.Linq;

namespace HappyStudio.Parsing.Subtitle.Testing
{
    public class SrtBlockTest
    {
        [Test]
        public void Parse()
        {
            string content = File.ReadAllText("./test.srt");
            SrtBlock block = new SrtBlock(content);
            Assert.Greater(block.Lines.Count(), 10);
        }
    }
}
