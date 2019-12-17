using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestInterpreter {

        [TestCase("a := 1+1;", ExpectedResult = 2)]
        [TestCase("a := 2+3;", ExpectedResult = 5)]
        [TestCase("a := 1-1;", ExpectedResult = 0)]
        [TestCase("a := 2-3;", ExpectedResult = -1)]
        [TestCase("a := 3*2;", ExpectedResult = 6)]
        [TestCase("a := 4/2;", ExpectedResult = 2)]
        public int Interpret_Calculation(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 1 + 2 * 3;", ExpectedResult = 7)]
        [TestCase("a := 1 / 2 + 3;", ExpectedResult = 3)]
        [TestCase("a := 4 * 3 / 2;", ExpectedResult = 6)]
        public int Parse_RespectsArithmeticPrecedence(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := (1 + 1) * 3;", ExpectedResult = 6)]
        [TestCase("a := 2 + (1 + 3) * 3;", ExpectedResult = 14)]
        public int Parse_RespectParentheses(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 3 * (2 + (5 - 3));", ExpectedResult = 12)]
        public int Parse_HandleNestedParentheses(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 5---2;", ExpectedResult = 3)]
        public int Parse_HandleUnaryOperator(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("x := 2; a := x + 3;", ExpectedResult = 5)]
        [TestCase("a := 3; b := a * 2; a := b - 2;", ExpectedResult = 4)]
        public int Parse_HandleVariableValues(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text).Next()).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }
    }

}