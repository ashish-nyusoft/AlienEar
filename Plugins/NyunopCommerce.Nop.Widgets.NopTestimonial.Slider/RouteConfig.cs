using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Infrastructure;
using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routes;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider
{
    public class RouteConfig : IRouteProvider
    {
        public int Priority
        {
            get { return 0; }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
           // routes.MapRoute("Plugin.Testimonial.Slider.ManageTrackers",
           //     "MyTestimonial/Manage",
           //     new { controller = "MyTestimonial", action = "Manage" },
           //     new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           //);

            routes.MapRoute("Plugin.Testimonial.Slider.Manage",
                "MyTestimonial/Manage",
                new { controller = "MyTestimonial", action = "Manage" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            routes.MapRoute("Plugin.Testimonial.Slider.NewEntry",
                "MyTestimonial/NewEntry",
                new { controller = "MyTestimonial", action = "NewEntry" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            routes.MapRoute("Plugin.Testimonial.Slider.GetTestimonial",
                "MyTestimonial/GetTestimonial",
                new { controller = "MyTestimonial", action = "GetTestimonial" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            
            routes.MapRoute("Plugin.Testimonial.Slider.UpdateTrial",
                "MyTestimonial/UpdateTrial",
                new { controller = "MyTestimonial", action = "UpdateTrial" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            routes.MapRoute("Plugin.Testimonial.Slider.DeleteTrial",
                "MyTestimonial/DeleteTrial",
                new { controller = "MyTestimonial", action = "DeleteTrial" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );


            routes.MapRoute("Plugin.Testimonial.Slider.ViewTestimonials",
                "MyTestimonial/ViewTestimonials",
                new { controller = "MyTestimonial", action = "ViewTestimonials" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            routes.MapRoute("Plugin.Testimonial.Slider.PublicInfo",
                "MyTestimonial/PublicInfo",
                new { controller = "MyTestimonial", action = "PublicInfo" },
                new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );

            //routes.MapLocalizedRoute("TopicAuthenticate",
            //                "topic/authenticate",
            //                new { controller = "Topic", action = "Authenticate" },
            //                new[] { "Nop.Web.Controllers" });

            ViewEngines.Engines.Insert(0, new CustomViewEngine());
        }
    }
}
