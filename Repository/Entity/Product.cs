namespace Repository.Entity
{
    public class Product
    {
        public int Id { get; set; }

        public int InventoryDataId { get; set; }

        public string ProductName { get; set; }

        public double ProductPrice { get; set; }
        public virtual InventoryData InventoryData { get; set; }
    }
}
