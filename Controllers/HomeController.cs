using WebCompiler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.FormCollection;

namespace WebCompiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string text_content = "";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string text, string loadedfile, string state)
        {
            text_content = text;
            ViewBag.input = text;
            ViewBag.output = "";
            if (state == "parse")
            {
                //parse code.
                List<string> input = new List<string>();
                string inputText = "";
                text_content += "\n";
                for (int i = 0; i < text_content.Length; i++)
                {
                    if (text_content[i] != '\n')
                    {
                        inputText += text_content[i];
                    }
                    else
                    {
                        input.Add(inputText);
                        inputText = "";
                    }
                }

                List<string> ret = ScannerModel.run(input, true);
                string finalOutput = "";
                for (int i = 0; i < ret.Count; i++)
                {
                    finalOutput += ret[i];
                }
                ViewBag.output = finalOutput;
                ViewBag.input = "";
                return View("Index");
            }
            else if (state == "scan")
            {
                //scan code.
                List<string> input = new List<string>();
                string inputText = "";
                text_content += "\n";
                for (int i = 0; i < text_content.Length; i++)
                {
                    if (text_content[i] != '\n')
                    {
                        inputText += text_content[i];
                    }
                    else
                    {
                        input.Add(inputText);
                        inputText = "";
                    }
                }

                List<string> ret = ScannerModel.run(input , false);
                string finalOutput = "";
                for (int i = 0; i < ret.Count; i++){
                    finalOutput += ret[i];
                }
                ViewBag.output = finalOutput;
                ViewBag.input = "";
                return View("Index");
            }
            else
            {
                
                ViewBag.output = loadedfile;
                return View("Index");
            }
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
