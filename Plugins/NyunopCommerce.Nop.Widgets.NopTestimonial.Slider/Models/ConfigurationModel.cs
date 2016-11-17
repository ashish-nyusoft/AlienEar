using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework;
using System.Web.Mvc;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Testimonial.Slider.ClientNameEnable")]
        public bool ClientNameEnable { get; set; }

        [NopResourceDisplayName("Plugins.Testimonial.Slider.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [NopResourceDisplayName("Plugins.Testimonial.Slider.ShowTestimonialPage")]
        public bool ShowTestimonialPage { get; set; }

        [NopResourceDisplayName("Plugins.Testimonial.Slider.PageSizeOptions")]
        public string PageSizeOptions { get; set; }
        //[NopResourceDisplayName("Plugins.Testimonial.Slider.TitleEnable")]
        //public bool TitleEnable { get; set; }

        //[NopResourceDisplayName("Plugins.Testimonial.Slider.DateEnable")]
        //public bool DateEnable { get; set; }
    }
}
