using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LadowebservisMVC
{
   
        public class RouteConfig
        {
            public static void RegisterRoutes(RouteCollection routes)
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute("zdravie", "zdravie", new { controller = "Home", action = "Zdravie" });
            routes.MapRoute("itservis", "itservis", new { controller = "Home", action = "ITservis" });
            routes.MapRoute("kontakt", "kontakt", new { controller = "Home", action = "Contact" });
            routes.MapRoute("odoslanie-spravy", "odoslanie-spravy", new { controller = "Home", action = "Kontakt" });
            routes.MapRoute("login", "login", new { controller = "Home", action = "Login" });
            routes.MapRoute("member", "member", new { controller = "Home", action = "Member" });
            routes.MapRoute("memberinfo", "memberinfo", new { controller = "Home", action = "MemberInfo" });
            routes.MapRoute("registracia-uzivatela", "registracia-uzivatela", new { controller = "Home", action = "Registracia" });
            routes.MapRoute("kosik", "kosik", new { controller = "Home", action = "Kosik" });
            routes.MapRoute("objednavka", "objednavka", new { controller = "Home", action = "Objednavka" });
            routes.MapRoute("hudba", "hudba", new { controller = "Home", action = "Hudba" });




            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Zdravie", id = UrlParameter.Optional }
                );
            }
        }
    }
        
    

