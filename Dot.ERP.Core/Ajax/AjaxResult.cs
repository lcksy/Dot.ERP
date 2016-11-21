using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Dot.ERP.Core.Ajax
{
    public class AjaxResult
    {
        private AjaxResult()
        {
            this.IsError = false;
            this.IsSessionTimeOut = false;
        }

        public bool IsError { get; set; }

        public bool IsSessionTimeOut { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        #region Error
        public static AjaxResult Error()
        {
            return new AjaxResult()
            {
                IsError = true
            };
        }
        public static AjaxResult Error(string message)
        {
            return new AjaxResult()
            {
                IsError = true,
                Message = message.Replace("\r\n", "<br/>")
            };
        }
        #endregion

        public static AjaxResult SessionTimeout(string message)
        {
            return new AjaxResult()
            {
                IsSessionTimeOut = true,
                Message = message.Replace("\r\n", "<br/>")
            };
        }

        #region Success
        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
            };
        }
        public static AjaxResult Success(string message)
        {
            return new AjaxResult()
            {
                Message = message
            };
        }
        public static AjaxResult Success(object data)
        {
            return new AjaxResult()
            {
                Data = data
            };
        }
        public static AjaxResult Success(object data, string message)
        {
            return new AjaxResult()
            {
                Data = data,
                Message = message
            };
        }
        #endregion
        
        public override string ToString()
        {
            var output = new JavaScriptSerializer().Serialize(this);
            return output;
        }
    }
}