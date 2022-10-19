using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _sut;

        [Ignore("should fail")]
        [Test]
        public void BeAbleToConstructCustomerManager()
        {
            _sut = new CustomerManager(null);
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        //Dummy
        [Test]
        public void BeAbleToConstruct_UsingMoq()
        {
            var mockObj = new Mock<ICustomerService>();
            _sut = new CustomerManager(mockObj.Object);

            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        //Stub
        [Category("Happy Path")]
        [Test]
        public void ReturnTrue_WhenUpdateIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA"
            };
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCustomer);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.That(result);
        }

        [Category("Sad Path")]
        [Test]
        public void ReturnFalse_WhenUpdateIsCalled_WithinValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result, Is.False);
        }

        [Category("Sad Path")]
        [Test]
        public void ReturnFalse_WhenUpdateIsCalled_AndExceptionIsthrown()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(new Customer());
            mockObject.Setup(cs => cs.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.That(result, Is.False);
        }

        [Category("Sad Path")]
        [Test]
        public void ReturnFalse_WhenDeleteIsCalled_OnNullCustomer()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Delete(It.IsAny<string>());
            Assert.That(result, Is.False);
        }

        [Category("Happy Path")]
        [Test]
        public void ReturnTrue_WhenDeleteIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA"
            };
            mockObject.Setup(cs => cs.GetCustomerById(originalCustomer.CustomerId)).Returns(originalCustomer);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Delete(originalCustomer.CustomerId);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Delete_SetsSelectedCustomerToNull()
        {
            var mockObject = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA"
            };
            mockObject.Setup(cs => cs.GetCustomerById(originalCustomer.CustomerId)).Returns(originalCustomer);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            _sut.Delete(originalCustomer.CustomerId);

            Assert.That(_sut.SelectedCustomer, Is.Null);
        }

        //Spy
        [Test]
        public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            _sut = new CustomerManager(mockObject.Object);
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            mockObject.Verify(cs => cs.SaveCustomerChanges(), Times.Once);
            mockObject.Verify(cs => cs.SaveCustomerChanges(), Times.Exactly(1));
            mockObject.Verify(cs => cs.GetCustomerList(), Times.Never);
        }

        [Test]
        public void CallRemoveCustomer_WhenDeleteIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            var myCustomer = new Customer();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(myCustomer);
            _sut = new CustomerManager(mockObject.Object);
            _sut.Delete(It.IsAny<string>());

            //Assert
            mockObject.Verify(cs => cs.RemoveCustomer(myCustomer), Times.Once);
        }

        [Test]
        public void CallCreateCustomer_WhenCreateIsCalled_WithValidCustomer()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.CreateCustomer(It.IsAny<Customer>()));
            _sut = new CustomerManager(mockObject.Object);
            _sut.Create("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            mockObject.Verify(cs => cs.CreateCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public void SetSelectedCustomer_IsCustomer()
        {
            var mockObject = new Mock<ICustomerService>();
            _sut = new CustomerManager(mockObject.Object);
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA"
            };
            _sut.SetSelectedCustomer(originalCustomer);
            var result = _sut.SelectedCustomer;

            Assert.That(result.CustomerId, Is.EqualTo(originalCustomer.CustomerId));
        }

        [Test]
        public void RetrieveAll_ReturnsCustomerList()
        {
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerList()).Returns(new List<Customer>());
            _sut = new CustomerManager(mockObject.Object);
            
            var result = _sut.RetrieveAll(); ;

            Assert.That(result, Is.TypeOf<List<Customer>>());
        }
    }
}
