using System;
using Rewired.Interpreter;

public class Program {

    // Indicates whether debug information should be shown
    public const bool DEBUG = true;

    public static void Main(string[] args) {
        while (true) {
            Console.Write("$ ");
            string input = Console.ReadLine();
            try {
                Tokenizer tokenizer = new Tokenizer(input);
                Parser parser = new Parser(tokenizer);
                AbstractSyntaxTreeNode tree = parser.Parse();

                SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
                analyzer.Analyze();
                if (DEBUG) {
                    Console.WriteLine("--== Semantic Analysis ==--");
                    Console.WriteLine(analyzer.Symbols);
                }

                Interpreter interpreter = new Interpreter(tree);
                interpreter.Interpret();
                if (DEBUG) {
                    Console.WriteLine("--== Global Scope (post-execution) ==--");
                    interpreter.PrintGlobalScope();
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                if (DEBUG) {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}