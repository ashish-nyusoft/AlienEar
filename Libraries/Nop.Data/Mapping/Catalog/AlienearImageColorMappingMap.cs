using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
	class AlienearImageColorMappingMap : NopEntityTypeConfiguration<AlienearImageColorMapping>
	{
		  public AlienearImageColorMappingMap()
        {
			this.ToTable("AlienearImageColorMapping");
            this.HasKey(pa => pa.ImageId);
            this.Ignore(pa => pa.Id);
            //this.HasRequired(pam => pam.Product)
            //    .WithMany()
            //    .HasForeignKey(pam => pam.ProductID);
			this.HasRequired(pam => pam.AlienearColorMaster)
			 .WithMany()
			 .HasForeignKey(pam => pam.ColorID);
        }
	
	}
}

