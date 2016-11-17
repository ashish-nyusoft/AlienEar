//using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Domain;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Data
{
    public class TestimonialRecordMap : EntityTypeConfiguration<TestimonialRecord>
    {
        public TestimonialRecordMap()
        {
            ToTable("Testimonials");
            HasKey(m => m.Id);

            Property(m => m.ClientName);
            //Property(m => m.Title);
            Property(m => m.Description);
            //Property(m => m.date);
            Property(m => m.ShowOnHome);
            Property(m => m.DisplayOrder);
            Property(m => m.Published);
            Property(m => m.PublishedOnTestimonials);
            Property(m => m.ImagePath);
        }
        
    }
}
