using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File.UI.Models
{
    public class ConfigModel
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        public byte[] AESKey { get; set; }

        /// <summary>
        /// 被锁定时需要超过此时间才能再次登录
        /// </summary>
        public DateTime CanLoginDate { get; set; }

        public string Pwd { get; set; }

        /// <summary>
        /// 密码输入错误次数
        /// </summary>
        public int PwdErrorCount { get; set; }
    }
}