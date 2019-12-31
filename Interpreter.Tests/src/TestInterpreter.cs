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
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 1 + 2 * 3;", ExpectedResult = 7)]
        [TestCase("a := 1 / 2 + 3;", ExpectedResult = 3)]
        [TestCase("a := 4 * 3 / 2;", ExpectedResult = 6)]
        public int Interpret_RespectsArithmeticPrecedence(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := (1 + 1) * 3;", ExpectedResult = 6)]
        [TestCase("a := 2 + (1 + 3) * 3;", ExpectedResult = 14)]
        public int Interprets_RespectParentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 3 * (2 + (5 - 3));", ExpectedResult = 12)]
        public int Interpret_HandleNestedParentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 5---2;", ExpectedResult = 3)]
        public int Interpret_HandleUnaryOperator(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("x := 2; a := x + 3;", ExpectedResult = 5)]
        [TestCase("a := 3; b := a * 2; a := b - 2;", ExpectedResult = 4)]
        public int Interpret_HandleVariableValues(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("a := 0; func One() { return 1; } a := One();", ExpectedResult = 1)]
        [TestCase("func Two() { return 2; } a := Two();", ExpectedResult = 2)]
        [TestCase("func Two() { return 2; } func Double(int n) { return 2 * n; } a := Double(Two());", ExpectedResult = 4)]
        public int Interpret_FunctionCall_Return(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }

        [TestCase("func Sum(int a, int b) { return a + b; } a := Sum(1, 2);", ExpectedResult = 3)]
        public int Interpret_FunctionCall_Parameters(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.ShouldPopStackAtEnd = false;
            interpreter.Interpret();
            return interpreter.GetGlobalVar("a");
        }
    }

}