using WebCompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebCompiler.Models
{
    public class ScannerModel
    {
        static public List<string> run(List <string> data , bool type) {
            List<string> answer = new List<string>();
            int LineNumber = 1;
            BuildScannerModel scanner = new BuildScannerModel();
            List<string> parserList = new List<string>();
            int currentErrors = 0;
            bool isComment = false;
            foreach(string line in data)
            {
                scanner.TakeData(line, LineNumber , ref answer , ref isComment);
                // parser handelling
                if(currentErrors != scanner.numberOfErrors)
                {
                    parserList.Add("Line : " + LineNumber + " Not Matched\n");
                    currentErrors = scanner.numberOfErrors;
                } else
                {
                    parserList.Add("Line : " + LineNumber + " Matched" + "    " + "Rule used : " + scanner.getRole(line)  + "\n");
                }
                ++LineNumber;
            }
            answer.Add("Total NO of errors: " + scanner.numberOfErrors);
            parserList.Add("Total NO of errors: " + scanner.numberOfErrors);
            return type ? parserList : answer;
        }
    }
}
