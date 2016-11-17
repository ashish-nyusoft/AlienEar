using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
  public partial class AlienearColorMasterMap : NopEntityTypeConfiguration<AlienearColorMaster>
	{
	  public AlienearColorMasterMap()
        {
			this.ToTable("AlienearColorMaster");
            this.Ignore(pa => pa.Id);
            this.HasKey(pa => pa.ColorId);
            this.Property(pa => pa.ColorName).IsRequired();
			this.Property(pa => pa.ColorImage).IsRequired();
        }
	}
}
