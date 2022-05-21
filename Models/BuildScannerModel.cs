using WebCompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCompiler.Models
{
    public class BuildScannerModel
    {
        private const int MAX = 55;
        private string[] KeyWords = new string[MAX];
        private string[] ReturnToken = new string[MAX];
        public int numberOfErrors = 0;
        public DfaModel dfa = new DfaModel();
        private void buildTokens() {
            KeyWords[0] = "Category"; ReturnToken[0] = "Class";
            KeyWords[1] = "Derive"; ReturnToken[1] = "Inheritance";
            KeyWords[2] = "If"; ReturnToken[2] = "Condition";
            KeyWords[3] = "Else"; ReturnToken[3] = "Condition";
            KeyWords[4] = "Ilap"; ReturnToken[4] = "Integer";
            KeyWords[5] = "Silap"; ReturnToken[5] = "SInteger";
            KeyWords[6] = "Clop"; ReturnToken[6] = "Character";
            KeyWords[7] = "Series"; ReturnToken[7] = "String";
            KeyWords[8] = "Ilapf"; ReturnToken[8] = "Float";
            KeyWords[9] = "Silapf"; ReturnToken[9] = "SFloat";
            KeyWords[10] = "None"; ReturnToken[10] = "Void";
            KeyWords[11] = "Rotatewhen"; ReturnToken[11] = "Loop";
            KeyWords[12] = "Continuewhen"; ReturnToken[12] = "Loop";
            KeyWords[13] = "Replywith"; ReturnToken[13] = "Return";
            KeyWords[14] = "Seop"; ReturnToken[14] = "Struct";
            KeyWords[15] = "Check"; ReturnToken[15] = "Switch";
            KeyWords[16] = "situationof"; ReturnToken[16] = "Switch";
            KeyWords[17] = "Program"; ReturnToken[17] = "Start Statement";
            KeyWords[18] = "End"; ReturnToken[18] = "End Statement";
            KeyWords[19] = "+"; ReturnToken[19] = "Arithmetic Operation";
            KeyWords[20] = "-"; ReturnToken[20] = "Arithmetic Operation";
            KeyWords[21] = "*"; ReturnToken[21] = "Arithmetic Operation";
            KeyWords[22] = "/"; ReturnToken[22] = "Arithmetic Operation";
            KeyWords[23] = "&&"; ReturnToken[23] = "Logic operator";
            KeyWords[24] = "||"; ReturnToken[24] = "Logic operator";
            KeyWords[25] = "~"; ReturnToken[25] = "Logic operator";
            KeyWords[26] = "=="; ReturnToken[26] = "relational operator";
            KeyWords[27] = "<"; ReturnToken[27] = "relational operator";
            KeyWords[28] = ">"; ReturnToken[28] = "relational operator";
            KeyWords[29] = "!="; ReturnToken[29] = "relational operator";
            KeyWords[30] = "<="; ReturnToken[30] = "relational operator";
            KeyWords[31] = ">="; ReturnToken[31] = "relational operator";
            KeyWords[32] = "="; ReturnToken[32] = "Assignment operator";
            KeyWords[33] = "."; ReturnToken[33] = "Access operator";
            KeyWords[34] = "{"; ReturnToken[34] = "Braces";
            KeyWords[35] = "}"; ReturnToken[35] = "Braces";
            KeyWords[36] = "["; ReturnToken[36] = "Braces";
            KeyWords[37] = "]"; ReturnToken[37] = "Braces";
            KeyWords[38] = "0"; ReturnToken[38] = "Constant";
            KeyWords[39] = "1"; ReturnToken[39] = "Constant";
            KeyWords[40] = "2"; ReturnToken[40] = "Constant";
            KeyWords[41] = "3"; ReturnToken[41] = "Constant";
            KeyWords[42] = "4"; ReturnToken[42] = "Constant";
            KeyWords[43] = "5"; ReturnToken[43] = "Constant";
            KeyWords[44] = "6"; ReturnToken[44] = "Constant";
            KeyWords[45] = "7"; ReturnToken[45] = "Constant";
            KeyWords[46] = "8"; ReturnToken[46] = "Constant";
            KeyWords[47] = "9"; ReturnToken[47] = "Constant";
            KeyWords[48] = "\""; ReturnToken[48] = "Quotation Mark";
            KeyWords[49] = "\'"; ReturnToken[49] = "Quotation Mark";
            KeyWords[50] = "Using"; ReturnToken[50] = "Inclusion";
            KeyWords[51] = "("; ReturnToken[51] = "Braces";
            KeyWords[52] = ")"; ReturnToken[52] = "Braces";
        }
        private void buildTree() {
            for (int i = 0; i <= 52; i++)
            {
                dfa.addTransition(KeyWords[i], ReturnToken[i]);
            }
        }
        private static List<string> filterData(string data) {
            List<string> result = new List<string>();
            string currentData = "";
            foreach (var c in data)
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (String.IsNullOrEmpty(currentData)) continue;
                    result.Add(currentData);
                    currentData = "";
                }
                else
                {
                    if (c == ';') continue;
                    currentData += c;
                }
            }
            if (!string.IsNullOrEmpty(currentData))
            {
                result.Add(currentData);
            }
            return result;
        }
        public void TakeData(string data, int numberOfLine , ref List<string> answer , ref bool isComment) {
            var dataAfterFilter = filterData(data);
            findToken(dataAfterFilter, numberOfLine , ref answer , ref isComment);
        }
        private bool isSingleComment(string line) {
            if (line.Length < 2) return false;
            return line[0] == '-' && line[1] == '-';
        }
        private bool isMultipleComment(string line) {
            if (line.Length < 2) return false;
            return line[0] == '<' && line[1] == '*';
        }
        private bool canCloseComment(string line) {
            if (line.Length < 2) return false;
            for(int i = 1;i < line.Length;i++)
            {
                if (line[i] == '>' && line[i - 1] == '*') return true;
            }
            return false;
        }
        private void findToken(List<string> data, int numberOfLine , ref List<string> answer , ref bool isComment) {
            // handel multible comments
            for (int index = 0; index < data.Count; index++)
            {
                if (canCloseComment(data[index]))
                {
                    isComment = false;
                    return;
                }
            }
            for (int index = 0; index < data.Count; index++)
            {
                // handel single comments
                if (isSingleComment(data[index])) break;
                if(isMultipleComment(data[index])){
                    isComment = true;
                    break;
                }
                // now we in multiple comment mode 
                if (isComment) break;
                string Token = dfa.Search(data[index]);
                if (Token == "-1")
                {
                    ++numberOfErrors;
                    AddErrorToken(numberOfLine, data[index] , ref answer);
                }
                else
                {
                    AddCorrectToken(numberOfLine, data[index], Token , ref answer);
                }
            }
        }
     

        private void AddCorrectToken(int numberOfLine, string TokenText, string TokenType , ref List <string> answer) {
            answer.Add("Line : " + numberOfLine + " Token Text:" + TokenText
             + "    " + "Token Type:" + TokenType + "\n");
        }
        private void AddErrorToken(int numberOfLine, string TokenText , ref List<string> answer) {
            answer.Add("Line : " + numberOfLine + " Error in Token Text:" + TokenText + "\n");
        }
         
        public BuildScannerModel() {
            buildTokens();
            buildTree();
        }
        public string getRole(string line) { // parser handelling
            var filterdLine = filterData(line);
            foreach (var current in filterdLine)
            {
                if (current == "Program") return "Program";
                else if (current == "Category") return " ClassDeclaration";
                else if (current == "W") return "MethodDeclaration";
                else if (current == "Ilap" || current == "Silap" || current == "Clop" || current == "Series" ||
                 current == "Ilapf" || current == "Silapf" || current == "Logical")
                    return "Type";
                else if (current == "If" || current == "Else") return "If _Statement";
                else if (current == "Continuewhen") return "Continuewhen";
                else if (current == "{" || current == "}") return "Block Statements";
            }
            return "Statement";
        }
    }
}
