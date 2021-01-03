using OperationManger;
using Question;
using SurveyWebSite.Controllers;
using SurveyWebSite.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SurveyWebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static BaseLog.Logger Logger = new BaseLog.Logger();
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ModelBinders.Binders.Add(typeof(Qustion), new QustionModelBinder());
                Operation.RefreshData();
                QuestionController obj = new QuestionController(); 
                Operation.PutListToShow = obj.AutoRefresh;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message); 
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Languages"]; 
                if (cookie != null && cookie.Value != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cookie.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value); 
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }
    }
}
