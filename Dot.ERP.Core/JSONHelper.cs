using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;

namespace Dot.ERP.Core
{
    public class JsonHelper
    {
        private static JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();
        /// <summary>
        /// 类对像转换成json格式
        /// </summary> 
        /// <returns></returns>
        public static string ToJson(object t)
        {
            return _JavaScriptSerializer.Serialize(t);
        }

        public static Dictionary<string, object> ToDictionary(DataSet data)
        {
            Dictionary<string, object> dics = new Dictionary<string, object>();
            if (data != null
                && data.Tables.Count > 0)
            {
                foreach (DataTable dt in data.Tables)
                {
                    var listData = JsonHelper.ToList(dt);
                    if (dics.ContainsKey(dt.TableName))
                    {
                        dics[dt.TableName] = listData;
                    }
                    else
                    {
                        dics.Add(dt.TableName, listData);
                    }
                }
            }

            return dics;
        }

        public static DataTable ToDataTable(string key, string ids)
        {
            DataTable result = new DataTable();
            result.Columns.Add(key);
            string[] arrID = ids.Split(',');
            DataRow dr = null;
            foreach (var id in arrID)
            {
                dr = result.NewRow();
                dr[key] = id.Trim();
                result.Rows.Add(dr);
            }

            return result;
        }

        public static DataTable ToDataTable(NameValueCollection form)
        {
            DataTable result = new DataTable();
            foreach (var key in form.AllKeys)
            {
                result.Columns.Add(key);
            }
            DataRow dr = result.NewRow();
            foreach (var key in form.AllKeys)
            {
                dr[key] = (string.IsNullOrWhiteSpace(form[key]) == true)
                            ? string.Empty
                            : form[key].Trim();
            }
            result.Rows.Add(dr);
            return result;
        }

        /// <summary>
        /// json格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            return _JavaScriptSerializer.Deserialize<T>(strJson);
        }

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="commandText">commandText</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        //public static string GetArrayJSON(string commandText, string id, string pid)
        //{
        //    var o = ArrayToTreeData(commandText, id, pid);
        //    return ToJson(o);
        //}
        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string GetArrayJSON(DataTable data, string id, string pid)
        {
            var o = ArrayToTreeData(data, id, pid);
            return ToJson(o);
        }

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string GetArrayJSON(IList<Hashtable> list, string id, string pid)
        {
            var o = ArrayToTreeData(list, id, pid);
            return ToJson(o);
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="id">id的字段名</param>
        /// <param name="pid">pid的字段名</param>
        /// <returns></returns>
        public static object ArrayToTreeData(DataRow[] rows, string id, string pid)
        {
            var list = DbReaderToHash(rows);
            return ArrayToTreeData(list, id, pid);

        }

        public static object ArrayToTreeData(DataTable data, string id, string pid)
        {
            return (data == null)
                    ? null
                    : ArrayToTreeData(data.Rows.Cast<DataRow>().ToArray(), id, pid);
        }

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="commandText">sql</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param> 
        /// <returns></returns>

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static object ArrayToTreeData(IList<Hashtable> list, string id, string pid)
        {
            var h = new Hashtable(); //数据索引 
            var r = new List<Hashtable>(); //数据池,要返回的 

            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                h[item[id].ToString()] = item;
            }
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !h.ContainsKey(item[pid].ToString()))
                {
                    r.Add(item);
                }
                else
                {
                    var pitem = h[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);

                }
            }
            return r;
        }

        public static List<Hashtable> ToList(DataTable data)
        {
            return DbReaderToHash(data);
        }

        private static List<Hashtable> DbReaderToHash(DataTable data)
        {
            return (data == null)
                    ? null
                    : DbReaderToHash(data.Rows.Cast<DataRow>().ToArray());
        }

        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<Hashtable> DbReaderToHash(DataRow[] rows)
        {
            var list = new List<Hashtable>();
            if (rows != null)
            {
                foreach (var row in rows.Cast<DataRow>())
                {
                    var item = new Hashtable();

                    foreach (DataColumn dc in row.Table.Columns)
                    {
                        var name = dc.ColumnName;

                        item[name] = row[dc];
                    }
                    list.Add(item);

                }
            }
            return list;
        }

        public static string ToJson(DataTable dt)
        {
            return (dt == null)
                    ? "[]"
                    : ToJson(dt.Rows.Cast<DataRow>().ToArray());
        }
    }
}