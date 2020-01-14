using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestInterpreter {

        [TestCase("return 1+1;", ExpectedResult = 2)]
        [TestCase("return 2+3;", ExpectedResult = 5)]
        [TestCase("return 1-1;", ExpectedResult = 0)]
        [TestCase("return 2-3;", ExpectedResult = -1)]
        [TestCase("return 3*2;", ExpectedResult = 6)]
        [TestCase("return 4/2;", ExpectedResult = 2)]
        public int Interpret_Calculation(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 1 + 2 * 3;", ExpectedResult = 7)]
        [TestCase("return 1 / 2 + 3;", ExpectedResult = 3)]
        [TestCase("return 4 * 3 / 2;", ExpectedResult = 6)]
        public int Interpret_RespectsArithmeticPrecedence(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return (1 + 1) * 3;", ExpectedResult = 6)]
        [TestCase("return 2 + (1 + 3) * 3;", ExpectedResult = 14)]
        public int Interprets_RespectParentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 3 * (2 + (5 - 3));", ExpectedResult = 12)]
        public int Interpret_HandleNestedParentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 5---2;", ExpectedResult = 3)]
        public int Interpret_HandleUnaryOperator(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 2f;", ExpectedResult = 2)]
        [TestCase("return 2.5f;", ExpectedResult = 2.5f)]
        [TestCase("return 2.5f + 1f;", ExpectedResult = 3.5f)]
        public float Interpret_FloatCalculation(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (float) interpreter.Interpret();
        }

        [TestCase("a := false; return a;", ExpectedResult = false)]
        [TestCase("a := true; return a;", ExpectedResult = true)]
        public bool Interpret_BoolVariable(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("x := 2; return x + 3;", ExpectedResult = 5)]
        [TestCase("a := 3; b := a * 2; return b - 2;", ExpectedResult = 4)]
        public int Interpret_HandleVariableValues(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("a := 0; func One() { return 1; } a := One(); return a;", ExpectedResult = 1)]
        [TestCase("func Two() { return 2; } return Two();", ExpectedResult = 2)]
        [TestCase("func Two() { return 2; } func Double(int n) { return 2 * n; } return Double(Two());", ExpectedResult = 4)]
        public int Interpret_FunctionCall_Return(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("func Sum(int a, int b) { return a + b; } return Sum(1, 2);", ExpectedResult = 3)]
        public int Interpret_FunctionCall_Parameters(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }
    }

}