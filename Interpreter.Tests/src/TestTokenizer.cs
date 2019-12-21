using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestTokenizer {

        [TestCase("1+1", ExpectedResult="+1")]
        [TestCase("1-2+3", ExpectedResult="-2+3")]
        public string Next_ConsumesOneToken(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Text;
        }

        [TestCase("1+1", ExpectedResult="1")]
        [TestCase("1-2+3", ExpectedResult="1")]
        public string Next_TokenIsUpdated(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Value;
        }

        [TestCase(" 1 + 2", 1, ExpectedResult="1")]
        [TestCase("1   +3", 2, ExpectedResult="+")]
        [TestCase("4 -  5", 3, ExpectedResult="5")]
        public string Next_SkipWhiteSpace(string text, int n) {
            Tokenizer tokenizer = new Tokenizer(text);
            for (int i = 0; i < n; i++) {
                tokenizer = tokenizer.Next();
            }
            return tokenizer.Token.Value;
        }

        [TestCase("a1", ExpectedResult=TokenType.Id)]
        [TestCase("123", ExpectedResult=TokenType.Integer)]
        [TestCase("+", ExpectedResult=TokenType.Plus)]
        [TestCase("-", ExpectedResult=TokenType.Minus)]
        [TestCase("*", ExpectedResult=TokenType.Asterisk)]
        [TestCase("/", ExpectedResult=TokenType.Slash)]
        [TestCase("(", ExpectedResult=TokenType.LeftParenthesis)]
        [TestCase(")", ExpectedResult=TokenType.RightParenthesis)]
        [TestCase("{", ExpectedResult=TokenType.LeftCurlyBracket)]
        [TestCase("}", ExpectedResult=TokenType.RightCurlyBracket)]
        [TestCase(";", ExpectedResult=TokenType.SemiColon)]
        [TestCase(":=", ExpectedResult=TokenType.Assign)]
        [TestCase("func", ExpectedResult=TokenType.Func)]
        public TokenType Next_CorrectTokenType(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Type;
        }

        [TestCase("a1", ExpectedResult="a1")]
        [TestCase("123", ExpectedResult="123")]
        [TestCase("+", ExpectedResult="+")]
        [TestCase("-", ExpectedResult="-")]
        [TestCase("*", ExpectedResult="*")]
        [TestCase("/", ExpectedResult="/")]
        [TestCase("(", ExpectedResult="(")]
        [TestCase(")", ExpectedResult=")")]
        [TestCase("{", ExpectedResult="{")]
        [TestCase("}", ExpectedResult="}")]
        [TestCase(";", ExpectedResult=";")]
        [TestCase(":=", ExpectedResult=":=")]
        [TestCase("func", ExpectedResult="func")]
        public string Next_CorrectTokenValue(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Value;
        }
    }
}