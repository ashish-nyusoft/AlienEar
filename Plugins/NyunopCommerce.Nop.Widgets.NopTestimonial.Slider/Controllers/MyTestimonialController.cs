using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Infrastructure.Cache;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Core.Data;
using Nop.Services.Catalog;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.News;
using Nop.Web.Framework.Kendoui;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework.Mvc;
using System.Text.RegularExpressions;
using System.Web;
using Nop.Core.Infrastructure;
using System.IO;
using System;
using Nop.Web.Framework;
using NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Data;
using System.Data.Entity;
using System.Net.NetworkInformation;
using System.Text;
//using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using Nop.Data;
using Nop.Core.Domain.Media;


namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Controllers
{

    public class MyTestimonialController : BasePluginController
    {
        //private System.Data.Entity.DbContext db = new System.Data.Entity.DbContext();
        private IRepository<TestimonialRecord> _TestimonialRepository;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private IProductService _productService;
        private ISettingService _settings;
        private INewsLetterSubscriptionService _mailingService;
        private readonly IPermissionService _permissionService;
        private readonly CommonSettings _commonSettings;
        //private readonly StoreInformationSettings _storeInformationSettings;
        private readonly BlogSettings _blogSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly ForumSettings _forumSettings;
        private readonly NewsSettings _newsSettings;
        private readonly IWebHelper _webHelper;
        private readonly IDownloadService _IDownloadService;


        public MyTestimonialController(IRepository<TestimonialRecord> TestimonialRepository, IProductService productService, ISettingService settingService, INewsLetterSubscriptionService mailService, IWorkContext workContext, IStoreContext storeContext,
             IStoreService storeService, IDownloadService IDownloadService)
        {
            _IDownloadService = IDownloadService;
            _productService = productService;
            _TestimonialRepository = TestimonialRepository;
            _settingService = settingService;
            _mailingService = mailService;
            _workContext = workContext;
            _storeContext = storeContext;
            _storeService = storeService;

        }

        public class EditPageSizeOptions
        {
            public IList<SelectListItem> PageSizeOptionsList { get; set; }
        }


        [AdminAuthorize]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope

            var store = EngineContext.Current.Resolve<IStoreContext>().CurrentStore;
            var setting = EngineContext.Current.Resolve<ISettingService>();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var testimonialSettings = _settingService.LoadSetting<MyTestimonialSettings>(storeScope);
            var model = new ConfigurationModel();


            model.ClientNameEnable = testimonialSettings.ClientNameEnable;
            model.ShowOnHomePage = testimonialSettings.ShowOnHomePage;
            model.ShowTestimonialPage = testimonialSettings.ShowTestimonialPage;
            model.PageSizeOptions = testimonialSettings.PageSizeOptions;



            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/Configure.cshtml", model);

        }

        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var testimonialSettings = _settingService.LoadSetting<MyTestimonialSettings>(storeScope);

            //testimonialSettings.DescriptionEnable = model.DescriptionEnable;
            //testimonialSettings.TitleEnable = model.TitleEnable;
            //testimonialSettings.DateEnable = model.DateEnable;
            testimonialSettings.ClientNameEnable = model.ClientNameEnable;
            testimonialSettings.ShowOnHomePage = model.ShowOnHomePage;
            testimonialSettings.ShowTestimonialPage = model.ShowTestimonialPage;
            if (model.PageSizeOptions == null)
                testimonialSettings.PageSizeOptions = Convert.ToString("5");
            else
                testimonialSettings.PageSizeOptions = model.PageSizeOptions;
            ViewBag.ShowTestimonialConfig = model.ShowTestimonialPage;

            _settingService.SaveSetting(testimonialSettings, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [AdminAuthorize]
        public ActionResult Manage()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
            //    return AccessDeniedView();
            //var model = _TestimonialRepository.Table.ToList().OrderByDescending(x => x.DisplayOrder);
            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/Manage.cshtml");
            //return RedirectToRoute("admin/MyTestimonial/Manage");

            //return RedirectToRoute("Manage");
        }

        public ActionResult ViewTestimonials()
        {
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var testimonialSettings = _settingService.LoadSetting<MyTestimonialSettings>(_storeContext.CurrentStore.Id);
            var enablepluginmodel = _settingService.GetSettingByKey("widgetsettings.activewidgetsystemnames", "null", 0, false);

            if (testimonialSettings.ShowTestimonialPage && enablepluginmodel.Contains("NyunopCommerce.Nop.Widgets.NopTestimonial.Slider"))
            {
                var model = new List<TestimonialRecord>();

                var testimonials = _TestimonialRepository.Table.ToList().OrderByDescending(x => x.DisplayOrder);
                foreach (var data in testimonials)
                {
                    if (data.Published && data.PublishedOnTestimonials)
                    {
                        TestimonialRecord obj = new TestimonialRecord();

                        obj.ClientName = data.ClientName;
                        //obj.Title = data.Title;
                        var decodestr = HttpUtility.HtmlDecode(data.Description);
                        obj.Description = decodestr;
                        obj.ImagePath = data.ImagePath;
                        //obj.date = data.date;
                        //obj.ConfigurationModel. = testimonialSettings.DescriptionEnable;
                        //obj.ConfigurationModel.TitleEnable = testimonialSettings.DescriptionEnable;
                        //ViewBag.DescriptionEnable = testimonialSettings.DescriptionEnable;
                        //ViewBag.TitleEnable = testimonialSettings.TitleEnable;
                        //ViewBag.DateEnable = testimonialSettings.DateEnable;
                        ViewBag.ClientName = testimonialSettings.ClientNameEnable;
                        ViewBag.Published = data.Published;
                        model.Add(obj);

                    }

                }
                var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
                var testimonialSettingsforpage = _settingService.LoadSetting<MyTestimonialSettings>(storeScope);
                string pageSizeOptions = testimonialSettingsforpage.PageSizeOptions;
                TempData["pagecount"] = testimonialSettingsforpage.PageSizeOptions;

                var pageSizes = pageSizeOptions.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                bool flag = true;
                //TestimonialRecord obj2 = new TestimonialRecord();
                List<SelectListItem> pagelistitems = new List<SelectListItem>();
                #region not in use(Page size options)
                //foreach (var pageSize in pageSizes)
                //{
                //    int temp = 0;
                //    if (!int.TryParse(pageSize, out temp))
                //    {
                //        continue;
                //    }
                //    if (temp <= 0)
                //    {
                //        continue;
                //    }
                //    if (flag)
                //    {
                //        TempData["firstcount"] = Convert.ToInt32(pageSize);
                //        flag = false;
                //    }
                //    var currentPageUrl = Request.Url.AbsoluteUri;
                //    //var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                //    //sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");
                //    //IList<SelectListItem> pagelistitems = new IList<SelectListItem>();

                //    pagelistitems.Add(new SelectListItem()
                //    {
                //        Text = pageSize,
                //        Value = pageSize
                //        //Value = String.Format(currentPageUrl, pageSize)
                //        //Selected = pageSize.Equals(pageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                //    });
                //    //EditPageSizeOptions objpage = new EditPageSizeOptions();
                //    //objpage.PageSizeOptionsList.Add(new SelectListItem()
                //    //{
                //    //    Text = pageSize,
                //    //    Value = pageSize
                //    //    //Value = String.Format(currentPageUrl, pageSize)
                //    //    //Selected = pageSize.Equals(pageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                //    //});

                //}
                //if (pagelistitems.Count > 0)
                //{

                //    ViewBag.Page = pagelistitems;
                //    ViewData["Page"] = pagelistitems;
                //}
                #endregion
                TempData["TestimonialCount"] = model.Count;
                return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/ViewTestimonials.cshtml", model);
            }
            else
            {
                //Response.Redirect("/");
                Response.Redirect(Url.RouteUrl("HomePage"));
            }

            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/ViewTestimonials.cshtml");
        }

        //[ChildActionOnly]
        public ActionResult PublicInfo()
        {

            //var model = new List<TestimonialRecord>();
            ////var model = new TestimonialRecord();
            //var testimonials = _TestimonialRepository.Table.ToList();
            ////var testimonials = _TestimonialRepository.Table.ToList();
            //foreach (var items in testimonials)
            //{
            //    TestimonialRecord obj = new TestimonialRecord();
            //    obj.CustomerName = items.CustomerName;
            //    obj.Title = items.Title;
            //    obj.description = items.description;
            //    obj.date = items.date;
            //    model.Add(obj);
            //}
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var testimonialSettings = _settingService.LoadSetting<MyTestimonialSettings>(_storeContext.CurrentStore.Id);
            ViewBag.ClientName = testimonialSettings.ClientNameEnable;
            var model = new List<TestimonialRecord>();

            var dataorderbydesc = _TestimonialRepository.Table.ToList().OrderByDescending(x => x.DisplayOrder).Where(x => x.ShowOnHome).Take(5);
            //var testimonials = dataorderbydesc.Take(5);
            //var testimonials = _TestimonialRepository.Table.ToList().Take(5);


            foreach (var data in dataorderbydesc)
            {
                if (data.Published && data.ShowOnHome)
                {
                    TestimonialRecord obj = new TestimonialRecord();
                    obj.ClientName = data.ClientName;
                    //obj.Title = data.Title;
                    var decodestr = HttpUtility.HtmlDecode(data.Description);
                    obj.Description = decodestr;
                    obj.ImagePath =  data.ImagePath;
                    //obj.date = data.date;
                    //obj.ConfigurationModel. = testimonialSettings.DescriptionEnable;
                    //obj.ConfigurationModel.TitleEnable = testimonialSettings.DescriptionEnable;
                    //ViewBag.DescriptionEnable = testimonialSettings.DescriptionEnable;
                    //ViewBag.TitleEnable = testimonialSettings.TitleEnable;
                    //ViewBag.DateEnable = testimonialSettings.DateEnable;
                    model.Add(obj);
                }

            }

            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/PublicInfo.cshtml", model.ToList());
        }

        public ActionResult NewEntry()
        {
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var model = new TestimonialRecord();
            var datamodel = _TestimonialRepository.Table;
            //default values
            if (datamodel.Count() > 0)
            {
                int maxdisplayorder = _TestimonialRepository.Table.ToList().Max(x => x.DisplayOrder);
                model.DisplayOrder = maxdisplayorder + 1;
            }
            else
            {
                model.DisplayOrder = 1;
            }
            model.Published = true;
            model.ShowOnHome = true;
            model.PublishedOnTestimonials = true;
            //return View(model);
            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/NewEntry.cshtml", model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewEntry(TestimonialRecord model, HttpPostedFileBase files)
        {
            //var testimonials = _TestimonialRepository.Table.ToList().OrderByDescending(x => x.Id);


            //if (!string.IsNullOrEmpty(model.description))
            //{
            //file


            Download dl = new Download();
            dl.DownloadGuid = Guid.NewGuid();
            string filePath = "content/images/default-image.png";
            if (files != null)
            {
                files.SaveAs(Server.MapPath("~/Content/UserImage/" + dl.DownloadGuid + ".jpg"));
                filePath = "Content/UserImage/" + dl.DownloadGuid + ".jpg";
            }

            model.ImagePath = filePath;
            var datamodel = _TestimonialRepository.Table;
            if (model != null)
            {
                if (model.DisplayOrder == 0)
                {
                    if (datamodel.Count() > 0)
                    {
                        int maxdisplayorder = _TestimonialRepository.Table.ToList().Max(x => x.DisplayOrder);
                        model.DisplayOrder = maxdisplayorder + 1;

                    }
                    else
                    {
                        model.DisplayOrder = 1;
                    }

                }
            }
            else
            {
                model.DisplayOrder = 1;
            }
            _TestimonialRepository.Insert(model);
            TempData["newentry"] = "Record has been added successfully.";

            var testimonials = _TestimonialRepository.Table.ToList();

            var gridModel = new DataSourceResult
            {
                Data = testimonials,
                Total = testimonials.Count
            };
            //return View("Manage");
            return RedirectToAction("Manage");
            //}
            //else
            //{
            //    TempData["lbldescriptionerror"] = "Description is required";
            //    TempData["lblname"]=model.ClientName;
            //    return RedirectToAction("NewEntry");

            //}
        }


        [ValidateInput(false)]
        public ActionResult SaveAndContinue(TestimonialRecord model)
        {
            _TestimonialRepository.Insert(model);

            var testimonials = _TestimonialRepository.Table.ToList();

            var gridModel = new DataSourceResult
            {
                Data = testimonials,
                Total = testimonials.Count
            };
            return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/NewEntry.cshtml");
        }

        //[ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterModel()
            {
                StoreName = "XYZ",
                WishlistEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                SitemapEnabled = _commonSettings.SitemapEnabled,
                WorkingLanguageId = _workContext.WorkingLanguage.Id,
                //FacebookLink = _storeInformationSettings.FacebookLink,
                //TwitterLink = _storeInformationSettings.TwitterLink,
                //YoutubeLink = _storeInformationSettings.YoutubeLink,
                //GooglePlusLink = _storeInformationSettings.GooglePlusLink,
                BlogEnabled = _blogSettings.Enabled,
                CompareProductsEnabled = _catalogSettings.CompareProductsEnabled,
                ForumEnabled = _forumSettings.ForumsEnabled,
                NewsEnabled = _newsSettings.Enabled,
                RecentlyViewedProductsEnabled = _catalogSettings.RecentlyViewedProductsEnabled,
                RecentlyAddedProductsEnabled = _catalogSettings.NewProductsEnabled,
                DisplayTaxShippingInfoFooter = _catalogSettings.DisplayTaxShippingInfoFooter
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult GetTestimonial(DataSourceRequest command)
        {
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var testimonials = _TestimonialRepository.Table.ToList().OrderBy(x => x.DisplayOrder);

            //var testimonials = _TestimonialRepository.Table.ToList();
            var testimnonialRecord = testimonials.Select(x =>
            {
                return new TestimonialRecord()
                {
                    ClientName = x.ClientName,
                    Id = x.Id,
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    Published = x.Published

                };
            }).AsQueryable();

            var gridModel = new DataSourceResult
            {
                Data = testimnonialRecord.PagedForCommand(command).ToList(),
                Total = testimonials.Count()
            };

            return Json(gridModel);
        }


        [HttpPost]
        //public ActionResult UpdateTestimonial(TestimonialRecord TestimonialUpdate)
        public ActionResult UpdateTestimonial(DataSourceRequest command, int Id, string Title, string CustomerName, string date)
        {
            var trial = _TestimonialRepository.GetById(Id);

            //trial.Title = Title;
            trial.ClientName = CustomerName;
            //trial.date = date;
            //trial.description = TestimonialUpdate.description;

            _TestimonialRepository.Update(trial);

            return new NullJsonResult();
        }

        public ActionResult Edit(int id)
        {

            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            //var poll = _pollService.GetPollById(id);
            var model = _TestimonialRepository.GetById(id);
            //if (poll == null)
            //    //No poll found with the specified id
            //    return RedirectToAction("List");

            //ViewBag.AllLanguages = _languageService.GetAllLanguages(true);

            //var model = trial.ToModel();
            //model. = poll.StartDateUtc;
            //model.EndDate = poll.EndDateUtc;
            return View(model);
            //return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TestimonialRecord model, HttpPostedFileBase files)
        {

            //file

            if (files != null)
            {

                Download dl = new Download();
                dl.DownloadGuid = Guid.NewGuid();
                string filePath = "~/content/images/default-image.png";
                if (files != null)
                {
                    files.SaveAs(Server.MapPath("~/Content/UserImage/" + dl.DownloadGuid + ".jpg"));
                    filePath = "Content/UserImage/" + dl.DownloadGuid + ".jpg";
                }


                model.ImagePath = filePath;
            }
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            //if (!string.IsNullOrEmpty(model.description))
            //{
            //var poll = _pollService.GetPollById(id);
            var trial = _TestimonialRepository.GetById(model.Id);
            trial.ClientName = model.ClientName;
            //var decodestr = HttpUtility.HtmlDecode(model.description);
            trial.Description = model.Description;
            if (files != null)
            {
                trial.ImagePath = model.ImagePath;
            }
            trial.Published = model.Published;
            trial.ShowOnHome = model.ShowOnHome;
            trial.PublishedOnTestimonials = model.PublishedOnTestimonials;
            trial.DisplayOrder = model.DisplayOrder;
            _TestimonialRepository.Update(trial);
            //return View("~/Plugins/Widgets.NopTestimonial.Slider/Views/Manage.cshtml");
            return RedirectToAction("Manage");
            //return View(model);
            //return View();
            //}
            //else
            //{
            //    TempData["lbldescriptionerroredit"] = "Description is required.";

            //    return RedirectToAction("Edit", model.Id);
            //}
        }
        public ActionResult Delete(int id)
        {

            var testimonials = _TestimonialRepository.GetById(id);
            _TestimonialRepository.Delete(testimonials);
            return RedirectToAction("Manage");
        }
        [HttpPost]
        public ActionResult DeleteTestimonial(DataSourceRequest command, int Id)
        {


            var testimonials = _TestimonialRepository.GetById(Id);
            _TestimonialRepository.Delete(testimonials);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (selectedIds != null)
            if (selectedIds.Count > 0)
            {
                foreach (int items in selectedIds)
                {
                    var testimonials = _TestimonialRepository.GetById(items);
                    _TestimonialRepository.Delete(testimonials);
                }
                return Json(new { Result = true });
            }
            else
            {
                return Json(new { Result = false });
            }

        }


        public ActionResult Replacefooter()
        {
            string fullpage = "", _path, _path2, output = "", input = "";

            string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\"))";
            replacestr += "{";
            replacestr += "<li><a href=\"~/Common/Footer\">Testimonials</a></li>";
            replacestr += "}";
            replacestr += "</ul>";

            int i = 0;
            string[] arry = new string[1000];
            arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
            arry[1] = "@{@*TestimonialLink*@";
            arry[2] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
            arry[3] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
            arry[4] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
            arry[5] = "}@*TestimonialLink*@";
            i = 6;

            bool flag = true;
            _path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");

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
            //using (StreamWriter writer = new StreamWriter(_path, true))
            //{
            //    {
            // //       string output = input.Replace("</ul>", "<li>Testimoniallink</li></ul>");

            //        for (int n = 0; n <= i; n++)
            //        {
            //            writer.Write(arry[n]);
            //        }
            //    }
            //    writer.Close();
            //}

            string name = "";

            return RedirectToAction("ViewTestimonials");
        }
        public ActionResult Rmovefooter()
        {
            string fullpage = "", _path, _path2, output = "", input = "";
            string line = "string";
            int i = 0;
            string[] readarray = new string[1000];
            string[] writearray = new string[1000];
            _path = System.Web.HttpContext.Current.Server.MapPath("~/Views/Common/Footer.cshtml");
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
                    if (line.Contains("widgetsettings.activewidgetsystemnames"))
                    {
                        line = line.Replace(line, "");
                    }
                    if (line.Contains("Testimonials"))
                    {
                        line = line.Replace(line, "</ul>");
                    }
                    writearray[i] = line;
                    input += line;
                }
                i++;
            }

            reader.Close();
            System.IO.File.WriteAllLines(_path, writearray);
            return RedirectToAction("ViewTestimonials");
        }

        public ActionResult GetFiles()
        {
            //string[] fileInfoEnum = Directory.GetFiles(Server.MapPath("~/ProjectImage/" + Request.QueryString["UserId"] + "/" + Request.QueryString["ProjectId"] + "/Completed/").Replace("QuickImageEdit", "QuickImageApp"));
            //string[] themefiles = Directory.GetFiles(Server.MapPath("~/Themes/"));

            //string[] Viewfiles = Directory.GetFiles(Server.MapPath("~/Views/Common/"));
            #region Add Footer and widget For All Themes.
            DirectoryInfo themefilesdi = new DirectoryInfo(Server.MapPath("~/Themes/"));
            DirectoryInfo mainviewfilesdi = new DirectoryInfo(Server.MapPath("~/Views/"));
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
                    string[] arrywidget = new string[length + 1];
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
                    arrywidget[j] = "@Html.Widget(\"homepage_testimonial_slider\")";
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

                    string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\"))";
                    replacestr += "{";
                    //replacestr += "<li><a href=\"~/MyTestimonial/ViewTestimonials\">Testimonials</a></li>";
                    replacestr += "<li><a href=\"~/testimonials\">Testimonials</a></li>";
                    replacestr += "}";
                    replacestr += "</ul>";

                    int i = 0;
                    string[] arry = new string[1000];
                    arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
                    arry[1] = "@{@*TestimonialLink*@";
                    arry[2] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
                    arry[3] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
                    arry[4] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
                    arry[5] = "}@*TestimonialLink*@";
                    i = 6;

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
                    string[] arrywidget = new string[length + 1];
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
                    arrywidget[j] = "@Html.Widget(\"homepage_testimonial_slider\")";
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

                    string replacestr = "@if (testimonialpluginmodel.Contains(\"True\") && testimonialenablepluginmodel.Contains(\"NyunopCommerce.Nop.Widgets.NopTestimonial.Slider\"))";
                    replacestr += "{";
                    //replacestr += "<li><a href=\"~/MyTestimonial/ViewTestimonials\">Testimonials</a></li>";
                    replacestr += "<li><a href=\"~/testimonials\">Testimonials</a></li>";
                    replacestr += "}";
                    replacestr += "</ul>";

                    int i = 0;
                    string[] arry = new string[1000];
                    arry[0] = "@using Nop.Services.Configuration@*TestimonialLink*@";
                    arry[1] = "@{@*TestimonialLink*@";
                    arry[2] = "    var testimonialsettingservice = EngineContext.Current.Resolve<ISettingService>();";
                    arry[3] = "    var testimonialpluginmodel = testimonialsettingservice.GetSettingByKey(\"mytestimonialsettings.showtestimonialpage\", \"null\", 0, false);";
                    arry[4] = "    var testimonialenablepluginmodel = testimonialsettingservice.GetSettingByKey(\"widgetsettings.activewidgetsystemnames\", \"null\", 0, false);";
                    arry[5] = "}@*TestimonialLink*@";
                    i = 6;

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
            return RedirectToAction("ViewTestimonials");
        }
        public ActionResult Removecodefromfiles()
        {
            #region Remove Footer and widget For All Themes.
            DirectoryInfo themefilesdi = new DirectoryInfo(Server.MapPath("~/Themes/"));
            DirectoryInfo mainviewfilesdi = new DirectoryInfo(Server.MapPath("~/Views/"));
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
                            if (line.Contains("widgetsettings.activewidgetsystemnames"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("Testimonials"))
                            {
                                line = line.Replace(line, "</ul>");
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
                            if (line.Contains("widgetsettings.activewidgetsystemnames"))
                            {
                                line = line.Replace(line, "");
                            }
                            if (line.Contains("Testimonials"))
                            {
                                line = line.Replace(line, "</ul>");
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
            return RedirectToAction("ViewTestimonials");
        }

        public ActionResult GenerateTableScript()
        {
            GenerateScript();
            return RedirectToAction("ViewTestimonials");
        }

        public ActionResult InsertOldDataWithNewColumn()
        {
            insertolddatawithnewcolumntable();
            return RedirectToAction("ViewTestimonials");
        }

        #region generatescript
        public void GenerateScript()
        {
            string macadd = GetMAC();

            Microsoft.SqlServer.Management.Smo.Server srv = new Microsoft.SqlServer.Management.Smo.Server();
            // really you would get these from config or elsewhere:
            srv.ConnectionContext.LoginSecure = false;
            srv.ConnectionContext.Login = "sa";
            srv.ConnectionContext.Password = "Nyusoft123";
            srv.ConnectionContext.ServerInstance = "NS1\\SQLEXPRESS2008R2";
            string dbName = "nop34";

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
                    options.Default = true;
                    options.DriAll = false;
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
            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts"));
            var file = System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt";
            if (!System.IO.File.Exists(file))
            {
                System.IO.StreamWriter datas = System.IO.File.CreateText(System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt");
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
        #endregion

        #region Insert Old Data With New Column
        public void insertolddatawithnewcolumntable()
        {
            string macadd = GetMAC();
            var file = System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt";
            if (System.IO.File.Exists(file))
            {
                System.IO.StreamReader datas = new System.IO.StreamReader(file);
                StringBuilder resultScript = new StringBuilder(string.Empty);
                resultScript.AppendLine(datas.ReadToEnd());
                resultScript.AppendLine("ALTER TABLE Testimonials ADD test bit NULL Default 1 with values");
                Database.SetInitializer<TestimonialRecordObjectContext>(null);
                var dbScript = datas.ReadToEnd();
                dbScript += "ALTER TABLE Testimonials ADD test bit NULL Default 1 with values";
                //System.Data.Entity.Database database = new System.Data.Entity.Database();
                //database.ExecuteSqlCommand(dbScript);
                //database.SqlQuery<TestimonialRecord>(dbScript);

                //datas.Write(resultScript.ToString());
                datas.Close();
            }
        }
        #endregion

        #region ExportExcel(not in use)
        public void exporttable()
        {

            //parth
            SqlConnection cnn;
            string connectionstring = null;
            string sql = null;
            string data = null;
            int i = 0;
            int j = 0;

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;


            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            connectionstring = "Data Source=NS1\\SQLEXPRESS2008R2;Initial Catalog=nop34;Integrated Security=True;Pooling=False;User ID = sa;Password=Nyusoft123";
            cnn = new SqlConnection(connectionstring);
            cnn.Open();
            sql = "SELECT * FROM Testimonials";
            SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 1, j + 1] = data;
                }
            }


            xlWorkBook.SaveAs("informations.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);



        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }

        }
        #endregion
    }
}