# Rewired Interpreter

This is the interpreter I am writing to use in a game I am planning on developing called Rewired. I am reading the awesome interpreter series: https://ruslanspivak.com/ to learn about interpreters and compilers.

Once I have learned enough I will design my own small language and implement the interpreter for it.

## Grammar

Here is the grammar that the interpreter currently supports:

```
COMPOUND -> STATEMENT+
STATEMENT -> (ASSIGNMENT | EMPTY) ";"
ASSIGNMENT -> VAR "=" EXPR
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