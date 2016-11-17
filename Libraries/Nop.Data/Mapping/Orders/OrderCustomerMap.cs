using Nop.Core.Domain.OrderCustomerNote;
using Nop.Core.Domain.Orders;

namespace Nop.Data.Mapping.Orders
{
    public partial class OrderCustomerMap : NopEntityTypeConfiguration<OrderCustomerNote>
    {
        public OrderCustomerMap()
        {
            this.ToTable("OrderCustomerNote");
            this.HasKey(on => on.Id);
        }
    }
}