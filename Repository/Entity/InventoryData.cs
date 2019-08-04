using Repository.Entity;
using System.Collections.Generic;

namespace Repository
{
    public class InventoryData
    {
        public int Id { get; set; }

        public string ShopName { get; set; }

        public double TotalPrice { get; set; }

        public virtual IList<Product> Products { get; set; }        
    }
}