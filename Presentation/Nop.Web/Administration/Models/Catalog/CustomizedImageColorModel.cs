﻿using System.Collections.Generic;
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

namespace Nop.Admin.Models.Catalog
{
    public partial class CustomizedImageColorModel : BaseNopEntityModel
    {
        public CustomizedImageColorModel()
        {

        }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
    }
}