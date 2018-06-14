using Huanent.Logging.File.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Huanent.Logging.File.UI.Areas.Logging.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Pwd { get; set; }

        public string Title { get; set; } = "ÈÕÖ¾²éÑ¯µÇÂ¼";

        public string Btn { get; set; } = "µÇÂ¼";

        public string Tip { get; set; } = string.Empty;


        public void OnGet()
        {
            if (ConfigHelper.Get() == null)
            {
                Title = "ÇëÊäÈë³õÊ¼ÃÜÂë";
                Btn = "±£´æ";
            }
        }
        public IActionResult OnPost()
        {
            var config = ConfigHelper.Get();
            if (config != null)
            {
                if (config.CanLoginDate > DateTime.Now)
                {
                    Tip = "ÃÜÂë´íÎó³¬¹ý3´Î£¬Ëø¶¨5·ÖÖÓ";
                    return Page();
                }

                if (config.Pwd.Trim() != Pwd)
                {
                    Tip = "ÃÜÂë´íÎó";
                    config.PwdErrorCount += 1;
                    if (config.PwdErrorCount > 3)
                    {
                        config.PwdErrorCount = 0;
                        config.CanLoginDate = DateTime.Now.AddMinutes(5);
                        Tip = "ÃÜÂë´íÎó³¬¹ý3´Î£¬Ëø¶¨5·ÖÖÓ";
                    }
                    ConfigHelper.Save(config);
                    return Page();
                }
            }
            else
            {
                var rijndaelManaged = Rijndael.Create();
                rijndaelManaged.Mode = CipherMode.ECB;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.GenerateKey();
                config = new ConfigModel
                {
                    Pwd = Pwd,
                    AESKey = rijndaelManaged.Key
                };
                ConfigHelper.Save(config);
            }
            string token = AES256Helper.Encrypt(DateTime.Now.AddMinutes(1).ToString(), config.AESKey);
            Response.Cookies.Append(Constants.CookieName, token);
            return RedirectToPage("Index");
        }
    }
}