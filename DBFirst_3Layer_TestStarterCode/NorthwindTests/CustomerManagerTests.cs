using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Linq;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheNumberOfCustomersIncreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Create("MANDA", "Nish", "Sparta Global");
                var numberOfCustomersAfter = db.Customers.Count();

                Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish", "Sparta Global");
                var selectedCustomer = db.Customers.Find("MANDA");
                Assert.That(selectedCustomer.ContactName, Is.EqualTo("Nish"));
                Assert.That(selectedCustomer.CompanyName, Is.EqualTo("Sparta Global"));
            }
        }

        [Test]
        public void WhenACustomersDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish", "Sparta Global");               
                _customerManager.Update("MANDA", "Bob", "Germany", "Berlin", "n25");

                var selectedCustomer = db.Customers.Find("MANDA");

                Assert.That(selectedCustomer.ContactName, Is.EqualTo("Bob"));
                Assert.That(selectedCustomer.Country, Is.EqualTo("Germany"));
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish", "Sparta Global");
                _customerManager.SetSelectedCustomer(db.Customers.Find("MANDA"));
                _customerManager.Update("MANDA", "Bob", "Germany", "Berlin", "n25");

                Assert.That(_customerManager.SelectedCustomer.Country, Is.EqualTo("Germany"));
            }
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            using (var db = new NorthwindContext())
            {
                var result = _customerManager.Update("MANDA", "Bob", "Germany", "Berlin", "n25");

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                
                _customerManager.Create("MANDA", "Nish", "Sparta Global");
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Delete("MANDA");
                var numberOfCustomersAfter = db.Customers.Count();

                Assert.That(numberOfCustomersBefore - 1, Is.EqualTo(numberOfCustomersAfter));
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            using (var db = new NorthwindContext())
            {

                _customerManager.Create("MANDA", "Nish", "Sparta Global");
                _customerManager.Delete("MANDA");
                var result = db.Customers.Find("MANDA");

                Assert.That(result, Is.EqualTo(null));
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}