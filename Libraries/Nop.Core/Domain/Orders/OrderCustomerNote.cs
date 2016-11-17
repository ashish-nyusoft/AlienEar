using System;

namespace Nop.Core.Domain.OrderCustomerNote
{
    /// <summary>
    /// Represents an order note
    /// </summary>
    public partial class OrderCustomerNote : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        /// 
        public int Id { get; set; }
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the note
        /// </summary>
        public string OrderNote { get; set; }

        /// <summary>
        /// Gets or sets the attached file (download) identifier
        /// </summary>
        public string CustomerNote { get; set; }

       
    }

}
