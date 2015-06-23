using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace FCMA.RewardRecognition.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfiguration.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            LocalDatabaseConfiguration.RegisterDatabases();
        }
    }
}