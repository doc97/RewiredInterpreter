# Rewired Interpreter

This is the interpreter I am writing to use in a game I am planning on 
developing called Rewired. I am reading the awesome interpreter series: 
https://ruslanspivak.com/ to learn about interpreters and compilers.

Once I have studied enough I will design my own small language and 
implement the interpreter for it.

## Development Setup

**Step 1**: Install .NET SDK 3.0 or higher [[link][1]]  
**Step 2**: Clone the repository and open in VS Code
```
$ git clone git@github.com:doc97/RewiredInterpreter.git
$ cd RewiredInterpreter
$ code .
```
**Step 3**: Install the VSCode extensions
- C# by Microsoft [[link][2]]
- .NET Core Test Explorer by Jun Han [[link][3]]

## Testing and Running

You can run the unit tests from the terminal:
```
$ dotnet test
```
Or from the extension:  
![Unit test gif][gif]

To run the program:
```
$ dotnet run --project Interpreter
$ dotnet run                        # you can omit the --project flag, if 
                                    # you're in the Interpreter directory
```

## How it works

The project consists of 4 separate components; the tokenizer, the parser, 
the semantic analyzer and the interpreter.

`Tokenizer -> Parser -> Semantic Analyzer -> Interpreter`

The source code is given to the `Tokenizer` that converts the text into 
a stream of so called tokens. The `Parser` then takes these tokens and
constructs an abstract syntax tree, which is an "intermediate form".

Instead of combining the parser and the interpreter, they are separated 
for a couple of reasons. The first being, that it nicely separates the 
responsibilities of each component. Secondly and more importantly, the 
intermediate form, can be used by both the `Semantic Analyzer` and the 
`Interpreter`. The `Semantic Analyzer` checks for static semantic errors 
before the code is even interpreted. The dynamic semantics must be 
checked during runtime.

## Grammar

Here is the grammar that the interpreter currently supports:

```
PROGRAM -> (DECLARATION | STATEMENT_LIST)*
DECLARATION -> "func" ID "(" ")" BLOCK
BLOCK -> "{" STATEMENT_LIST "}"
STATEMENT_LIST -> STATEMENT+
STATEMENT -> ASSIGNMENT ";" | EMPTY
ASSIGNMENT -> VAR ":=" EXPR
VAR -> ID
EMPTY -> ""
EXPR -> TERM (("+" | "-") TERM)*
TERM -> FACTOR (("*" | "/") FACTOR)*
FACTOR -> "+" FACTOR
        | "-" FACTOR
        | "(" EXPR ")"
        | INTEGER
        | VAR
```

`ID`: An identifier (name) of a variable

[1]: https://dotnet.microsoft.com/download
[2]: https://github.com/OmniSharp/omnisharp-vscode
[3]: https://github.com/formulahendry/vscode-dotnet-test-explorer
[gif]: Docs/rewired_unittest.gif "Unit test demo"