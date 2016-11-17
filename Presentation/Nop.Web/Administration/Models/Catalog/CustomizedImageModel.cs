using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Models.Customers;
using Nop.Admin.Models.Discounts;
using Nop.Admin.Models.Stores;
using Nop.Admin.Validators.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using Nop.Core.Domain.Catalog;
using System.Web;

namespace Nop.Admin.Models.Catalog
{
    //public partial class CustomizedImageModel : BaseNopEntityModel
    //{
    //    public CustomizedImageModel()
    //    {
    //        ListData = new List<CustomizedImageModel>();
    //        AlienearColorMaster = new CustomizedImageColorModel();
    //    }
    //    public int ImageId { get; set; }
    //    public int ProductID { get; set; }
    //    public int ColorID { get; set; }
    //    public bool ImageIdividualFlag { get; set; }
    //    public string ImageLeftEar { get; set; }
    //    public string ImageRightEar { get; set; }
    //    public HttpPostedFileBase LeftImage { get; set; }
    //    public HttpPostedFileBase RightImage { get; set; }
    //    public List<CustomizedImageModel> ListData { get; set; }
    //    //public virtual Product Product { get; set; }
    //    public CustomizedImageColorModel AlienearColorMaster { get; set; }
    //}
}