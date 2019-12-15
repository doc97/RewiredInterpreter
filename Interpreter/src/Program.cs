using System;
using InterpreterPractice;

public class Program {

    public static void Main(string[] args) {
        while (true) {
            Console.Write("$ ");
            string input = Console.ReadLine();
            try {
                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.Next());
                Interpreter interpreter = new Interpreter(parser);
                object result = interpreter.Interpret();
                if (result is int) {
                    Console.WriteLine(result);
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}