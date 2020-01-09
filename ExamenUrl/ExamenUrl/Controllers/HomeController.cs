using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExamenUrl.Models;
using HtmlAgilityPack;

namespace ExamenUrl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static List<string> links = new List<string>();

        public IActionResult Save()
        {
            return View(new SaveModel { Text = links });
        }
        public  void SearchHtml(List<string> list, string html, int depth)
        {
            depth--;
            HtmlWeb web = new HtmlWeb();
            try
            {
                var htmlDoc = web.Load(html);

                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                foreach (var node in htmlNodes.Take(10))
                {
                    if (node.Attributes["href"] == null) continue;
                    var current = node.Attributes["href"].Value;
                    if (current[6] != '#' && current[0]!= '#')
                        list.Add(current);
                    if (depth >= 0) SearchHtml(list, "https:" + current, depth);
                }
            }
            catch
            {
                return;
            }
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(new IndexModel { Text = "aa" });
        }
        public IActionResult Search(string depth,string text2 )
        {
            var list = new List<string>();
            SearchHtml(list, text2, int.Parse(depth));
            links = list;
            return View(new SearchModel { Text = list});
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
