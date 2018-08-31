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
        public IEnumerable<LogModel> Logs { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public IActionResult OnGet([FromServices] AuthService authService)
        {
            bool login = authService.Check(HttpContext);
            if (!login) return RedirectToPage("Login");

            string filePath = Path.Combine(AppContext.BaseDirectory, "logs", $"{Name}.txt");
            string text;
            using (var logFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                var bytes = new List<byte>();
                byte[] buffer = new byte[1024 * 1024 * 3];
                while (true)
                {
                    int length = logFile.Read(buffer, 0, buffer.Length);
                    if (length == 0) break;
                    bytes.AddRange(buffer.Take(length));
                }
                text = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
            }

            //string text = System.IO.File.ReadAllText(filePath);
            string[] list = text.Trim().Split(new[] { "-----End-----" }, StringSplitOptions.RemoveEmptyEntries);
            var logs = new ConcurrentBag<LogModel>();
            Parallel.ForEach(list, item =>
            {
                var log = new LogModel();
                string[] lines = item.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 0) log.Time = Convert.ToDateTime(lines[0].Replace("-", ""));
                if (lines.Length > 1)
                {
                    string[] categoryAndLogLevel = lines[1].Split(':');
                    if (categoryAndLogLevel.Length > 0) log.Loglevel = categoryAndLogLevel[0].Trim();
                    if (categoryAndLogLevel.Length > 1)
                    {
                        log.Category = string.Join(string.Empty, categoryAndLogLevel.Skip(1));
                    }
                }
                if (lines.Length > 2) log.Content = string.Join(Environment.NewLine, lines.Skip(2));
                logs.Add(log);
            });
            Logs = logs.OrderByDescending(o => o.Time).ToList();
            return Page();
        }
    }
}