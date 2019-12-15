using NUnit.Framework;

namespace InterpreterPractice.Tests {

    [TestFixture]
    public class TestInterpreter {

        [TestCase("1+1", ExpectedResult=2)]
        [TestCase("2+3", ExpectedResult=5)]
        [TestCase("1-1", ExpectedResult=0)]
        [TestCase("2-3", ExpectedResult=-1)]
        [TestCase("3*2", ExpectedResult=6)]
        [TestCase("4/2", ExpectedResult=2)]
        public int Interpret_Calculation(string text) {
            return (int)new Interpreter(new Parser(new Lexer(text).Next())).Interpret();
        }

        [TestCase("1 + 2 * 3", ExpectedResult=7)]
        [TestCase("1 / 2 + 3", ExpectedResult=3)]
        [TestCase("4 * 3 / 2", ExpectedResult=6)]
        public int Parse_RespectsArithmeticPrecedence(string text) {
            return (int)new Interpreter(new Parser(new Lexer(text).Next())).Interpret();
        }

        [TestCase("(1 + 1) * 3", ExpectedResult=6)]
        [TestCase("2 + (1 + 3) * 3", ExpectedResult=14)]
        public int Parse_RespectParentheses(string text) {
            return (int)new Interpreter(new Parser(new Lexer(text).Next())).Interpret();
        }

        [TestCase("3 * (2 + (5 - 3))", ExpectedResult=12)]
        public int Parse_HandleNestedParentheses(string text) {
            return (int)new Interpreter(new Parser(new Lexer(text).Next())).Interpret();
        }

        [TestCase("5---2", ExpectedResult=3)]
        public int Parse_HandleUnaryOperator(string text) {
            return (int)new Interpreter(new Parser(new Lexer(text).Next())).Interpret();
        }
    }
}