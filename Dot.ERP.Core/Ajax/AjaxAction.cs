using System.Reflection;

namespace Dot.ERP.Core.Ajax
{
    /// <summary>
    /// 内部 Ajax 方法 容器
    /// </summary>
    internal class AjaxAction
    {
        public AjaxAction(MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters();
        }

        public MethodInfo Method { get; private set; }

        public ParameterInfo[] Parameters { get; private set; }
    }
}