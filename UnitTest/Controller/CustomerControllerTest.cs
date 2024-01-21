using APICatalogo.Repository;
using AutoMapper;
using DocumentNumber.Portugal.Nif.Generator;
using DocumentNumber.ValidatorAbstractions;
using EmanuelCegidTest.Controllers;
using EmanuelCegidTest.DTOs;
using EmanuelCegidTest.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portugal.Nif.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Controller
{
    public class CustomerControllerTest
    {
        private readonly IUnitOfWork _context;
        private readonly INifValidator _NifValidator;
        private readonly IMapper _Mapper;
        public CustomerControllerTest()
        {
            _context = A.Fake<IUnitOfWork>();
            _NifValidator = A.Fake<INifValidator>();
            _Mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void CustomerController_GetCustomers_ReturnOK()
        {
            //Arrenge
            var Customers = A.Fake<ICollection<CustomersDTO>>();
            var CustomersList = A.Fake<List<CustomersDTO>>();
            A.CallTo(() => _Mapper.Map<List<CustomersDTO>>(Customers)).Returns(CustomersList);
            var Controller = new CustomerController (_context, _NifValidator, _Mapper);
            
            //Act
            var result = Controller.GetCustomers();
            
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult<IEnumerable<CustomersDTO>>>>();
        }

        [Fact]
        public void CustomerController_GetCustomer_ReturnOK()
        {
            //Arrenge
            int id = 2;
            var Customer = A.Fake<Customers>();
            var CustomerDTO = A.Fake<CustomersDTO>();
            A.CallTo(() => _Mapper.Map<CustomersDTO>(Customer)).Returns(CustomerDTO);
            A.CallTo(() => _context.CostomerRepository.GetById(C => C.ID == id)).Returns(Customer);
            var Controller = new CustomerController(_context, _NifValidator, _Mapper);

            //Act
            var result = Controller.GetCustomer(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult<CustomersDTO>>>();
        }

        [Fact]
        public void CustomerController_SaveCustomer_ReturnOK()
        {
            //Arrenge
            CustomersDTO CustomerDTO = new CustomersDTO
            {
                ID = 1,
                Country = "PORTUGAL",
                Name = "Test",
                TaxID = "788179365"
            };

            A.CallTo(() => _NifValidator.Validate(CustomerDTO.TaxID)).Returns(true);
            var Customer = A.Fake<Customers>();
            A.CallTo(() => _Mapper.Map<Customers>(CustomerDTO)).Returns(Customer);
            A.CallTo(() => _context.CostomerRepository.Add(Customer));
            var Controller = new CustomerController(_context, _NifValidator, _Mapper);

            //Act
            var result = Controller.SaveCustomer(CustomerDTO);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult>>();
        }

        [Fact]
        public async void CustomerController_SaveCustomer_ReturnBadRequest_WhenNifIsNotValid()
        {
            //Arrenge
            CustomersDTO CustomerDTO = new CustomersDTO
            {
                ID = 1,
                Country = "PORTUGAL",
                Name = "Test",
                TaxID = "258008596"
            };

            A.CallTo(() => _NifValidator.Validate(CustomerDTO.TaxID)).Returns(false);
            var Customer = A.Fake<Customers>();
            A.CallTo(() => _Mapper.Map<Customers>(CustomerDTO)).Returns(Customer);
            A.CallTo(() => _context.CostomerRepository.Add(Customer));
            var Controller = new CustomerController(_context, _NifValidator, _Mapper);

            //Act
            var result = Controller.SaveCustomer(CustomerDTO);

            //Assert
            result.Should().NotBeNull();
            var actionResult = await result;
            actionResult.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = (BadRequestObjectResult)actionResult;
            badRequestResult.Value.Should().Be("Invalid Nif.");
        }

        [Fact]
        public void CustomerController_UpdateCustomer_ReturnOK()
        {
            int id = 01;
            //Arrenge
            CustomersDTO CustomerDTO = new CustomersDTO
            {
                ID = 1,
                Country = "PORTUGAL",
                Name = "Test",
                TaxID = "788179365"
            };

            var Customer = A.Fake<Customers>();

            A.CallTo(() => _context.CostomerRepository.GetById(C => C.ID == id));
            A.CallTo(() => _NifValidator.Validate(CustomerDTO.TaxID)).Returns(true);
            A.CallTo(() => _Mapper.Map<Customers>(CustomerDTO)).Returns(Customer);
            A.CallTo(() => _context.CostomerRepository.Update(Customer));
            var Controller = new CustomerController(_context, _NifValidator, _Mapper);

            //Act
            var result = Controller.UpdateCustomer(id, CustomerDTO);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult>>();
        }

        [Fact]
        public async void CustomerController_UpdateCustomer_ReturnsBadRequest_WhenNifIsNotValid()
        {
            // Arrange
            int id = 1;
            CustomersDTO customerDTO = new CustomersDTO
            {
                ID = 1,
                Country = "PORTUGAL",
                Name = "Test",
                TaxID = "258008596"
            };

            var Customer = A.Fake<Customers>();
            A.CallTo(() => _context.CostomerRepository.GetById(A<Expression<Func<Customers, bool>>>.Ignored)).Returns(Customer);  // Configurando o mock para retornar null
            A.CallTo(() => _NifValidator.Validate(customerDTO.TaxID)).Returns(false);
            var controller = new CustomerController(_context, _NifValidator, _Mapper);

            // Act
            var result = controller.UpdateCustomer(id, customerDTO);

            // Assert
            result.Should().NotBeNull();
            var actionResult = await result;
            actionResult.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = (BadRequestObjectResult)actionResult;
            badRequestResult.Value.Should().Be("Invalid Nif.");
        }


        [Fact]
        public void CustomerController_DeleteCustomer_ReturnsOk()
        {
            // Arrange
            var Customer = A.Fake<Customers>();
            var CustomerDTO = A.Fake<CustomersDTO>();
            A.CallTo(() => _context.CostomerRepository.GetById(C => C.ID == Customer.ID)).Returns(Customer);
            A.CallTo(() => _Mapper.Map<CustomersDTO>(Customer)).Returns(CustomerDTO);
            A.CallTo(() => _context.CostomerRepository.Delete(Customer));
            var controller = new CustomerController(_context, _NifValidator, _Mapper);

            // Act
            var result = controller.DeleteCustomer(Customer.ID);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult>>();
        }


        [Fact]
        public async void CustomerController_DeleteCustomer_ReturnsNotFound_WhenCustomerNotFound()
        {
            // Arrange
            int id = 1;
            A.CallTo(() => _context.CostomerRepository.GetById(A<Expression<Func<Customers, bool>>>.Ignored)).Returns(Task.FromResult<Customers>(null));
            var controller = new CustomerController(_context, _NifValidator, _Mapper);

            // Act
            var result = controller.DeleteCustomer(id);

            // Assert
            result.Should().NotBeNull();
            var actionResult = await result;
            actionResult.Should().BeOfType<NotFoundObjectResult>();  

            var badRequestResult = (NotFoundObjectResult)actionResult;  
            badRequestResult.Value.Should().Be("Customer not found.");
        }
    }
}
