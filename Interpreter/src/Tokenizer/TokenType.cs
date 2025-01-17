namespace Rewired.Interpreter {

    /// <summary>
    /// The different types of tokens recognized by the `Tokenizer`.
    /// </summary>
    public enum TokenType {
        IntegerType,
        FloatType,
        BoolType,
        IntegerConst,
        FloatConst,
        BoolConst,
        Plus,
        Minus,
        Slash,
        Asterisk,
        ExclamationPoint,
        LeftParenthesis,
        RightParenthesis,
        LeftCurlyBracket,
        RightCurlyBracket,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        Equal,
        NotEqual,
        LogicalAnd,
        LogicalOr,
        Assign,
        SemiColon,
        Comma,
        Id,
        Func,
        Return,
        If,
        Else,
        Eof
    }
}