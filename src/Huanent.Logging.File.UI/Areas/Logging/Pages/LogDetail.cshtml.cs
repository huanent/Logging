using Huanent.Logging.File.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Huanent.Logging.File.UI.Areas.Logging.Pages
{
    public class LogDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public IEnumerable<LogModel> Logs { get; set; }

        public IActionResult OnGet()
        {
            bool login = AuthHelper.Check(HttpContext);
            if (!login) return RedirectToPage("Login");

            string filePath = Path.Combine(AppContext.BaseDirectory, "logs", $"{Name}.txt");
            string text = System.IO.File.ReadAllText(filePath);
            string[] list = text.Trim().Split(new[] { "-----End-----" }, StringSplitOptions.RemoveEmptyEntries);
            var logs = new ConcurrentBag<LogModel>();
            Parallel.ForEach(list, item =>
            {
                var log = new LogModel();
                string[] lines = item.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                log.Time = Convert.ToDateTime(lines[0].Replace("-", ""));
                string[] categoryAndLogLevel = lines[1].Split(':');
                log.Loglevel = categoryAndLogLevel[0].Trim();
                log.Category = string.Join(string.Empty, categoryAndLogLevel.Skip(1));
                log.Content = string.Join(Environment.NewLine, lines.Skip(2));
                logs.Add(log);
            });
            Logs = logs.OrderByDescending(o => o.Time).ToList();
            return Page();
        }
    }
}