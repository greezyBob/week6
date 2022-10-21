using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Services;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IServiceSupplier _service;

        public SuppliersController(IServiceSupplier service)
        {
            _service = service;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            var suppliers = await _service.GetSuppliersAsync();
            var suppliersDto = suppliers.Select(s => Utils.SupplierToDTO(s)).ToList();
            return suppliersDto;
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsForSupplier(int id)
        {
            if (!SupplierExists(id))
            {
                return NotFound();
            }

            var products = await _service.GetProductsBySupplierIdAsync(id);
            return products.Select(p => Utils.ProductToDTO(p)).ToList();           
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        //returns supplier as JSON or responsecode
        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
            var supplier = await _service.GetSupplierAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Utils.SupplierToDTO(supplier);
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierDTO supplierDto)
        {
            if (id != supplierDto.SupplierId)
            {
                return BadRequest();
            }

            //_context.Entry(supplier).State = EntityState.Modified;
            var supplierToUpdate = await _service.GetSupplierAsync(id);
            supplierToUpdate.CompanyName = supplierDto.CompanyName ?? supplierToUpdate.CompanyName;
            supplierToUpdate.ContactName = supplierDto.ContactName ?? supplierToUpdate.ContactName;
            supplierToUpdate.ContactTitle = supplierDto.ContactTitle ?? supplierToUpdate.ContactTitle;
            supplierToUpdate.Country = supplierDto.Country ?? supplierToUpdate.Country;

            try
            {
                await _service.SaveSupplierChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            await _service.CreateSupplierAsync(supplier);                 
            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierId }, supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplierToDelete = await _service.GetSupplierAsync(id);

            if (SupplierExists(id))
            {
                return NotFound();
            }

            await _service.RemoveSupplierAsync(supplierToDelete);

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _service.SupplierExists(id);
        }

    }
}
