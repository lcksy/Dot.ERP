using Dot.ERP.Objects;
using Dot.ERP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Dot.ERP.Controllers
{
    public class SystemMenuController : Controller
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menus = new MenuService().GetMenuData();

            return PartialView(menus);
        }

        public ActionResult MenuManager()
        {
            var menus = new MenuService().GetMenuData();

            return PartialView(menus);
        }

        /*
        public JsonResult MenuPage()
        {
            var result = this.CreateMenuList(this.HttpContext);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private DataResult CreateMenuList(HttpContextBase context)
        {
            var request = context.Request;

            var re = new UserRequest();
            //获取页面大小
            if (string.IsNullOrWhiteSpace(request["iDisplayLength"]))
                re.PageSize = 10;
            else
                re.PageSize = int.Parse(request["iDisplayLength"]);
            //获取开始数 算出当前页数
            if (string.IsNullOrWhiteSpace(request["iDisplayStart"]))
                re.PageIndex = 0;
            else
                re.PageIndex = (int.Parse(request["iDisplayStart"]) / re.PageSize) + 1;
            //自定义的两个参数 Division和UserName
            if (!string.IsNullOrWhiteSpace(request["Division"]))
                re.Division = request["Division"];
            if (!string.IsNullOrWhiteSpace(request["UserName"]))
                re.User_Ename = request["UserName"];
            //排序
            if (!string.IsNullOrWhiteSpace(request["iSortCol_0"]) && !string.IsNullOrWhiteSpace(request["sSortDir_0"]))
                re.OrderBy = request[("mDataProp_" + request["iSortCol_0"])];// +" " + Request["sSortDir_0"];
            else
                re.OrderBy = request[("mDataProp_0")];
            //正序还是倒序
            if (!string.IsNullOrWhiteSpace(request["sSortDir_0"]))
                re.Isdesc = (request["sSortDir_0"] == "descdesc" ? true : false);
            var result = new DataResult();
            var data = this.GetMenuData().Skip((re.PageIndex - 1) * re.PageSize).Take(re.PageSize);//获取数据的方法
            result.recordsTotal = 25;
            result.recordsFiltered = 25;
            result.iTotalDisplayRecords = 25;
            result.iTotalRecords = 25;
            result.aaData = data;
            result.draw = 10;

            return result;
        }
        //*/
    }
}