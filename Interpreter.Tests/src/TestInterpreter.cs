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
        public int Interpret_Int_Calculation(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 1 + 2 * 3;", ExpectedResult = 7)]
        [TestCase("return 1 / 2 + 3;", ExpectedResult = 3)]
        [TestCase("return 4 * 3 / 2;", ExpectedResult = 6)]
        public int Interpret_Int_ArithmeticPrecedence(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return (1 + 1) * 3;", ExpectedResult = 6)]
        [TestCase("return 2 + (1 + 3) * 3;", ExpectedResult = 14)]
        public int Interprets_Int_Parentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 3 * (2 + (5 - 3));", ExpectedResult = 12)]
        public int Interpret_Int_NestedParentheses(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 5---2;", ExpectedResult = 3)]
        public int Interpret_Int_UnaryOperator(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("return 2f;", ExpectedResult = 2)]
        [TestCase("return 2.5f;", ExpectedResult = 2.5f)]
        [TestCase("return 2.5f + 1f;", ExpectedResult = 3.5f)]
        public float Interpret_Float_Calculation(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (float) interpreter.Interpret();
        }

        [TestCase("bool a := false; return a;", ExpectedResult = false)]
        [TestCase("bool a := true; return a;", ExpectedResult = true)]
        public bool Interpret_Bool_Variable(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("return !false;", ExpectedResult = true)]
        [TestCase("return !true;", ExpectedResult = false)]
        [TestCase("bool a := false; return !a;", ExpectedResult = true)]
        [TestCase("bool a := true; return !a;", ExpectedResult = false)]
        public bool Interpret_Bool_Negation(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("bool t := true; return !(t);", ExpectedResult = false)]
        [TestCase("bool t := true; return (!t);", ExpectedResult = false)]
        [TestCase("bool t := true; return (!(t));", ExpectedResult = false)]
        [TestCase("bool t := true; return !(!(t));", ExpectedResult = true)]
        [TestCase("bool f := false; return !(f);", ExpectedResult = true)]
        [TestCase("bool f := false; return (!f);", ExpectedResult = true)]
        [TestCase("bool f := false; return (!(f));", ExpectedResult = true)]
        [TestCase("bool f := false; return !(!(f));", ExpectedResult = false)]
        [TestCase("return !(true && false);", ExpectedResult = true)]
        public bool Interpret_Bool_Parenthesis(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("return true && false;", ExpectedResult = false)]
        [TestCase("return true || false;", ExpectedResult = true)]
        [TestCase("return !false || !true;", ExpectedResult = true)]
        public bool Interpret_Bool_LogicalOperators(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("return 1 < 2;", ExpectedResult = true)]
        [TestCase("return 1 > 2;", ExpectedResult = false)]
        [TestCase("return 1 <= 2;", ExpectedResult = true)]
        [TestCase("return 1 >= 2;", ExpectedResult = false)]
        [TestCase("return 1 == 2;", ExpectedResult = false)]
        [TestCase("return 1 != 2;", ExpectedResult = true)]
        [TestCase("int a := 2; return a + 3 == 5;", ExpectedResult = true)]
        [TestCase("func Sum(int a, int b) { return a + b; } return Sum(3, 2) < 5;", ExpectedResult = false)]
        public bool Interpret_Bool_ConditionalOperators(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (bool) interpreter.Interpret();
        }

        [TestCase("int x := 2; return x + 3;", ExpectedResult = 5)]
        [TestCase("int a := 3; int b := a * 2; return b - 2;", ExpectedResult = 4)]
        public int Interpret_HandleVariableValues(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("if true { return 1; }", ExpectedResult = 1)]
        [TestCase("if false { return 2; }", ExpectedResult = null)]
        [TestCase("if false { return 2; } else { return 3; }", ExpectedResult = 3)]
        public int? Interpret_If(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int?) interpreter.Interpret();
        }

        [TestCase("bool b := true; if b { return 1; } else { return 0; }", ExpectedResult = 1)]
        [TestCase("bool b := false; if b { return 1; } else { return 0; }", ExpectedResult = 0)]
        public int Interpret_If_Variable(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            Interpreter interpreter = new Interpreter(tree);
            return (int) interpreter.Interpret();
        }

        [TestCase("int a := 0; func One() { return 1; } int a := One(); return a;", ExpectedResult = 1)]
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