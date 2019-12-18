using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestParser {

        [Test]
        public void Parse_HasNoState() {
            Parser parser = new Parser(new Lexer("a := 1;"));
            Assert.True(parser.Parse() is Compound);
            Assert.True(parser.Parse() is Compound);
        }
    }

}