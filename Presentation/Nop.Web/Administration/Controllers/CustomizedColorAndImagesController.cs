using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Infrastructure.Cache;
using Nop.Admin.Models.Catalog;
using Nop.Admin.Models.Orders;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Core.Caching;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using Nop.Web.Framework.Security;
namespace Nop.Admin.Controllers
{
    public partial class CustomizedColorAndImagesController : BaseAdminController
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICustomerService _customerService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IPictureService _pictureService;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IProductTagService _productTagService;
        private readonly ICopyProductService _copyProductService;
        private readonly IPdfService _pdfService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IAclService _aclService;
        private readonly IStoreService _storeService;
        private readonly IOrderService _orderService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IVendorService _vendorService;
        private readonly IShippingService _shippingService;
        private readonly IShipmentService _shipmentService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDiscountService _discountService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IDownloadService _downloadService;

        #endregion

        #region Constructors

        public CustomizedColorAndImagesController(IProductService productService,
            IProductTemplateService productTemplateService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICustomerService customerService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ISpecificationAttributeService specificationAttributeService,
            IPictureService pictureService,
            ITaxCategoryService taxCategoryService,
            IProductTagService productTagService,
            ICopyProductService copyProductService,
            IPdfService pdfService,
            IExportManager exportManager,
            IImportManager importManager,
            ICustomerActivityService customerActivityService,
            IPermissionService permissionService,
            IAclService aclService,
            IStoreService storeService,
            IOrderService orderService,
            IStoreMappingService storeMappingService,
             IVendorService vendorService,
            IShippingService shippingService,
            IShipmentService shipmentService,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IMeasureService measureService,
            MeasureSettings measureSettings,
            ICacheManager cacheManager,
            IDateTimeHelper dateTimeHelper,
            IDiscountService discountService,
            IProductAttributeService productAttributeService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IShoppingCartService shoppingCartService,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser,
            IDownloadService downloadService)
        {
            this._productService = productService;
            this._productTemplateService = productTemplateService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._customerService = customerService;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._specificationAttributeService = specificationAttributeService;
            this._pictureService = pictureService;
            this._taxCategoryService = taxCategoryService;
            this._productTagService = productTagService;
            this._copyProductService = copyProductService;
            this._pdfService = pdfService;
            this._exportManager = exportManager;
            this._importManager = importManager;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._aclService = aclService;
            this._storeService = storeService;
            this._orderService = orderService;
            this._storeMappingService = storeMappingService;
            this._vendorService = vendorService;
            this._shippingService = shippingService;
            this._shipmentService = shipmentService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
            this._cacheManager = cacheManager;
            this._dateTimeHelper = dateTimeHelper;
            this._discountService = discountService;
            this._productAttributeService = productAttributeService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._shoppingCartService = shoppingCartService;
            this._productAttributeFormatter = productAttributeFormatter;
            this._productAttributeParser = productAttributeParser;
            this._downloadService = downloadService;
        }

        #endregion

