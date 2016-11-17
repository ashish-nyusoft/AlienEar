using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Data;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models;
using Nop.Core.Data;
using Nop.Web.Framework.Menu;
//using Nop.Web.Framework.Web;
using Nop.Services.Events;
using Nop.Core.Events;
using Nop.Core.Domain.Messages;
using System;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class MyTestimonialPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin, IConsumer<EntityDeleted<NewsLetterSubscription>>
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private TestimonialRecordObjectContext _context;
        private IRepository<TestimonialRecord> _trialRepo;
        private ISettingService _settings;

        public MyTestimonialPlugin(TestimonialRecordObjectContext context, IRepository<TestimonialRecord> trialRepo, ISettingService commonSettings, IWebHelper webHelper)
        {
            _context = context;
            _trialRepo = trialRepo;
            _settingService = commonSettings;
            this._webHelper = webHelper;
        }
        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            //return new List<string>() { "right_side_column_after" };
            return new List<string>() { "homepage_testimonial_slider" };
            //return new List<string>() { "homepage_testimonial_slider" };
            //return new List<string>() { "main_column_after" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "MyTestimonial";
            routeValues = new RouteValueDictionary() { { "Namespaces", "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "MyTestimonial";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new MyTestimonialSettings()
            {
                ClientNameEnable = true,
                ShowOnHomePage = true,
                ShowTestimonialPage = true,
                PageSizeOptions = "5"
            };

            #region comment


            //#region addwidget
            //string _pathwidget = System.Web.HttpContext.Current.Server.MapPath("~/Views/Home/Index.cshtml");
            //string linewidget = "string", lenghtstring = "string";

            //int j = 0, length = 0;
            //System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);
            //while (lenghtstring != null)
            //{
            //    lenghtstring = readerwidget.ReadLine();
            //    length++;
            //}
            //string[] arrywidget = new string[length + 1];
            //readerwidget.Close();
            //System.IO.StreamReader readertowrite = new System.IO.StreamReader(_pathwidget);
            //while (linewidget != null)
            //{

            //    linewidget = readertowrite.ReadLine();

            //    if (!string.IsNullOrEmpty(linewidget))
            //    {


            //        arrywidget[j] = linewidget;
            //        //input += line;
            //    }
            //    j++;
            //}
            //arrywidget[j] = "@Html.Widget(\"homepage_testimonial_slider\")";
            //readertowrite.Close();
            //System.IO.File.WriteAllLines(_pathwidget, arrywidget);

            //#endregion
            //#region footer link add
            //string fullpage = "", _path, _path2, output = "", input = "";

            //string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\"))";
            //replacestr += "{";
            ////replacestr += "<li><a href=\"~/MyTestimonial/ViewTestimonials\">Testimonials</a></li>";
            //replacestr += "<li><a href=\"~/testimonials\">Testimonials</a></li>";
            //replacestr += "}";
            //replacestr += "</ul>";

            //int i = 0;
            //string[] arry = new string[1000];
            //arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
            //arry[1] = "@{@*TestimonialLink*@";
            //arry[2] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
            //arry[3] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
            //arry[4] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
            //arry[5] = "}@*TestimonialLink*@";
            //i = 6;

            //bool flag = true;
            //_path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");

            //string line = "string";
            //System.IO.StreamReader reader = new System.IO.StreamReader(_path);

            //while (line != null)
            //{

            //    line = reader.ReadLine();

            //    if (!string.IsNullOrEmpty(line))
            //    {

            //        if (line.Contains("</ul>") && flag)
            //        {

            //            line = line.Replace(line, replacestr);
            //            flag = false;
            //            //break;
            //        }
            //        arry[i] = line;
            //        input += line;
            //    }
            //    i++;
            //}

            //reader.Close();
            //System.IO.File.WriteAllLines(_path, arry);
            //#endregion

            #endregion



            _context.Install();
            //Remove Line.
            #region Remove header and footer lines funtion
            RemoveLines();
            #endregion

            //Write Line.
            #region Add Footer and widget For All Themes.
            DirectoryInfo themefilesdi = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Themes/"));
            DirectoryInfo mainviewfilesdi = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/"));
            string[] themefiles = new string[1000];
            string[] mainviewfiles = new string[1000];
            string[] themefilenames = new string[1000];
            string[] mainviewfilenames = new string[1000];
            int w = 0, x = 0, y = 0, z = 0;
            //Main View Index
            #region Main View Index
            foreach (var files in mainviewfilesdi.GetFiles("*Index*", SearchOption.AllDirectories))
            {
                mainviewfiles[w] = files.DirectoryName + "\\" + files.Name;
                w++;
            }
            string mainindexpath = "";
            for (int l = 0; mainviewfiles[l] != null; l++)
            {
                if (mainviewfiles[l].Contains("Home"))
                {
                    mainindexpath = mainviewfiles[l];
                    #region addwidget
                    string _pathwidget = mainindexpath;
                    string linewidget = "string", lenghtstring = "string";

                    int j = 0, length = 0;
                    System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);
                    while (lenghtstring != null)
                    {
                        lenghtstring = readerwidget.ReadLine();
                        length++;
                    }
                    string[] arrywidget = new string[length + 5];
                    readerwidget.Close();
                    System.IO.StreamReader readertowrite = new System.IO.StreamReader(_pathwidget);
                    while (linewidget != null)
                    {

                        linewidget = readertowrite.ReadLine();

                        if (!string.IsNullOrEmpty(linewidget))
                        {


                            arrywidget[j] = linewidget;
                            //input += line;
                        }
                        j++;
                    }
                    arrywidget[j] = "<div class=\"home-page-testimonial\">@*homepage_testimonial_slider*@";
                    arrywidget[j + 1] = "@Html.Widget(\"homepage_testimonial_slider\")";
                    arrywidget[j + 2] = "</div>@*homepage_testimonial_slider*@";
                    readertowrite.Close();
                    System.IO.File.WriteAllLines(_pathwidget, arrywidget);

                    #endregion

                }
            }
            #endregion
            //Main View Footer
            #region Main View Footer
            foreach (var files in mainviewfilesdi.GetFiles("*Footer*", SearchOption.AllDirectories))
            {
                mainviewfiles[x] = files.DirectoryName + "\\" + files.Name;
                x++;
            }
            for (int l = 0; mainviewfiles[l] != null; l++)
            {
                if (mainviewfiles[l].Contains("Common"))
                {
                    mainindexpath = mainviewfiles[l];
                    #region footer link add
                    string fullpage = "", _path, _path2, output = "", input = "";

                    string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\") && flag)";
                    replacestr += "{";
                    //replacestr += "<li><a href=\"~/MyTestimonial/ViewTestimonials\">Testimonials</a></li>";
                    replacestr += "<li><a href=\"~/testimonials\">Testimonials</a></li>";
                    replacestr += "}";
                    replacestr += "</ul>";

                    int i = 0;
                    string[] arry = new string[1000];
					arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
					arry[1] = "@using Nop.Core.Plugins@*TestimonialLink*@";
					arry[2] = "@{@*TestimonialLink*@";
					arry[3] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
					arry[4] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
					arry[5] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
					arry[6] = "    var currentsoteid = Nop.Core.Infrastructure.EngineContext.Current.Resolve<Nop.Core.IStoreContext>().CurrentStore.Id;";
					arry[7] = "    var _pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();";
					arry[8] = "    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\", LoadPluginsMode.All);";
					arry[9] = "    bool flag = true;";
					arry[10] = "    if (pluginDescriptor.LimitedToStores.Count > 0)";
					arry[11] = "    {@*TestimonialLink*@";
					arry[12] = "    if (!pluginDescriptor.LimitedToStores.Contains(currentsoteid))";
					arry[13] = "    {@*TestimonialLink*@";
					arry[14] = "    flag = false;";
					arry[15] = "    }@*TestimonialLink*@";
					arry[16] = "    }@*TestimonialLink*@";
					arry[17] = "}@*TestimonialLink*@";
					i = 18;

                    bool flag = true;
                    //_path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");
                    _path = mainindexpath;

                    string line = "string";
                    System.IO.StreamReader reader = new System.IO.StreamReader(_path);

                    while (line != null)
                    {

                        line = reader.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                        {

                            if (line.Contains("</ul>") && flag)
                            {

                                line = line.Replace(line, replacestr);
                                flag = false;
                                //break;
                            }
                            arry[i] = line;
                            input += line;
                        }
                        i++;
                    }

                    reader.Close();
                    System.IO.File.WriteAllLines(_path, arry);
                    #endregion
                }
            }
            #endregion
            //Theme Index
            #region Theme Index
            foreach (var files in themefilesdi.GetFiles("*Index*", SearchOption.AllDirectories))
            {
                themefiles[y] = files.DirectoryName + "\\" + files.Name;
                y++;
            }
            for (int l = 0; themefiles[l] != null; l++)
            {
                if (themefiles[l].Contains("Home\\Index.cshtml"))
                {
                    mainindexpath = themefiles[l];
                    #region addwidget
                    string _pathwidget = mainindexpath;
                    string linewidget = "string", lenghtstring = "string";

                    int j = 0, length = 0;
                    System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);
                    while (lenghtstring != null)
                    {
                        lenghtstring = readerwidget.ReadLine();
                        length++;
                    }
                    string[] arrywidget = new string[length + 5];
                    readerwidget.Close();
                    System.IO.StreamReader readertowrite = new System.IO.StreamReader(_pathwidget);
                    while (linewidget != null)
                    {

                        linewidget = readertowrite.ReadLine();

                        if (!string.IsNullOrEmpty(linewidget))
                        {


                            arrywidget[j] = linewidget;
                            //input += line;
                        }
                        j++;
                    }
                    arrywidget[j] = "<div class=\"home-page-testimonial\">@*homepage_testimonial_slider*@";
                    arrywidget[j + 1] = "@Html.Widget(\"homepage_testimonial_slider\")";
                    arrywidget[j + 2] = "</div>@*homepage_testimonial_slider*@";
                    readertowrite.Close();
                    System.IO.File.WriteAllLines(_pathwidget, arrywidget);

                    #endregion

                }
            }
            #endregion
            //Theme Footer
            #region Theme Footer
            foreach (var files in themefilesdi.GetFiles("*Footer*", SearchOption.AllDirectories))
            {
                themefiles[z] = files.DirectoryName + "\\" + files.Name;
                z++;
            }
            for (int l = 0; themefiles[l] != null; l++)
            {
                if (themefiles[l].Contains("Common\\Footer.cshtml"))
                {
                    mainindexpath = themefiles[l];
                    #region footer link add
                    string fullpage = "", _path, _path2, output = "", input = "";

                    string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\") && flag)";
                    replacestr += "{";
                    //replacestr += "<li><a href=\"~/MyTestimonial/ViewTestimonials\">Testimonials</a></li>";
                    replacestr += "<li><a href=\"~/testimonials\">Testimonials</a></li>";
                    replacestr += "}";
                    replacestr += "</ul>";

                    int i = 0;
                    string[] arry = new string[1000];
					arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
					arry[1] = "@using Nop.Core.Plugins@*TestimonialLink*@";
					arry[2] = "@{@*TestimonialLink*@";
					arry[3] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
					arry[4] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
					arry[5] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
					arry[6] = "    var currentsoteid = Nop.Core.Infrastructure.EngineContext.Current.Resolve<Nop.Core.IStoreContext>().CurrentStore.Id;";
					arry[7] = "    var _pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();";
					arry[8] = "    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\", LoadPluginsMode.All);";
					arry[9] = "    bool flag = true;";
					arry[10] = "    if (pluginDescriptor.LimitedToStores.Count > 0)";
					arry[11] = "    {@*TestimonialLink*@";
					arry[12] = "    if (!pluginDescriptor.LimitedToStores.Contains(currentsoteid))";
					arry[13] = "    {@*TestimonialLink*@";
					arry[14] = "    flag = false;";
					arry[15] = "    }@*TestimonialLink*@";
					arry[16] = "    }@*TestimonialLink*@";
					arry[17] = "}@*TestimonialLink*@";
                    i = 18;

                    bool flag = true;
                    //_path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");
                    _path = mainindexpath;

                    string line = "string";
                    System.IO.StreamReader reader = new System.IO.StreamReader(_path);

                    while (line != null)
                    {

                        line = reader.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                        {

                            if (line.Contains("</ul>") && flag)
                            {

                                line = line.Replace(line, replacestr);
                                flag = false;
                                //break;
                            }
                            arry[i] = line;
                            input += line;
                        }
                        i++;
                    }

                    reader.Close();
                    System.IO.File.WriteAllLines(_path, arry);
                    #endregion
                }
            }
            #endregion
            #endregion

            Directory.CreateDirectory(_webHelper.MapPath("~/Themes/DefaultClean/Scripts"));
            Directory.CreateDirectory(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/StoreImage"));

            var ico = _webHelper.MapPath("~/Administration/Content/images/ico_testimonial.png");
            if (!File.Exists(ico))
            {
                File.Copy(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/ico_testimonial.png"), _webHelper.MapPath("~/Administration/Content/images/ico_testimonial.png"));
            }

            var bxminjs = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.min.js");
            if (!File.Exists(bxminjs))
            {
                File.Copy(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Scripts/jquery.bxslider.min.js"), _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.min.js"));
            }

            var bxcss = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.css");
            if (!File.Exists(bxcss))
            {
                File.Copy(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Scripts/jquery.bxslider.css"), _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.css"));
            }

            var bxms = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.js");
            if (!File.Exists(bxms))
            {
                File.Copy(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Scripts/jquery.bxslider.js"), _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.js"));
            }

            var pageination = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/Pagination.js");
            if (!File.Exists(pageination))
            {
                File.Copy(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Scripts/jquery.bxslider.js"), _webHelper.MapPath("~/Themes/DefaultClean/Scripts/Pagination.js"));
            }


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.customername", "Client name");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.Title", "Title");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.Date", "Date");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.Description", "Description");

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.PublishedOnTestimonial", "Show on testimonial page");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.ShowOnHome", "Show on Home Page slider");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.DisplayOrder", "Display order");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.Published", "Publish");
            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.PageSizeOptions", "Page size");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.PageSizeOptionsList", "Page size options List");

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.customernameRequired", "Client name is required.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.titleRequired", "Title is required.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.dateRequired", "Date is required.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.Testimonial.descriptionRequired", "Description is required.");

            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.DescriptionEnable", "Description enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.TitleEnable", "Title enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.DateEnable", "Date enable");

            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.ClientNameEnable", "Client name enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.ShowOnHomePage", "Show Home Page Slider");
            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.ShowTestimonialPage", "Show testimonial page");

            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.ConfigureTestimonials", "Configure testimonials");

            this.AddOrUpdatePluginLocaleResource("Plugins.Testimonial.Slider.AddNewTestimonial", "Testimonial details");

            this.AddOrUpdatePluginLocaleResource("plugins.Testimonial.Slider.descriptionenable.hint", "Check this if you want to show Description at client side.");
            this.AddOrUpdatePluginLocaleResource("plugins.Testimonial.Slider.titleenable.hint", "Check this if you want to show Title at client side.");
            this.AddOrUpdatePluginLocaleResource("plugins.Testimonial.Slider.dateenable.hint", "Check this if you want to show Date at client side.");
            this.AddOrUpdatePluginLocaleResource("plugins.testimonial.slider.pagesizeoptions.hint", "Enter page count number for, per page how many testimonials you want to show at client side on testimonials page.");


            this.AddOrUpdatePluginLocaleResource("Admin.ContentManagement.Testimonial.Details", "Details Configuration");
            this.AddOrUpdatePluginLocaleResource("Admin.ContentManagement.Testimonial.Show", "Visiblity Configuration");
            this.AddOrUpdatePluginLocaleResource("plugins.testimonial.slider.showonhomepage.hint", "Check this if you want to keep  home page slider visible.");
            this.AddOrUpdatePluginLocaleResource("plugins.testimonial.slider.showtestimonialpage.hint", "Check this if you want to keep testimonial page visible.");
            this.AddOrUpdatePluginLocaleResource("plugins.testimonial.slider.clientnameenable.hint", "Check this if you want to keep client name visible in testimonials.");
            //string path = _webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/ico_testimonial.png");
            //string path2 = _webHelper.MapPath("~/Administration/Content/images/ico_testimonial.png");


            _settingService.SaveSetting(settings);


            //_settings.SetSetting<bool>("EnableTestimonial", false);
            base.Install();

        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            _settingService.DeleteSetting<MyTestimonialSettings>();

            #region Generatedatabasescript
            GenerateScript();
            #endregion

            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.customername");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.Title");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.Date");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.Description");

            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.ShowOnHome");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.DisplayOrder");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.Published");
            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.PageSizeOptions");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.PageSizeOptionsList");

            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.customernameRequired");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.titleRequired");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.dateRequired");
            this.DeletePluginLocaleResource("Plugins.Widgets.Testimonial.descriptionRequired");

            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.DescriptionEnable");
            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.TitleEnable");
            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.DateEnable");

            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.ClientNameEnable");
            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.ShowOnHomePage");
            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.ShowTestimonialPage");

            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.ConfigureTestimonials");

            this.DeletePluginLocaleResource("Plugins.Testimonial.Slider.AddNewTestimonial");

            this.DeletePluginLocaleResource("plugins.Testimonial.Slider.descriptionenable.hint");
            this.DeletePluginLocaleResource("plugins.Testimonial.Slider.titleenable.hint");
            this.DeletePluginLocaleResource("plugins.Testimonial.Slider.dateenable.hint");

            this.DeletePluginLocaleResource("Admin.ContentManagement.Testimonial.Details");
            this.DeletePluginLocaleResource("Admin.ContentManagement.Testimonial.Show");
            this.DeletePluginLocaleResource("plugins.testimonial.slider.pagesizeoptions.hint");

            var f = _webHelper.MapPath("~/Administration/Content/images/ico_testimonial.png");
            var js = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.min.js");
            var css = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.css");
            var bjs = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.js");
            var pagejs = _webHelper.MapPath("~/Themes/DefaultClean/Scripts/Pagination.js");
            if (File.Exists(f))
            {
                File.Delete(_webHelper.MapPath("~/Administration/Content/images/ico_testimonial.png"));
            }
            if (File.Exists(js))
            {
                File.Delete(_webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.min.js"));
            }
            if (File.Exists(css))
            {
                File.Delete(_webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.css"));
            }
            if (File.Exists(bjs))
            {
                File.Delete(_webHelper.MapPath("~/Themes/DefaultClean/Scripts/jquery.bxslider.js"));
            }
            if (File.Exists(pagejs))
            {
                File.Delete(_webHelper.MapPath("~/Themes/DefaultClean/Scripts/Pagination.js"));
            }

            _context.Uninstall();

            #region comment
            //#region removewidget
            //string _pathwidget = System.Web.HttpContext.Current.Server.MapPath("~/Views/Home/Index.cshtml");
            //string linewidget = "string", lenghtstring = "string";
            ////string[] arrywidget = new string[1000];
            //int j = 0, length = 0;
            //System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);

            //while (lenghtstring != null)
            //{
            //    lenghtstring = readerwidget.ReadLine();
            //    length++;
            //}
            //string[] arrywidget = new string[length];
            //readerwidget.Close();
            //System.IO.StreamReader readertoremove = new System.IO.StreamReader(_pathwidget);
            //while (linewidget != null)
            //{

            //    linewidget = readertoremove.ReadLine();

            //    if (!string.IsNullOrEmpty(linewidget))
            //    {
            //        if (linewidget.Contains("homepage_testimonial_slider"))
            //        {

            //            linewidget = linewidget.Replace(linewidget, "");
            //            //break;
            //        }
            //        arrywidget[j] = linewidget;
            //        //input += line;
            //    }
            //    j++;
            //}

            //readertoremove.Close();
            //System.IO.File.WriteAllLines(_pathwidget, arrywidget);
            //#endregion
            //#region footer link remove
            //string fullpage = "", _path, _path2, output = "", input = "";
            //string line = "string";
            //int i = 0;
            //string[] readarray = new string[1000];
            //string[] writearray = new string[1000];
            //_path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");
            //readarray = System.IO.File.ReadAllLines(_path);
            //System.IO.StreamReader reader = new System.IO.StreamReader(_path);

            //while (line != null)
            //{
            //    line = reader.ReadLine();
            //    if (!string.IsNullOrEmpty(line))
            //    {
            //        //if (line.Contains("</ul>") && flag)
            //        //{
            //        //    line = line.Replace(line, replacestr.ToString());
            //        //    flag = false;
            //        //    //break;
            //        //}
            //        if (line.Contains("testimonialsettingservice"))
            //        {
            //            line = line.Replace(line,"");
            //        }
            //        if (line.Contains("TestimonialLink"))
            //        {
            //            line = line.Replace(line, "");
            //        }
            //        if (line.Contains("mytestimonialsettings.showtestimonialpage"))
            //        {
            //            line = line.Replace(line, "");
            //        }
            //        if (line.Contains("widgetsettings.activewidgetsystemnames"))
            //        {
            //            line = line.Replace(line, "");
            //        }
            //        if (line.Contains("Testimonials"))
            //        {
            //            line = line.Replace(line, "</ul>");
            //        }
            //        writearray[i] = line;
            //        input += line;
            //    }
            //    i++;
            //}

            //reader.Close();
            //System.IO.File.WriteAllLines(_path, writearray);
            //#endregion
            #endregion

            #region Remove header and footer lines funtion
            RemoveLines();
            #endregion



            base.Uninstall();
        }

        public void RemoveLines()
        {
            #region Remove Footer and widget For All Themes.
            DirectoryInfo rthemefilesdi = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Themes/"));
            DirectoryInfo rmainviewfilesdi = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/"));
            string[] rthemefiles = new string[1000];
            string[] rmainviewfiles = new string[1000];
            string[] rthemefilenames = new string[1000];
            string[] rmainviewfilenames = new string[1000];
            int w = 0, x = 0, y = 0, z = 0;
            //Main View Index
            #region Main View Index
            foreach (var files in rmainviewfilesdi.GetFiles("*Index*", SearchOption.AllDirectories))
            {
                rmainviewfiles[w] = files.DirectoryName + "\\" + files.Name;
                w++;
            }
            string mainindexpath = "";
            for (int l = 0; rmainviewfiles[l] != null; l++)
            {
                if (rmainviewfiles[l].Contains("Home"))
                {
                    mainindexpath = rmainviewfiles[l];
                    #region removewidget
                    //string _pathwidget = System.Web.HttpContext.Current.Server.MapPath("~/Views/Home/Index.cshtml");
                    string _pathwidget = mainindexpath;
                    string linewidget = "string", lenghtstring = "string";
                    //string[] arrywidget = new string[1000];
                    int j = 0, length = 0;
                    System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);

                    while (lenghtstring != null)
                    {
                        lenghtstring = readerwidget.ReadLine();
                        length++;
                    }
                    string[] arrywidget = new string[length];
                    readerwidget.Close();
                    System.IO.StreamReader readertoremove = new System.IO.StreamReader(_pathwidget);
                    while (linewidget != null)
                    {

                        linewidget = readertoremove.ReadLine();

                        if (!string.IsNullOrEmpty(linewidget))
                        {
                            if (linewidget.Contains("homepage_testimonial_slider"))
                            {

                                linewidget = linewidget.Replace(linewidget, "");
                                //break;
                            }
                            arrywidget[j] = linewidget;
                            //input += line;
                        }
                        j++;
                    }

                    readertoremove.Close();
                    System.IO.File.WriteAllLines(_pathwidget, arrywidget);
                    #endregion
                }
            }
            #endregion
            //Main View Footer
            #region Main View Footer
            foreach (var files in rmainviewfilesdi.GetFiles("*Footer*", SearchOption.AllDirectories))
            {
                rmainviewfiles[x] = files.DirectoryName + "\\" + files.Name;
                x++;
            }
            for (int l = 0; rmainviewfiles[l] != null; l++)
            {
                if (rmainviewfiles[l].Contains("Common"))
                {
                    mainindexpath = rmainviewfiles[l];
                    #region footer link remove
                    string fullpage = "", _path, _path2, output = "", input = "";
                    string line = "string";
                    int i = 0;
                    string[] readarray = new string[1000];
                    string[] writearray = new string[1000];
                    _path = mainindexpath;
                    readarray = System.IO.File.ReadAllLines(_path);
                    System.IO.StreamReader reader = new System.IO.StreamReader(_path);

                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            //if (line.Contains("</ul>") && flag)
                            //{
                            //    line = line.Replace(line, replacestr.ToString());
                            //    flag = false;
                            //    //break;
                            //}
                            if (line.Contains("testimonialsettingservice"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("TestimonialLink"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("mytestimonialsettings.showtestimonialpage"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("Testimonials"))
                            {
                                line = line.Replace(line, "</ul>");
                            }
							if (line.Contains("currentsoteid"))
                            {
                                line = line.Replace(line, "");
                            }
							if (line.Contains("_pluginFinder"))
                            {
                                line = line.Replace(line, "");
                            }
							if (line.Contains("pluginDescriptor"))
                            {
                                line = line.Replace(line, "");
                            }
							if (line.Contains("flag"))
                            {
                                line = line.Replace(line, "");
                            }
							writearray[i] = line;
                            input += line;
                        }
                        i++;
                    }

                    reader.Close();
                    System.IO.File.WriteAllLines(_path, writearray);
                    #endregion
                }
            }
            #endregion
            //Theme Index
            #region Theme Index
            foreach (var files in rthemefilesdi.GetFiles("*Index*", SearchOption.AllDirectories))
            {
                rthemefiles[y] = files.DirectoryName + "\\" + files.Name;
                y++;
            }
            for (int l = 0; rthemefiles[l] != null; l++)
            {
                if (rthemefiles[l].Contains("Home\\Index.cshtml"))
                {
                    mainindexpath = rthemefiles[l];
                    #region removewidget
                    //string _pathwidget = System.Web.HttpContext.Current.Server.MapPath("~/Views/Home/Index.cshtml");
                    string _pathwidget = mainindexpath;
                    string linewidget = "string", lenghtstring = "string";
                    //string[] arrywidget = new string[1000];
                    int j = 0, length = 0;
                    System.IO.StreamReader readerwidget = new System.IO.StreamReader(_pathwidget);

                    while (lenghtstring != null)
                    {
                        lenghtstring = readerwidget.ReadLine();
                        length++;
                    }
                    string[] arrywidget = new string[length];
                    readerwidget.Close();
                    System.IO.StreamReader readertoremove = new System.IO.StreamReader(_pathwidget);
                    while (linewidget != null)
                    {

                        linewidget = readertoremove.ReadLine();

                        if (!string.IsNullOrEmpty(linewidget))
                        {
                            if (linewidget.Contains("homepage_testimonial_slider"))
                            {

                                linewidget = linewidget.Replace(linewidget, "");
                                //break;
                            }
                            arrywidget[j] = linewidget;
                            //input += line;
                        }
                        j++;
                    }

                    readertoremove.Close();
                    System.IO.File.WriteAllLines(_pathwidget, arrywidget);
                    #endregion
                }
            }
            #endregion
            //Theme Footer
            #region Theme Footer
            foreach (var files in rthemefilesdi.GetFiles("*Footer*", SearchOption.AllDirectories))
            {
                rthemefiles[z] = files.DirectoryName + "\\" + files.Name;
                z++;
            }
            for (int l = 0; rthemefiles[l] != null; l++)
            {
                if (rthemefiles[l].Contains("Common\\Footer.cshtml"))
                {
                    mainindexpath = rthemefiles[l];
                    #region footer link remove
                    string fullpage = "", _path, _path2, output = "", input = "";
                    string line = "string";
                    int i = 0;
                    string[] readarray = new string[1000];
                    string[] writearray = new string[1000];
                    _path = mainindexpath;
                    readarray = System.IO.File.ReadAllLines(_path);
                    System.IO.StreamReader reader = new System.IO.StreamReader(_path);

                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            //if (line.Contains("</ul>") && flag)
                            //{
                            //    line = line.Replace(line, replacestr.ToString());
                            //    flag = false;
                            //    //break;
                            //}
                            if (line.Contains("testimonialsettingservice"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("TestimonialLink"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("mytestimonialsettings.showtestimonialpage"))
                            {
                                line = line.Replace(line, "");
                            }
                          
                            if (line.Contains("Testimonials"))
                            {
                                line = line.Replace(line, "</ul>");
                            }
							if (line.Contains("currentsoteid"))
							{
								line = line.Replace(line, "");
							}
							if (line.Contains("_pluginFinder"))
							{
								line = line.Replace(line, "");
							}
							if (line.Contains("pluginDescriptor"))
							{
								line = line.Replace(line, "");
							}
							if (line.Contains("flag"))
							{
								line = line.Replace(line, "");
							}
                            writearray[i] = line;
                            input += line;
                        }
                        i++;
                    }

                    reader.Close();
                    System.IO.File.WriteAllLines(_path, writearray);
                    #endregion

                }
            }
            #endregion
            #endregion
        }

        public void GenerateScript()
        {
            string macadd = GetMAC();
            var settingsfile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Settings.txt");
            System.IO.StreamReader settingsreader = new System.IO.StreamReader(settingsfile);
            string settingsalllines = "";
            foreach (string line in System.IO.File.ReadAllLines(settingsfile))
            {
                settingsalllines += line;
            }
            string devidebycolun = settingsalllines.Split(':')[2];
            string[] devidebysemicolun = devidebycolun.Split(';');
            string dbName = "";
            Microsoft.SqlServer.Management.Smo.Server srv = new Microsoft.SqlServer.Management.Smo.Server();
            // really you would get these from config or elsewhere:
            srv.ConnectionContext.LoginSecure = false;
            foreach (string lines in devidebysemicolun)
            {
                if (lines.Contains("Data Source"))
                {
                    srv.ConnectionContext.ServerInstance = lines.Split('=')[1];
                }
                if (lines.Contains("Initial Catalog"))
                {
                    dbName = lines.Split('=')[1];
                }
                if (lines.Contains("User ID"))
                {
                    srv.ConnectionContext.Login = lines.Split('=')[1];
                }
                if (lines.Contains("Password"))
                {
                    srv.ConnectionContext.Password = lines.Split('=')[1];
                }
            }

            // really you would get these from config or elsewhere:
            //srv.ConnectionContext.LoginSecure = false;
            //srv.ConnectionContext.Login = "sa";
            //srv.ConnectionContext.Password = "Nyusoft123";
            //srv.ConnectionContext.ServerInstance = "NS1\\SQLEXPRESS2008R2";
            //string dbName = "nop34";

            Microsoft.SqlServer.Management.Smo.Database db = new Microsoft.SqlServer.Management.Smo.Database();
            db = srv.Databases[dbName];

            StringBuilder sb = new StringBuilder();
            StringBuilder resultScript = new StringBuilder(string.Empty);
            resultScript.AppendLine("IF OBJECT_ID('Testimonials') IS NOT NULL DROP TABLE Testimonials");
            foreach (Microsoft.SqlServer.Management.Smo.Table tbl in db.Tables)
            {
                if (tbl.Name == "Testimonials")
                {
                    Microsoft.SqlServer.Management.Smo.ScriptingOptions options = new Microsoft.SqlServer.Management.Smo.ScriptingOptions();
                    options.ClusteredIndexes = false;
                    options.DriPrimaryKey = true;
                    options.Default = true;
                    options.DriAll = true;
                    options.Indexes = true;
                    options.IncludeHeaders = false;
                    options.ScriptData = true;
                    options.IncludeIfNotExists = true;
                    //options.ScriptDrops = true;

                    //StringCollection coll = tbl.Script(options);
                    StringCollection coll = tbl.Script();
                    var datascript = tbl.EnumScript();

                    //parth

                    Microsoft.SqlServer.Management.Smo.Scripter scripter = new Microsoft.SqlServer.Management.Smo.Scripter();
                    scripter.Server = srv;
                    scripter.Options = options;

                    var script = scripter.EnumScript(new Microsoft.SqlServer.Management.Smo.SqlSmoObject[] { tbl });
                    //resultScript.AppendLine("BEGIN");
                    foreach (var line in script)
                    {
                        resultScript.Append(Environment.NewLine);
                        resultScript.AppendLine(line);
                    }
                    //resultScript.AppendLine("END");
                    //parth

                    foreach (string str in coll)
                    {
                        sb.Append(str);
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            //Directory.CreateDirectory(_webHelper.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/DBScripts"));
            int p = 0;
          //  Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts"));
          
      //      var file = System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt";
      //  changes by arjun on 15 th june

            string dt = DateTime.Now.ToString("yyyy-MM-dd");
           string tm = DateTime.Now.ToString("HH-mm-ss");
            //string day=DateTime.Parse(dt).ToShortDateString();

           //var file = System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + "_Date(yyyy-mm-dd)_" + dt + "_Time(hh-mm-ss)_" + tm + ".txt";
         //changes on 24/06/2015

           Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Backup"));

           var file = System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Backup/") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            if (!System.IO.File.Exists(file))
            {
                //System.IO.StreamWriter datas = System.IO.File.CreateText(System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt");
                // changes on 15 th june
                System.IO.StreamWriter datas = System.IO.File.CreateText(System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Backup/") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                datas.Write(resultScript.ToString());
                datas.Close();
            }
            else
            {
                System.IO.File.Delete(file);
                //System.IO.StreamWriter datas = System.IO.File.CreateText(System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt");
                // changes on 15 th june
                System.IO.StreamWriter datas = System.IO.File.CreateText(System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Widgets.NopTestimonial.Slider/Backup/") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                datas.Write(resultScript.ToString());
                datas.Close();
            }
        }
        private string GetMAC()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        //public SiteMapNode BuildMenuItem()
        //{
        //    //SiteMapNode node = new SiteMapNode { Visible = true, Title = "MyTestimonial", Url = "~/Admin/MyTestimonial/Manage", ImageUrl = "~/Administration/Content/images/ico_testimonial.png"};
        //    SiteMapNode node = new SiteMapNode { Visible = true, Title = "Testimonials", Url = "~/Admin/MyTestimonial/Manage", ImageUrl = "~/Administration/Content/images/ico_testimonial.png" };
        //    return node;
        //}

         public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "NyunopCommerce.Nop.Widgets.NopTestimonial.Slider",
                Title = "Testimonials",
                ControllerName = "MyTestimonial",
                ActionName = "Manage",
                Visible = true,    
                 ImageUrl = "~/Administration/Content/images/ico_testimonial.png",
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null && pluginNode.Visible)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        public bool Authenticate()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public void HandleEvent(EntityDeleted<NewsLetterSubscription> eventMessage)
        {
            throw new System.NotImplementedException();
        }

    }
}
