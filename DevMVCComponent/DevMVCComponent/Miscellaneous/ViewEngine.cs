using System.IO;
using System.Web.Mvc;

namespace DevMvcComponent.Miscellaneous {
    /// <summary>
    ///     Additional functionalities to ViewEngine.
    /// </summary>
    //public static class ViewEngine {
    //    /// <summary>
    //    ///     Convert Razor view to string
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="viewName"></param>
    //    /// <param name="model"></param>
    //    /// <returns></returns>
    //    public static string RenderRazorViewToString(Controller context, string viewName, object model) {
    //        var viewData = context.ViewData;
    //        var tempData = context.TempData;
    //        viewData.Model = model;
    //        using (var sw = new StringWriter()) {
    //            var viewResult = ViewEngines.Engines.FindPartialView(context.ControllerContext, viewName);
    //            var viewContext = new ViewContext(context.ControllerContext, viewResult.View, viewData, tempData, sw);
    //            viewResult.View.Render(viewContext, sw);
    //            viewResult.ViewEngine.ReleaseView(context.ControllerContext, viewResult.View);
    //            return sw.GetStringBuilder().ToString();
    //        }
    //    }
    //}
}