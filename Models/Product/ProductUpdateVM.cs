using System.ComponentModel.DataAnnotations;

namespace PharmaCare.Models.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }  // Important for editing

        [Required]
        public string Name { get; set; }


        public float Price { get; set; }

    
        public int QuantityInStock { get; set; }

        public bool BulkAllowed { get; set; } = false;
        public bool PrescriptionRequired { get; set; } = true;

        public string ImageUrl { get; set; }  // To keep track of current image

        public int InventoryId { get; set; }
        public int CategoryId { get; set; }  // Important for editing
    }
}
