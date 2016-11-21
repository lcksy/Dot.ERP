﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Dot.ERP.Core.Ajax
{
    /// <summary>
    /// 这个类是 ajax请求的公共类
    /// 在这个程序中，ajax请求会通过反射的方式动态加载 类.方法成员
    /// 这个类在初始化的时候会遍历全部程序集,并把符合条件的类保存起来
    /// </summary>
    public class AjaxRequestHelper
    {
        #region 初始化加载
        private static List<AjaxTypeContainer> ajaxTypeContainerList = new List<AjaxTypeContainer>();

        /// <summary>
        /// 获取 类.方法名 列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetActionNameList()
        {
            var list = new List<string>();
            foreach (var container in ajaxTypeContainerList)
            {
                foreach (var action in container.Actions)
                {
                    if (new string[] { "ToString", "GetHashCode", "GetType", "Equals" }.Contains(action.Method.Name)) continue;

                    list.Add(container.AjaxClass.Name + "." + action.Method.Name);
                }
            }
            return list.Where(c => !c.StartsWith("AjaxResult") && !c.StartsWith("AjaxRequestHelper")).ToArray();
        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        static AjaxRequestHelper()
        {
            DirectoryInfo directInfo = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
            FileInfo[] files = directInfo.GetFiles("*Service.dll");
            foreach (var file in files)
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                try
                {
                    foreach (Type t in assembly.GetExportedTypes())
                    {
                        ajaxTypeContainerList.Add(new AjaxTypeContainer(t));
                    }
                }
                catch { }
            }
        }
        #endregion

        #region 获取方法的参数值
        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static object[] GetMethodParms(AjaxAction action, HttpContextBase context)
        {
            return GetMethodParms(action.Parameters, context);
        }


        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetMethodParms(string typeName, string methodName, HttpContextBase context)
        {
            var method = GetMethod(typeName, methodName);
            if (method == null) return null;
            return GetMethodParms(method, context);
        }


        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="method"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetMethodParms(MethodInfo method, HttpContextBase context)
        {
            var parms = method.GetParameters();
            return GetMethodParms(parms, context);
        }
        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetMethodParms(ParameterInfo[] parms, HttpContextBase context)
        {
            var objs = new object[parms.Length];
            for (var i = 0; i < parms.Length; i++)
            {
                var parm = parms[i];

                if (parm.ParameterType == typeof(NameValueCollection))
                {
                    //if (parm.Name.EqualsTo("Form", true))

                    NameValueCollection values = new NameValueCollection();
                    foreach (var key in context.Request.Form.AllKeys)
                    {
                        values.Add(key, context.Server.UrlDecode(context.Request[key]));
                    }
                    objs[i] = values;
                    //else if (parm.Name.EqualsTo("QueryString", true))
                    //    objs[i] = context.Request.QueryString;
                }
                else
                {
                    Type paramterType = parm.ParameterType;
                    if (parm.ParameterType.IsGenericType)
                        paramterType = Nullable.GetUnderlyingType(parm.ParameterType) ?? parm.ParameterType;
                    if (TypeHelper.IsSimpleType(paramterType))
                    {
                        objs[i] = GetMethodParmValue(context.Request, parm);
                    }
                    else if (paramterType == typeof(DataTable))
                    {
                        NameValueCollection values = null;
                        values = context.Request.Form;
                        objs[i] = JsonHelper.ToDataTable(values);

                    }
                    else if (paramterType == typeof(HttpContext))
                    {
                        objs[0] = context;
                    }
                    else
                    {
                        //如果这个参数 是自定义的类，那么实例化一个对象，然后使用 Request键值对 赋值
                        var obj = Activator.CreateInstance(paramterType);

                        var members = parm.ParameterType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var mi in members)
                        {
                            if (!TypeHelper.IsFieldOrProperty(mi) || TypeHelper.IsReadOnly(mi))
                                continue;
                            var val = GetMethodParmValue(context.Request, mi.Name, TypeHelper.GetMemberType(mi));
                            if (val != null)
                            {
                                TypeHelper.SetValue(mi, obj, val);
                            }
                        }
                        objs[i] = obj;
                    }
                }
            }
            return objs;
        }

        /// <summary>
        /// 获取方法单个参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parmName"></param>
        /// <param name="parmType"></param>
        /// <returns></returns>
        public static object GetMethodParmValue(HttpRequestBase request, string parmName, Type parmType)
        {
            if (string.IsNullOrEmpty(request[parmName])) return null;
            if (!TypeHelper.IsSimpleType(parmType)) return null;
            return request[parmName];
            //return null;
        }

        /// <summary>
        /// 获取方法单个参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static object GetMethodParmValue(HttpRequestBase request, ParameterInfo parm)
        {
            return GetMethodParmValue(request, parm.Name, parm.ParameterType);
        }

        #endregion

        #region 获取类、方法定义
        public static Type GetType(string typeName)
        {
            foreach (var c in ajaxTypeContainerList)
            {
                var type = c.AjaxClass;
                if (type == null || type.Name != typeName) continue;
                return type;
            }
            return null;
        }


        public static MethodInfo GetMethod(string typeName, string methodName)
        {
            foreach (var c in ajaxTypeContainerList)
            {
                var type = c.AjaxClass;
                if (type == null || type.Name != typeName) continue;
                return c.GetMethod(methodName);
            }
            return null;
        }
        #endregion

    }
}