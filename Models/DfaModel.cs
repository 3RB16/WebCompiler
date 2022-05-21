namespace WebCompiler.Models
{
    public class DfaModel
    {
        private readonly TrieNode dfa;
        public DfaModel() {
            dfa = new TrieNode();
        }
        public void addTransition(string word, string type) {
            var next = dfa;
            foreach (var c in word){
                if (next[c] == null){
                   next[c] = new TrieNode();
                }
                next = next[c];
            }
            next.TokenType = type;
        }
        private bool isConstant(string digits) {
            bool isDigit = true;
            foreach (var c in digits)
                isDigit &= Char.IsDigit(c);
            return isDigit;
        }
        private bool isError(string word) {
            if(char.IsDigit(word[0])) {
                return true;
            }
            bool foundError = false;
            foreach (var c in word)
            {
               foundError |= (char.IsLetterOrDigit(c) == false && c != '_');
            }
            return foundError;
        }
        public string Search(string word) {
            var next = dfa;
            bool isIdentifier = false;
            char currentChar = '0';
            foreach (var c in word)
            {
                if (next[c] == null)
                {
                    isIdentifier = true;
                    currentChar = c;
                    break;
                }
                next = next[c];
            }
            if (isIdentifier)
            {
                // handel edge cases like 10 or 0muhammed or hell#world
                // handel comments
                if (isConstant(word))
                    return "Constant";
                else if (isError(word) && currentChar != ';')
                    return "-1";
                return "identifier";
            }
            return next.TokenType;
        }

        class TrieNode
        {
            private TrieNode[] transactionTable = new TrieNode[130];    

            public string TokenType { get; set; }
            public TrieNode this[char c] {
                get { return transactionTable[c - '!']; }
                set { transactionTable[c - '!'] = value; }
            }
        }
    }
}
