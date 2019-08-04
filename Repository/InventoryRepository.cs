using Microsoft.EntityFrameworkCore;
using Repository.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public interface IInventoryRepository
    {
        bool AddProduct(InventoryData data);

        MetaData GetMetaData();
        IList<Product> GetAll();
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly ProductContext _context;
        public InventoryRepository(ProductContext context)
        {
            _context = context;
        }

        public bool AddProduct(InventoryData data)
        {
            _context.Add(data);
            return _context.SaveChanges() > 0;
        }

        public MetaData GetMetaData()
        {
            var inventory = _context.InventoryData.ToList();

            var products = _context.Products.ToList();

            var inventoryCount = inventory.Count();

            double totalPrice = inventory.Sum(item => item.TotalPrice);

            return new MetaData()
            {
                NoOfBills = inventoryCount,
                TotalOrderPrice = totalPrice
            };

        }

        public IList<Product> GetAll()
        {
            return _context.Set<Product>().Include(x=>x.InventoryData).ToList();
        }
    }
}
