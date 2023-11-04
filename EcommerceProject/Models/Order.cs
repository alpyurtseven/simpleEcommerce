using System;
namespace EcommerceProject.Models
{
	public class Order
	{
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual Customer Customer { get; set; }
        public bool Status { get; set; }

        public decimal ComputeTotalValue()
        {
            decimal total = 0;

            foreach (var item in this.OrderItems)
            {
                total+= item.Product.Price * item.Quantity;
            }

            return total;
        }
    }
}

