﻿namespace PharmaCare.Models.Inventory
{
    public class PharmacyReadVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MangerPharmacyId { get; set; }
    }
}
