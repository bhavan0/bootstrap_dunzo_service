using Microsoft.EntityFrameworkCore;
using Repository.Entity;

namespace Repository
{
    public class ProductContext: DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options): base(options)
        {

        }

        public virtual DbSet<InventoryData> InventoryData { get; set; }

        public virtual DbSet<Product> Products { get; set; }
    }
}
