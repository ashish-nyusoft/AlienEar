using FluentValidation.Attributes;
using Nop.Core;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models
{
    [Validator(typeof(TestimonialValidator))]
    public class TestimonialRecord : BaseEntity
    {
        //public TestimonialRecord()
        //{
        //    ConfigurationModel = new List<ConfigurationModel>();
        //}

        //public IList<ConfigurationModel> ConfigurationModel { get; set; }
    

        public int Id { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.Testimonial.customername")]
        public string ClientName { get; set; }

        //[NopResourceDisplayName("Plugins.Widgets.Testimonial.Title")]
        //public string Title { get; set; }

        
        [NopResourceDisplayName("Plugins.Widgets.Testimonial.Description")]
        [AllowHtml]
        public string Description { get; set; }

        //[NopResourceDisplayName("Plugins.Widgets.Testimonial.Date")]
        //public string date { get; set; }        

        [NopResourceDisplayName("Plugins.Widgets.Testimonial.ShowOnHome")]
        public bool ShowOnHome { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonial.DisplayOrder")]
        public  int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonial.PublishedOnTestimonial")]
        public bool PublishedOnTestimonials { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonial.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Testimonial.ImagePath")]
        public string ImagePath { get; set; }

        
    }
}
