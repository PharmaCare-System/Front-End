using System.ComponentModel.DataAnnotations;

namespace PharmaCare.Models.Product
{
    public class ProductReadVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int QuantityInStock { get; set; }
        public String BarCode { get; set; }
        [Display(Name = "Product Image")]
        [DataType(DataType.Upload)]
        public string ImageUrl { get; set; }
        public bool BulkAllowed { get; set; } = false;
        public bool PrescriptionRequired { get; set; } = true;


        public int InventoryId { get; set; }
        public int CategoryId { get; set; }

    }
}
