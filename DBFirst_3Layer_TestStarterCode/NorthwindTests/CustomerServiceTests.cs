using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindData.Services;
using NorthwindData;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using NorthwindBusiness;

namespace NorthwindTests
{
    public class CustomerServiceTests
    {
        private CustomerService _sut;
        private NorthwindContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "Example_DB")
                .Options;
            _context = new NorthwindContext(options);
            _sut = new CustomerService(_context);

            //Seed the database
            _sut.CreateCustomer(new Customer { CustomerId = "PHILL", ContactName = "Philip Windridge", CompanyName = "Sparta Global", City = "Birmingham" });
            _sut.CreateCustomer(new Customer { CustomerId = "MANDA", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "Birmingham" });
        }

        [Test]
        public void GivenValidId_CorrectCustomerIsReturned()
        {
            var result = _sut.GetCustomerById("PHILL");

            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Philip Windridge"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(result.City, Is.EqualTo("Birmingham"));
        }

        [Test]
        public void GivenANewCustomer_CreateCustomerAddsToDataBase()
        {
            var numberOfCustomersBefore = _context.Customers.Count();
            var newCustomer = new Customer
            {
                CustomerId = "ODELL",
                ContactName = "Max Odell",
                CompanyName = "Sparta Global",
                City = "Surrey"
            };
            _sut.CreateCustomer(newCustomer);
            var numberOfCustomersAfter = _context.Customers.Count();
            Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));

            //Clean up
            _context.Customers.Remove(newCustomer);
            _context.SaveChanges();
        }

        [Test]
        public void RemoveCustomer_DecreasesDataBaseByOne()
        {            
            var numberOfCustomersBeforeRemove = _context.Customers.Count();
            var cust = _sut.GetCustomerById("PHILL");
            _sut.RemoveCustomer(cust);
            var numberOfCustomersAfterRemove = _context.Customers.Count();

            Assert.That(numberOfCustomersBeforeRemove - 1, Is.EqualTo(numberOfCustomersAfterRemove));

            //clean up
            _sut.CreateCustomer(new Customer { CustomerId = "PHILL", ContactName = "Philip Windridge", CompanyName = "Sparta Global", City = "Birmingham" });
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            var newCustomer = new Customer
            {
                CustomerId = "ODELL",
                ContactName = "Max Odell",
                CompanyName = "Sparta Global",
                City = "Surrey"
            };
            _sut.CreateCustomer(newCustomer);
            _sut.RemoveCustomer(newCustomer);
            var result = _sut.GetCustomerById("ODELL");

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void CustomerList_ListLengthIsEqualtoDbLength()
        {
            var result = _sut.GetCustomerList().Count();
            var dbLength = _context.Customers.Count();

            Assert.That(result, Is.EqualTo(dbLength));
        }
    }
}
