using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class RecordInfo
    {
        public long UserID { get; set; }

        public string BillType { get; set; }

        public string InOrOut { get; set; }

        public float Money { get; set; }

        public string Remark { get; set; }

        public DateTime RecordTime { get; set; }
    }
}
