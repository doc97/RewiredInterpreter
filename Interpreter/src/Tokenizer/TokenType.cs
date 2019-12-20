namespace Rewired.Interpreter {

    /// <summary>
    /// The different types of tokens recognized by the `Tokenizer`.
    /// </summary>
    public enum TokenType {
        Integer,
        Plus,
        Minus,
        Slash,
        Asterisk,
        LeftParenthesis,
        RightParenthesis,
        Assign,
        SemiColon,
        Id,
        Eof
    }
}