using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
	public partial class AlienearImageColorMapping : BaseEntity, ILocalizedEntity
	{
		public int ImageId { get; set; }
		public int ProductID { get; set; }
		public int ColorID { get; set; }
		public bool ImageIdividualFlag { get; set; }
		public string ImageLeftEar { get; set; }
		public string ImageRightEar { get; set; }

		//public virtual Product Product { get; set; }
		public virtual AlienearColorMaster AlienearColorMaster { get; set; }
	}
}
