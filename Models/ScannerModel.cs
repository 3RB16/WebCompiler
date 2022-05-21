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
        static public List<string> run(List <string> data) {
            List<string> answer = new List<string>();
            int LineNumber = 1;
            BuildScannerModel scanner = new BuildScannerModel();
            bool isComment = false;
            foreach(string line in data)
            {
                scanner.TakeData(line, LineNumber , ref answer , ref isComment);
                ++LineNumber;
            }
            answer.Add("Total NO of errors: " + scanner.numberOfErrors);
            return answer;
        }
    }
}
