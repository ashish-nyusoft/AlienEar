
using Nop.Core.Configuration;
using System.Web.Mvc;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider
{
    public class MyTestimonialSettings : ISettings
    {
    //    public int Picture1Id { get; set; }

        
        public bool ClientNameEnable { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool ShowTestimonialPage { get; set; }
        public string PageSizeOptions { get; set; }

        //public bool DescriptionEnable { get; set; }
        //public bool TitleEnable { get; set; }
        //public bool DateEnable { get; set; }
    }
}