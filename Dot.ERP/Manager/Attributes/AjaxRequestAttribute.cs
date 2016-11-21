using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dot.ERP.Manager.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AjaxRequestAttribute : ActionMethodSelectorAttribute
    {
        public AjaxRequestAttribute()
            : this(true)
        {
        }

        public AjaxRequestAttribute(bool isAjaxRequest)
        {
            IsAjaxRequest = isAjaxRequest;
        }

        public bool IsAjaxRequest
        {
            get;
            private set;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            bool isAjaxRequest = controllerContext.HttpContext.Request.IsAjaxRequest();
            return IsAjaxRequest == isAjaxRequest;
        }
    }
}