        [HttpPost]
        public ActionResult GetColorMaster(string productId)
        {
            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer(); 
            var data = _productService.GetColors();
            //return jsSerializer.Serialize(data);
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = data.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        [AdminAntiForgery(true)]
        public ActionResult GetColorMasterWithImageOfProduct(int productId)
        {
            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer(); 
            var data = _productService.getPeoductImageAvalable(productId);
            //return jsSerializer.Serialize(data);
            //var gridModel = new DataSourceResult
            //{
            //    Data = data,
            //    Total = data.Count
            //};

            return Json(data);
        }
        [HttpPost]
        [AdminAntiForgery(true)]
        public string CheckColorName(string colorName)
        {
            if (_productService.GetColorCountByColorName(colorName) > 0)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }
        [HttpPost]
        [AdminAntiForgery(true)]
        public string CheckColorCode(string code)
        {
            if (_productService.GetColorCodeCountByColorName(code) > 0)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }
        [HttpPost]
        [AdminAntiForgery(true)]
        public JsonResult SaveColorMaster(string name, string image)
        {
            try
            {
                //string base64 = image.Substring(image.IndexOf(',') + 1);
                //base64 = base64.Trim('\0');
                //byte[] chartData = Convert.FromBase64String(base64);
                //Image img = byteArrayToImage(chartData);
                if (image != null)
                {

                    //if (_productService.GetColorCountByColorName(name) == 0)
                    //{
                    //var data = _productService.GetColors();
                    //Download dl = new Download();
                    //dl.DownloadGuid = Guid.NewGuid();
                    AlienearColorMaster obj = new AlienearColorMaster();
                    //img.Save(Server.MapPath("~/Content/Images/" + (dl.DownloadGuid) + ".png"));
                    //obj.ColorImage = "/Content/Images/" + (dl.DownloadGuid) + ".png";
                    obj.ColorImage = image;
                    obj.ColorName = name;
                    //var coun= 
                    _productService.InsertProductColorDetails(obj);
                    //}
                    //}
                }
                var data = _productService.GetColors();
                //return jsSerializer.Serialize(data);
                var gridModel = new DataSourceResult
                {
                    Data = data,
                    Total = data.Count
                };


                return Json(gridModel);
            }
            catch (Exception)
            {
                return Json("");
            }


        }



        //[HttpPost]
        //[AdminAntiForgery(true)]
        //public bool SaveImages1(CustomizedImageModel obj)
        //{
        //}
        [HttpPost]
        [AdminAntiForgery(true)]
        public bool SaveImages(string productId, string leftImage, string rightImage, string ColorId, string flag)
        {
            string base641 = leftImage.Substring(leftImage.IndexOf(',') + 1);
            base641 = base641.Trim('\0');
            byte[] chartData1 = Convert.FromBase64String(base641);
            Image img1 = byteArrayToImage(chartData1);
            string base642 = rightImage.Substring(rightImage.IndexOf(',') + 1);
            base642 = base642.Trim('\0');
            byte[] chartData2 = Convert.FromBase64String(base642);
            Image img2 = byteArrayToImage(chartData2);
            if (!string.IsNullOrEmpty(leftImage) != null && !string.IsNullOrEmpty(rightImage))
            {

                //if (_productService.GetColorCountByColorName(name) == 0)
                //{
                //var data = _productService.GetColors();
                Download dl = new Download();
                dl.DownloadGuid = Guid.NewGuid();
                AlienearImageColorMapping obj = new AlienearImageColorMapping();
                img1.Save(Server.MapPath("~/Content/Images/" + (dl.DownloadGuid) + ".png"));
                obj.ImageLeftEar = "/Content/Images/" + (dl.DownloadGuid) + ".png";
                img2.Save(Server.MapPath("~/Content/Images/" + (dl.DownloadGuid) + ".png"));
                obj.ImageRightEar = "/Content/Images/" + (dl.DownloadGuid) + ".png";
                if (flag == "true")
                {
                    obj.ImageIdividualFlag = true;
                }
                else
                {
                    obj.ImageIdividualFlag = false;
                }
                obj.ColorID = Convert.ToInt16(ColorId);
                obj.ProductID = Convert.ToInt16(productId);
                //var coun= 
                _productService.InsertProductColorImageDetails(obj);

                //}
                //}
                return true;
            }
            else
            {
                return false;
            }
            //var data = _productService.GetColors();
            ////return jsSerializer.Serialize(data);
            //var gridModel = new DataSourceResult
            //{
            //    Data = data,
            //    Total = data.Count
            //};


        }



        [HttpPost]
        [AdminAntiForgery(true)]
        public string CheckCategory(int ProductId)
        {
            string result = "false";
            try
            {
                var category = _categoryService.GetProductCategoriesByProductId(ProductId);
                foreach (var item in category)
                {
                    if (item.CategoryId == 17)
                    {
                        result = "true";
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        [HttpPost]
        [AdminAntiForgery(true)]
        public void DeleteColor(int colorId)
        {
            try
            {

                List<AlienearImageColorMapping> lst = _productService.GetProductImagesAllByColorIdId(colorId);
                foreach (var item in lst)
                {
                    string[] str = _productService.DeleteProductColorImageDetails(item.ImageId);
                    System.IO.File.Delete(Server.MapPath("~" + str[0]));
                    System.IO.File.Delete(Server.MapPath("~" + str[1]));
                }
                System.IO.File.Delete(Server.MapPath("~" + _productService.DeleteProductColorDetails(colorId)));

            }
            catch (Exception)
            {
            }


        }


        [HttpPost]
        [AdminAntiForgery(true)]
        public void EditColor(string colorId,string ColorName,string ColorCode)
        {
            try
            {
                AlienearColorMaster obj = new AlienearColorMaster();
                obj.ColorId = Convert.ToInt32(colorId);
                obj.ColorName = ColorName;
                obj.ColorImage = ColorCode;
                _productService.EditProductColorDetails(obj);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }



        [HttpPost]
        [AdminAntiForgery(true)]
        public void DeleteColorImage(string id)
        {
            try
            {
                AlienearImageColorMapping obj = _productService.GetProductImagesByProductIdSingle(Convert.ToInt32(id));
                //List<AlienearImageColorMapping> lst = _productService.GetProductImagesAllByColorIdId(colorId);
                //foreach (var item in lst)
                //{
                //string[] str = _productService.DeleteProductColorImageDetails();
                System.IO.File.Delete(Server.MapPath("~" + obj.ImageLeftEar));
                System.IO.File.Delete(Server.MapPath("~" + obj.ImageRightEar));
                _productService.DeleteImagesmapping(obj);
                //}
                //System.IO.File.Delete(Server.MapPath("~" + _productService.DeleteProductColorDetails(colorId)));

            }
            catch (Exception)
            {
            }


        }








    }
}

