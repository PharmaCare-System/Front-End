namespace PharmaCare.Models.Product
{
    public class ProductWithImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BarCode { get; set; }
        public bool BulkAllowed { get; set; }
        public bool PrescriptionRequired { get; set; }
        public int CategoryId { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile Image { get; set; } 

        public int InventoryId { get; set; }

    }
}
