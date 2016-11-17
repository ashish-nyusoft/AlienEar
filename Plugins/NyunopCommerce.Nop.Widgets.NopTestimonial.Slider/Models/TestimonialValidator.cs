using FluentValidation;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models
{
    public class TestimonialValidator : AbstractValidator<TestimonialRecord>
    {
        public TestimonialValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Testimonial.customernameRequired"));
                

            //RuleFor(x => x.Title)
            //    .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Testimonial.titleRequired"));

            //RuleFor(x => x.date)
            //    .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Testimonial.dateRequired"));

            //RuleFor(x => x.description)
            //    .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Testimonial.descriptionRequired"));

            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Testimonial.descriptionRequired"));
        }
    }
}
