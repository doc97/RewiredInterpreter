namespace Rewired.Interpreter {

    public struct ReservedKeyword {
        public TokenType type;
        public string name;

        public ReservedKeyword(TokenType type, string name) {
            this.type = type;
            this.name = name;
        }
    }

}