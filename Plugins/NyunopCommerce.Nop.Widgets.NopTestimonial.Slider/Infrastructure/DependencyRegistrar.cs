using Autofac;
using Autofac.Core;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Data;
using Nop.Data;
using Nop.Core.Data;
using Autofac.Core;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models;
using Nop.Web.Framework.Mvc;
using Nop.Core.Configuration;



namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_product_view_tracker";

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder,NopConfig config)
        {
            //we cache presentation models between requests
            builder.RegisterType<MyTestimonialController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));

            this.RegisterPluginDataContext<TestimonialRecordObjectContext>(builder, CONTEXT_NAME);

            builder.RegisterType<EfRepository<TestimonialRecord>>()
                .As<IRepository<TestimonialRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
