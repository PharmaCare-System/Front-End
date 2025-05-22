using PharmaCare.Models.Product;

namespace PharmaCare.Models.Categories
{
    public class CategoryProducts
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<ProductReadVM> Products { get; set; }
    }
}
