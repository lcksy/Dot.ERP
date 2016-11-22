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
                new Menu(){ SysNo = "1.0", Name = "系统管理", Url="", Menus = new List<Menu>(){
                    new Menu(){SysNo = "1.1", Name = "菜单管理", Url="/SystemMenu/MenuPage"},
                    new Menu(){SysNo = "1.2", Name = "权限中心", Url="/SysMenu/Right"},
                    new Menu(){SysNo = "1.3", Name = "数据权限", Url="/SysMenu/DataPrivilege"},
                    new Menu(){SysNo = "1.4", Name = "底层权限", Url="/SysMenu/AdminMethod"},
                    new Menu(){SysNo = "1.5", Name = "操作权限", Url="/SysMenu/Configuration"}
                }},
                new Menu(){ SysNo = "2.0", Name = "基础信息管理", Url="", Menus = new List<Menu>(){
                    new Menu(){SysNo = "2.1", Name = "类别", Url="/Menu/Index"},
                    new Menu(){SysNo = "2.2", Name = "产品", Url="/Menu/Right"},
                    new Menu(){SysNo = "2.3", Name = "供应商", Url="/Menu/DataPrivilege", Menus = new List<Menu>(){
                        new Menu(){SysNo = "2.3.1", Name = "供应商001", Url="/Menu/Right"},
                        new Menu(){SysNo = "2.3.2", Name = "供应商002", Url="/Menu/Right"},
                        new Menu(){SysNo = "2.3.3", Name = "供应商003", Url="/Menu/Right"},
                    }},
                    new Menu(){SysNo = "2.4", Name = "员工", Url="/Menu/AdminMethod"}
                }},
                new Menu(){SysNo = "3.1", Name = "订单", Url="/Order/List"}
            };

            return menus;
        }
    }
}
