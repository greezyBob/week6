using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Services
{
    public class SupplierService : IServiceSupplier
    {
        private readonly NorthwindContext _context;

        public SupplierService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task CreateSupplierAsync(Supplier item)
        {
            _context.Suppliers.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsBySupplierIdAsync(int id)
        {
            return await _context.Products.Where(p => p.SupplierId == id).ToListAsync();
        }

        public async Task<Supplier?> GetSupplierAsync(long id)
        {
            return await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Include(s => s.Products)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Products)           
                .ToListAsync();
        }

        public async Task RemoveSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSupplierChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool SupplierExists(long id)
        {
            return _context.Suppliers.Any(s => s.SupplierId == id);
        }
    }
}
