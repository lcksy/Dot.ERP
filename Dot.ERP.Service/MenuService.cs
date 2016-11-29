using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dot.ERP.Objects;
using System.Data;
using System.Reflection;

namespace Dot.ERP.Service
{
    public class MenuService
    {
        public List<SysMenu> GetMenuData()
        {
            var sql = @"SELECT * FROM Sys_Menu WHERE Status = 1";

            using (SqlConnection connection = new SqlConnection("server=localhost;uid=sa;pwd=lcksy+110;database=Dot"))
            { 
                using(SqlCommand command = new SqlCommand(sql,connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        var menuTable = new DataTable("menuTable");
                        adapter.Fill(menuTable);
                        return MapMenu(menuTable);
                    }
                }
            }
        }

        private List<SysMenu> MapMenu(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
                return Enumerable.Empty<SysMenu>().ToList();

            var menus = new List<SysMenu>();
            var sysNos = source.AsEnumerable().OfType<DataRow>()
                               .Where(r => r.Field<int>("ParentSysNo") == 0).Select(s => s.Field<int>("SysNo"));
            foreach (int sysNo in sysNos)
            {
                var menu = this.MapMenuItem(source.AsEnumerable(), sysNo);
                menus.Add(menu);
            }

            return menus;
        }

        private SysMenu MapMenuItem(IEnumerable<DataRow> menuRows, int sysNo)
        {
            if (menuRows == null) throw new ArgumentException("source 为null");
            if (sysNo < 0) throw new ArgumentException("sysNo 不能小于0");

            var menu = menuRows.FirstOrDefault(m => m.Field<int>("SysNo") == sysNo).Map<SysMenu>();

            if (menu == null) return menu;

            var childRows = menuRows.Where(m => m.Field<int>("ParentSysNo") == sysNo);

            foreach (var childRow in childRows)
            {
                var childMenu = childRow.Map<SysMenu>();
                childMenu = MapMenuItem(menuRows, childRow.Field<int>("SysNo"));
                menu.ChildMenu.Add(childMenu);
            }

            return menu;
        }
    }

    public static class DataRowMapperExtension
    {
        private static BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        public static TInstance Map<TInstance>(this DataRow row)
        {
            if (row == null) return default(TInstance);

            var instance = Activator.CreateInstance<TInstance>();

            var properties = instance.GetType().GetProperties(flag);

            foreach (DataColumn column in row.Table.Columns)
            {
                var propertyInfo = properties.FirstOrDefault(p => p.Name == column.ColumnName);

                if (propertyInfo == null) continue;

                var methodInfo = typeof(DataRowExtensions).GetMethod("Field", new Type[] { typeof(DataRow), typeof(string) });

                var callMethodInfo = methodInfo.MakeGenericMethod(new Type[] { propertyInfo.PropertyType });

                var value = callMethodInfo.Invoke(row, new object[] { row, propertyInfo.Name });

                propertyInfo.SetValue(instance, value);
            }

            return instance;
        }
    }
}