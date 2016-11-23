using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.ERP.Objects;

namespace Dot.ERP.Service
{
    public class MenuService
    {
        public List<Menu> GetMenuData()
        {
            var menus = new List<Menu>() 
            { 
                new Menu(){SysNo = "0.0", Name = "Dashboard", Url="/Home/Dashboard"},
                new Menu(){ SysNo = "1.0", Name = "系统管理", Url=""},
                new Menu(){ SysNo = "2.0", Name = "基础信息管理", Url=""},
                new Menu(){SysNo = "3.1", Name = "订单", Url="/Order/List"}
            };

            return menus;
        }
    }
}
