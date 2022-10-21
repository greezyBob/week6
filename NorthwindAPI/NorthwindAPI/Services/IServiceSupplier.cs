using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Services
{
    public interface IServiceSupplier
    {
        public Task<List<Supplier>> GetSuppliersAsync();
        public Task<Supplier> GetSupplierAsync(long id);
        public Task CreateSupplierAsync(Supplier item);
        public Task SaveSupplierChangesAsync();
        public Task RemoveSupplierAsync(Supplier item);
        public bool SupplierExists(long id);
        public Task<List<Product>> GetProductsBySupplierIdAsync(int id);
    }
}
