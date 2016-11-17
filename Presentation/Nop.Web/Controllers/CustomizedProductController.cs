using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;
using System.Web.Script.Serialization;
using Nop.Core.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Nop.Web.Controllers
{
    public partial class CustomizedProductController : BasePublicController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IShoppingCartService _ShoppingCartService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IMeasureService _measureService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IWebHelper _webHelper;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
        private readonly ICompareProductsService _compareProductsService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IProductTagService _productTagService;
        private readonly IOrderReportService _orderReportService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly IDownloadService _downloadService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IShippingService _shippingService;
        private readonly IEventPublisher _eventPublisher;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly SeoSettings _seoSettings;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructors

        public CustomizedProductController(ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService,
            IVendorService vendorService,
            IProductTemplateService productTemplateService,
            IProductAttributeService productAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ITaxService taxService,
            ICurrencyService currencyService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IMeasureService measureService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IWebHelper webHelper,
            ISpecificationAttributeService specificationAttributeService,
            IDateTimeHelper dateTimeHelper,
            IRecentlyViewedProductsService recentlyViewedProductsService,
            ICompareProductsService compareProductsService,
            IWorkflowMessageService workflowMessageService,
            IProductTagService productTagService,
            IOrderReportService orderReportService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService,
            IDownloadService downloadService,
            ICustomerActivityService customerActivityService,
            IProductAttributeParser productAttributeParser,
            IShippingService shippingService,
            IEventPublisher eventPublisher,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            VendorSettings vendorSettings,
            ShoppingCartSettings shoppingCartSettings,
            LocalizationSettings localizationSettings,
            CustomerSettings customerSettings,
            CaptchaSettings captchaSettings,
            SeoSettings seoSettings,
            ICacheManager cacheManager, IShoppingCartService ShoppingCartService)
        {
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._vendorService = vendorService;
            this._productTemplateService = productTemplateService;
            this._productAttributeService = productAttributeService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._measureService = measureService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._webHelper = webHelper;
            this._specificationAttributeService = specificationAttributeService;
            this._dateTimeHelper = dateTimeHelper;
            this._recentlyViewedProductsService = recentlyViewedProductsService;
            this._compareProductsService = compareProductsService;
            this._workflowMessageService = workflowMessageService;
            this._productTagService = productTagService;
            this._orderReportService = orderReportService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._downloadService = downloadService;
            this._customerActivityService = customerActivityService;
            this._productAttributeParser = productAttributeParser;
            this._shippingService = shippingService;
            this._eventPublisher = eventPublisher;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._vendorSettings = vendorSettings;
            this._shoppingCartSettings = shoppingCartSettings;
            this._localizationSettings = localizationSettings;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;
            this._seoSettings = seoSettings;
            this._cacheManager = cacheManager;
            this._ShoppingCartService = ShoppingCartService;
        }

        #endregion

        [HttpPost]
        public string GetColors(int productID)
        {
            //string left, right;
            List<temp> lst = new List<temp>();
            try
            {
                // var  


                List<AlienearImageColorMapping> lst1 = _productService.GetProductImagesByProductId(productID).OrderBy(x=>x.ColorID).ToList();
                int count = 0;
                foreach (var item in lst1)
                {
                    temp obj = new temp();
                    obj.Numbers = count;
                    count++;
                    obj.left = item.ImageLeftEar;
                    obj.right = item.ImageRightEar;
                    obj.main = _productService.GetColorByColorId(item.ColorID);
                    obj.colorName = _productService.GetColorNameByColorId(item.ColorID);
                    lst.Add(obj);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lst);
                return output;
            }
            catch (Exception ex)
            {

                return "";
            }

        }

        public string GetColorsAll(int productID)
        {
            //string left, right;
            List<temp> lst = new List<temp>();
            try
            {
                // var  


                List<AlienearImageColorMapping> lst1 = _productService.GetProductImagesAllByProductId(productID);
                foreach (var item in lst1)
                {
                    temp obj = new temp();
                    obj.left = item.ImageLeftEar;
                    obj.right = item.ImageRightEar;
                    obj.main = _productService.GetColorByColorId(item.ColorID);
                    lst.Add(obj);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lst);
                return output;
            }
            catch (Exception ex)
            {

                return "";
            }

        }

        public System.Drawing.Bitmap CombineBitmap(List<Image> images1)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<Bitmap>();//images1;
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (Bitmap image in images1)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    width += image.Width;
                    height = image.Height > height ? image.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Transparent);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
        [HttpPost]
        
        public string Insert(string image, string image1, int id, string customeColor)
        {
            try
            {
                var Customer = EngineContext.Current.Resolve<IWorkContext>().CurrentCustomer.Id;
                List<Bitmap> images = new List<Bitmap>();
                List<Image> images1 = new List<Image>();
                //Bitmap finalImage = new Bitmap(684, 648);
                Image img1 = null;
                img1 = DownloadImage(image);
                images1.Add(img1);
                Image img2 = null;
                img2 = DownloadImage(image1);
                images1.Add(img2);


                Bitmap finalImage = CombineBitmap(images1);
                //finalImage.Save(Server.MapPath("~/Content/Images/testing.jpg"));
                //using (Bitmap b = new Bitmap(684, 324))
                //using (Graphics g = Graphics.FromImage(b))
                //{
                //    //g.CompositingMode = CompositingMode.SourceCopy;
                //    g.Clear(Color.Transparent);
                //    g.DrawImage(img1, 0,324);
                //    g.DrawImage(img2, img1.Width + 1, 324);

                //    b.Save(Server.MapPath("~/Content/Images/testing.jpg"));
                //}


                //using (Graphics g = Graphics.FromImage(finalImage))
                //{
                //    g.Clear(Color.White);
                //    //g.Clear(Color.Transparent);
                //    int offset = 0;
                //    g.DrawImage(images1[0], new Point(0, 684));
                //    g.DrawImage(images1[1], new Point(343, 684));

                //}
                System.Threading.Thread.Sleep(1000);

                ShoppingCartItem Shopping = _ShoppingCartService.GetEntryID(_ShoppingCartService.GetLastEntryID(Customer));
                Download dl = _downloadService.GetDownloadLst();
                Download dl1 = new Download();
                byte[] file1 = null;
                //CustomizedPetPatioDetails obj = new Core.Domain.Catalog.CustomizedPetPatioDetails();

                var product = _productService.GetProductById(id);
                int attributeID = 0;
                int cutomeid = 0;
                foreach (var attribures in product.ProductAttributeMappings)
                {
                    if (attribures.AttributeControlTypeId == 30)
                    {
                        attributeID = attribures.Id;
                    }
                    else if (attribures.AttributeControlTypeId == 4)
                    {
                        //var abc = _productAttributeService.GetProductAttributeById(attribures.Id);
                        if (_productAttributeService.GetProductAttributeNameById(attribures.ProductAttribute.Id).Name == "CustomColor")
                        {
                            cutomeid = attribures.Id;
                        }
                    }
                }



                if (!string.IsNullOrEmpty(Shopping.AttributesXml))
                {
                    string MainAttributeXml = Shopping.AttributesXml;
                    //string[] part1 = MainAttributeXml.Split(new string[] { "xx" }, StringSplitOptions.None); 
                    //string[] part2 = part1.Split(new string[] { "xx" }, StringSplitOptions.None);
                    //string[] part3 = part2.Split(new string[] { "xx" }, StringSplitOptions.None); 
                    if (MainAttributeXml.Contains(dl.DownloadGuid.ToString()))
                    {
                        finalImage.Save(Server.MapPath("~/Content/Images/" + dl.DownloadGuid + ".jpg"));
                        images.Clear();
                        file1 = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Images/" + dl.DownloadGuid + ".jpg"));
                        //obj.ProductId = _ShoppingCartService.GetLastEntryProductID(Customer);
                        //obj.CustomizedPetPatio = file1;
                    }
                    else
                    {
                        dl1.DownloadGuid = Guid.NewGuid();
                        finalImage.Save(Server.MapPath("~/Content/Images/" + dl1.DownloadGuid + ".jpg"));
                        images.Clear();
                        file1 = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Images/" + dl1.DownloadGuid + ".jpg"));
                    }
                }
                else
                {
                    dl1.DownloadGuid = Guid.NewGuid();
                    finalImage.Save(Server.MapPath("~/Content/Images/" + dl1.DownloadGuid + ".jpg"));
                    images.Clear();
                    file1 = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Images/" + dl1.DownloadGuid + ".jpg"));
                }
                //CustomizedPetPatioDetailsService obj12 = new CustomizedPetPatioDetailsService(_CustomizedPetPatioDetailsRepository, _eventPublisher);
                //Product objp = new Core.Domain.Catalog.Product();
                string resizeImageName1 = "";
                //dl.DownloadGuid = Guid.NewGuid();
                if (!string.IsNullOrEmpty(Shopping.AttributesXml))
                {
                    string MainAttributeXml = Shopping.AttributesXml;
                    //string[] part1 = MainAttributeXml.Split(new string[] { "xx" }, StringSplitOptions.None); 
                    //string[] part2 = part1.Split(new string[] { "xx" }, StringSplitOptions.None);
                    //string[] part3 = part2.Split(new string[] { "xx" }, StringSplitOptions.None); 
                    if (MainAttributeXml.Contains(dl.DownloadGuid.ToString()))
                    {
                        if (customeColor == "NA")
                        {
                            Shopping.AttributesXml = "<Attributes><ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute></Attributes>";
                        }
                        else
                        {
                            Shopping.AttributesXml = "<Attributes><ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute><ProductAttribute ID='" + cutomeid + "'><ProductAttributeValue><Value>" + customeColor + "</Value></ProductAttributeValue></ProductAttribute></Attributes>";
                        }
                        _ShoppingCartService.updateShoppingItems(Shopping);
                        dl.DownloadBinary = file1;
                        dl.Filename = "Customizable Product";
                        dl.ContentType = "image/jpeg";
                        dl.IsNew = true;
                        dl.Extension = ".jpg";
                        dl.UseDownloadUrl = false;
                        //_downloadService.UpdateDownload(dl);
                        _downloadService.UpdateDownload(dl);
                        resizeImageName1 = dl.DownloadGuid.ToString();
                    }
                    else
                    {
                        //dl1.DownloadGuid = Guid.NewGuid();
                        dl1.Filename = "Customizable Product";
                        dl1.ContentType = "image/jpeg";
                        dl1.IsNew = true;
                        dl1.Extension = ".jpg";
                        dl1.UseDownloadUrl = false;
                        //Shopping.AttributesXml = "<Attributes><ProductAttribute ID='40'><ProductAttributeValue><Value>" + dl1.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute></Attributes>";
                        string attributesCustome = Shopping.AttributesXml;
                        if (customeColor == "NA")
                        {
                            Shopping.AttributesXml = attributesCustome.Replace("</Attributes>", "<ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl1.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute></Attributes>");
                        }
                        else
                        {
                            Shopping.AttributesXml = attributesCustome.Replace("</Attributes>", "<ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl1.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute><ProductAttribute ID='" + cutomeid + "'><ProductAttributeValue><Value>" + customeColor + "</Value></ProductAttributeValue></ProductAttribute></Attributes>");
                        }
                        _ShoppingCartService.updateShoppingItems(Shopping);
                        dl1.DownloadBinary = file1;
                        //_downloadService.UpdateDownload(dl);
                        _downloadService.InsertDownload(dl1);
                        resizeImageName1 = dl1.DownloadGuid.ToString();
                    }
                }
                else
                {
                    //dl1.DownloadGuid = Guid.NewGuid();
                    dl1.Filename = "Customizable Product";
                    dl1.ContentType = "image/jpeg";
                    dl1.IsNew = true;
                    dl1.Extension = ".jpg";
                    dl1.UseDownloadUrl = false;
                    if (customeColor == "NA")
                    {
                        Shopping.AttributesXml = "<Attributes><ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl1.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute></Attributes>";
                    }
                    else
                    {
                        Shopping.AttributesXml = "<Attributes><ProductAttribute ID='" + attributeID + "'><ProductAttributeValue><Value>" + dl1.DownloadGuid + "</Value></ProductAttributeValue></ProductAttribute><ProductAttribute ID='" + cutomeid + "'><ProductAttributeValue><Value>" + customeColor + "</Value></ProductAttributeValue></ProductAttribute></Attributes>";
                    }
                    _ShoppingCartService.updateShoppingItems(Shopping);
                    dl1.DownloadBinary = file1;
                    //_downloadService.UpdateDownload(dl);
                    _downloadService.InsertDownload(dl1);
                    resizeImageName1 = dl1.DownloadGuid.ToString();
                }
                _pictureService.InsertPicture(file1, "image/jpg", resizeImageName1);

                //_CustomizedPetPatioDetailsService.Insert(obj);
                //int PictureId = _pictureService.GetPicturIdByBinary();
                //string reziseImagePath = _pictureService.GetPictureUrl(PictureId, 80);
                Image resizeImage = FixedSize(finalImage, 250, 143);
                resizeImage.Save(Server.MapPath("~/Content/Images/Thumbs/" + resizeImageName1 + ".jpg"));
                return "";
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        public static Image DownloadImage(string _URL)
        {
            Image _tmpImage = null;

            try
            {
                // Open a connection
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

                _HttpWebRequest.AllowWriteStreamBuffering = true;

                // set timeout for 20 seconds (Optional)
                _HttpWebRequest.Timeout = 20000;

                // Request response:
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

                // Open data stream:
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

                // convert webstream to image
                _tmpImage = Image.FromStream(_WebStream);

                // Cleanup
                _WebResponse.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                return null;
            }

            return _tmpImage;
        }
    }
    public class temp
    {
        public string left { get; set; }
        public string right { get; set; }
        public string main { get; set; }
        public string colorName { get; set; }
        public int Numbers{ get; set; }
    }
}
