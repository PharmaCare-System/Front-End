namespace PharmaCare.Models.Inventory
{
    public class InventoryReadVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int PharmacyId { get; set; }
    }
}
