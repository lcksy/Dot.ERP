using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.ERP.Objects
{
    public class UserRequest
    {
        public UserRequest() { Isdesc = false; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string User_Ename { get; set; }
        public string Division { get; set; }
        public bool IsValid { get; set; }
        public long User_ID { get; set; }
        public string OrderBy { get; set; }
        public bool Isdesc { get; set; }
    }
}
