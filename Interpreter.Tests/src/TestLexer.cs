using NUnit.Framework;

namespace InterpreterPractice.Tests {

    [TestFixture]
    public class TestLexer {

        [Test]
        public void New_EmptyString() {
            Assert.IsNull(Lexer.New(""));
        }

        [TestCase("1", ExpectedResult="")]
        [TestCase("1+1", ExpectedResult="+1")]
        public string New_TextIsOneLessChar(string text) {
            return Lexer.New(text).Text;
        }

        [TestCase("0", ExpectedResult="0")]
        [TestCase("1+2", ExpectedResult="1")]
        public string New_TokenIsFirstToken(string text) {
            return Lexer.New(text).Token.Value;
        }

        [TestCase("1+1", ExpectedResult="+1")]
        [TestCase("1-2+3", ExpectedResult="-2+3")]
        public string Next_ConsumesOneToken(string text) {
            Lexer lexer = new Lexer(text).Next();
            return lexer.Text;
        }

        [TestCase("1+1", ExpectedResult="1")]
        [TestCase("1-2+3", ExpectedResult="1")]
        public string Next_TokenIsUpdated(string text) {
            Lexer lexer = new Lexer(text).Next();
            return lexer.Token.Value;
        }

        [TestCase(" 1 + 2", 1, ExpectedResult="1")]
        [TestCase("1   +3", 2, ExpectedResult="+")]
        [TestCase("4 -  5", 3, ExpectedResult="5")]
        public string Next_SkipWhiteSpace(string text, int n) {
            Lexer lexer = new Lexer(text);
            for (int i = 0; i < n; i++) {
                lexer = lexer.Next();
            }
            return lexer.Token.Value;
        }

        [TestCase("12", ExpectedResult="12")]
        [TestCase("143+2", ExpectedResult="143")]
        public string Next_HandlesMultiDigitNumber(string text) {
            Lexer lexer = new Lexer(text).Next();
            return lexer.Token.Value;
        }
    }
}