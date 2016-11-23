using Dot.ERP.Core.Ajax;
using Dot.ERP.Manager.Attributes;
using Dot.ERP.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dot.ERP.Controllers
{
    public class AjaxController : Controller
    {
        private HttpContextBase context;

        //类名
        protected string TypeName
        {
            get { return this.context.Request["type"]; }
        }

        // 方法名
        protected string MethodName
        {
            get { return this.context.Request["method"]; }
        }

        [AjaxRequest]
        public ActionResult ProcessRequest()
        {
            this.context = this.HttpContext;

            var data = Run(context);

            var result = new DataResult();
            result.recordsTotal = 25;
            result.recordsFiltered = 25;
            result.iTotalDisplayRecords = 25;
            result.iTotalRecords = 25;
            result.aaData = data;
            result.draw = 10;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return Content(json);
        }

        private object Run(HttpContextBase context)
        {
            var method = AjaxRequestHelper.GetMethod(TypeName, MethodName);
            if (method == null)
            {
                throw new Exception(string.Format("找不到类{0} 方法{1}", TypeName, MethodName));
            }

            var objtype = AjaxRequestHelper.GetType(TypeName);
            object obj = Activator.CreateInstance(objtype);

            var result = method.Invoke(obj, AjaxRequestHelper.GetMethodParms(method, context));

            return result;
        }
    }
}