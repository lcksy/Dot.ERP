using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.ERP.Objects
{
    public class Menu
    {
        public string SysNo { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
