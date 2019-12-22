namespace Rewired.Interpreter {

    /// <summary>
    /// The different types of tokens recognized by the `Tokenizer`.
    /// </summary>
    public enum TokenType {
        IntegerType,
        FloatType,
        IntegerConst,
        FloatConst,
        Plus,
        Minus,
        Slash,
        Asterisk,
        LeftParenthesis,
        RightParenthesis,
        LeftCurlyBracket,
        RightCurlyBracket,
        Assign,
        SemiColon,
        Comma,
        Id,
        Func,
        Eof
    }
}