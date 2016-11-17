using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routes;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            //routes.MapLocalizedRoute("MyTestimonial",
            //  "Admin/MyTestimonial/Manage",
            //  new { controller = "MyTestimonial", action = "Manage"},
            //  new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }).DataTokens.Add("area","Admin");
            var manageroute = routes.MapRoute("Manageroute",
              "Admin/MyTestimonial/Manage",
              new { controller = "MyTestimonial", action = "Manage" },
              new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );//.DataTokens.Add("Area", "Admin");
            routes.Remove(manageroute);
            routes.Insert(0, manageroute);

            var newentryroute = routes.MapRoute("NewEntryroute",
              "Admin/MyTestimonial/NewEntry",
              new { controller = "MyTestimonial", action = "NewEntry" },
              new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
           );//.DataTokens.Add("Area", "Admin");
            routes.Remove(newentryroute);
            routes.Insert(0, newentryroute);

            var editroute = routes.MapRoute("Editroute",
               "Admin/MyTestimonial/Edit/{id}",
               new { controller = "MyTestimonial", action = "Edit" },
               new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
            );//.DataTokens.Add("Area", "Admin");
            routes.Remove(editroute);
            routes.Insert(0, editroute);

            var tetimonialsroute = routes.MapRoute("Tetimonialsroute",
               "testimonials",
               new { controller = "MyTestimonial", action = "ViewTestimonials" },
               new[] { "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }
            );//.DataTokens.Add("Area", "Admin");
            routes.Remove(tetimonialsroute);
            routes.Insert(0, tetimonialsroute);
            
        }

        public int Priority
        {
            get
            {
                return -100000;
            }
        }
    }
}
