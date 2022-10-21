using Microsoft.CodeAnalysis.CSharp.Syntax;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Controllers
{
    public static class Utils
    {
        public static SupplierDTO SupplierToDTO(Supplier supplier) =>
            new SupplierDTO
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactTitle,
                Country = supplier.Country,
                TotalProducts = supplier.Products.Count(),
                Products = supplier.Products.Select(x => ProductToDTO(x)).ToList()
            };

       
        public static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                UnitPrice = product.UnitPrice,
            };


        public static Supplier SupplierToDTO(SupplierDTO dto, Supplier supplier) =>
           new Supplier
           {
               SupplierId = dto.SupplierId,
               CompanyName = dto.CompanyName,
               ContactName = dto.ContactTitle,
               Country = dto.Country,
           };

    }
}
