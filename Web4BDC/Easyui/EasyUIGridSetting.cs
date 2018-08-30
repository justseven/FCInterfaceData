using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace Web4BDC.Easyui
{
    [ModelBinder(typeof(MvcEasyUIGridSettingBinder))]
    [Serializable]
    public class EasyUIGridSetting
    {
        public EasyUIGridSetting()
        {
            SortBy = new SortProperty();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public SortProperty SortBy { get; set; }
    }
    public class MvcEasyUIGridSettingBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            var setting = new EasyUIGridSetting();
            setting.PageIndex = Int32.Parse(request["page"] ?? "1");
            setting.PageSize = Int32.Parse(request["rows"] ?? "10");
            setting.SortBy.PropertyName = request["sort"] ?? string.Empty;
            if (request["order"] != null && request["order"] == "asc")
                setting.SortBy.SortType = SortType.Asc;
            else if (request["order"] != null && request["order"] == "desc")
                setting.SortBy.SortType = SortType.Desc;
            else
                setting.SortBy.SortType = SortType.Default;
            return setting;
        }
    }
}