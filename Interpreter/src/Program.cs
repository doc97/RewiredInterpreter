using System;
using Rewired.Interpreter;

public class Program {

    public static void Main(string[] args) {
        while (true) {
            Console.Write("$ ");
            string input = Console.ReadLine();
            try {
                Tokenizer tokenizer = new Tokenizer(input);
                Parser parser = new Parser(tokenizer);
                AbstractSyntaxTreeNode tree = parser.Parse();

                SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
                analyzer.ShouldExitRootScope = false;
                analyzer.IsLoggingEnabled = true;
                analyzer.Analyze();
                analyzer.PrintScopeInfo();

                Interpreter interpreter = new Interpreter(tree);
                interpreter.ShouldPopStackAtEnd = false;
                interpreter.IsLoggingEnabled = true;
                interpreter.Interpret();
                interpreter.PrintCallStack();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
