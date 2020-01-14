using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestTokenizer {

        [TestCase("1+1", ExpectedResult = "+1")]
        [TestCase("1-2+3", ExpectedResult = "-2+3")]
        public string Next_ConsumesOneToken(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Text;
        }

        [TestCase("1+1", ExpectedResult = "1")]
        [TestCase("1-2+3", ExpectedResult = "1")]
        public string Next_TokenIsUpdated(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Value;
        }

        [TestCase(" 1 + 2", 1, ExpectedResult = "1")]
        [TestCase("1   +3", 2, ExpectedResult = "+")]
        [TestCase("4 -  5", 3, ExpectedResult = "5")]
        public string Next_SkipWhiteSpace(string text, int n) {
            Tokenizer tokenizer = new Tokenizer(text);
            for (int i = 0; i < n; i++) {
                tokenizer = tokenizer.Next();
            }
            return tokenizer.Token.Value;
        }

        [TestCase("a1", ExpectedResult = TokenType.Id)]
        [TestCase("123", ExpectedResult = TokenType.IntegerConst)]
        [TestCase("+", ExpectedResult = TokenType.Plus)]
        [TestCase("-", ExpectedResult = TokenType.Minus)]
        [TestCase("*", ExpectedResult = TokenType.Asterisk)]
        [TestCase("/", ExpectedResult = TokenType.Slash)]
        [TestCase("(", ExpectedResult = TokenType.LeftParenthesis)]
        [TestCase(")", ExpectedResult = TokenType.RightParenthesis)]
        [TestCase("{", ExpectedResult = TokenType.LeftCurlyBracket)]
        [TestCase("}", ExpectedResult = TokenType.RightCurlyBracket)]
        [TestCase(";", ExpectedResult = TokenType.SemiColon)]
        [TestCase(":=", ExpectedResult = TokenType.Assign)]
        [TestCase("func", ExpectedResult = TokenType.Func)]
        [TestCase("int", ExpectedResult = TokenType.IntegerType)]
        [TestCase("float", ExpectedResult = TokenType.FloatType)]
        public TokenType Next_CorrectTokenType(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Type;
        }

        [TestCase("a1", ExpectedResult = "a1")]
        [TestCase("123", ExpectedResult = "123")]
        [TestCase("+", ExpectedResult = "+")]
        [TestCase("-", ExpectedResult = "-")]
        [TestCase("*", ExpectedResult = "*")]
        [TestCase("/", ExpectedResult = "/")]
        [TestCase("(", ExpectedResult = "(")]
        [TestCase(")", ExpectedResult = ")")]
        [TestCase("{", ExpectedResult = "{")]
        [TestCase("}", ExpectedResult = "}")]
        [TestCase(";", ExpectedResult = ";")]
        [TestCase(":=", ExpectedResult = ":=")]
        [TestCase("func", ExpectedResult = "func")]
        [TestCase("int", ExpectedResult = "int")]
        [TestCase("float", ExpectedResult = "float")]
        public string Next_CorrectTokenValue(string text) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            return tokenizer.Token.Value;
        }

        [TestCase("a := 2;", "a", 0, 1, 1)]
        [TestCase("a := 1;", ":=", 1, 1, 3)]
        [TestCase("a := a;", "a", 2, 1, 6)]
        public void Next_TokenPositionUpdated(string text, string token, int tokenIdx, int line, int column) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            for (int i = 0; i < tokenIdx; i++) {
                tokenizer = tokenizer.Next();
            }
            Assert.AreEqual(token, tokenizer.Token.Value);
            Assert.AreEqual(line, tokenizer.Token.Line);
            Assert.AreEqual(column, tokenizer.Token.Column);
        }

        [TestCase("1f", "1", 1f)]
        [TestCase("12F", "12", 12f)]
        [TestCase("1.", "1.", 1f)]
        [TestCase("1.2", "1.2", 1.2f)]
        [TestCase("1.2f", "1.2", 1.2f)]
        public void Next_FloatToken(string text, string str, float value) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            Assert.AreEqual(TokenType.FloatConst, tokenizer.Token.Type);
            Assert.AreEqual(str, tokenizer.Token.Value);
            Assert.AreEqual(value, float.Parse(tokenizer.Token.Value));
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        public void Next_BoolToken(string text, bool value) {
            Tokenizer tokenizer = new Tokenizer(text).Next();
            Assert.AreEqual(TokenType.BoolConst, tokenizer.Token.Type);
            Assert.AreEqual(value, bool.Parse(tokenizer.Token.Value));
        }

        [TestCase("#", 1, 1, '#')]
        [TestCase("a! := 1;", 1, 2, '!')]
        [TestCase("a := #4;", 1, 6, '#')]
        [TestCase("a := 1;\nb := 1%;", 2, 7, '%')]
        public void Next_UnrecognizedCharacter(string text, int errLine, int errCol, char errLexeme) {
            Tokenizer tokenizer = new Tokenizer(text);
            do {
                try {
                    tokenizer = tokenizer.Next();
                } catch (TokenizerError err) {
                    Assert.AreEqual(errLine, err.Line, "Wrong line number");
                    Assert.AreEqual(errCol, err.Column, "Wrong column number");
                    Assert.AreEqual(errLexeme, err.Lexeme, "Wrong lexeme");
                    break; // prevent infinite loop
                } catch (Exception ex) {
                    Assert.Fail(string.Format(
                        "Unrecognized exception thrown: ({0}): {1}",
                        ex.GetType(), ex.Message)
                    );
                }
            } while (tokenizer.Token.Type != TokenType.Eof);
        }
    }
}