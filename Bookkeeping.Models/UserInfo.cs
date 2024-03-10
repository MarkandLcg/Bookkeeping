using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class UserInfo
    {
        public long UserID { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserMail { get; set; }

        public byte[] UserPhoto { get; set; }
    }
}
