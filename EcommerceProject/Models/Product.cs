namespace EcommerceProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

