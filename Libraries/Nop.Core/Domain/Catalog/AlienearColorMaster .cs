using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
	public partial class AlienearColorMaster : BaseEntity, ILocalizedEntity
	{
		public int ColorId { get; set; }
		public string ColorName { get; set; }
		public string ColorImage { get; set; }
	}
}
