using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcommerceAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Repositories.Repositories;
using Ecommerce.DAL.DBContext;
using Microsoft.Extensions.Logging;
using Ecommerce.Repositories.DTO;
using NLog.Web;
using NLog;
using Microsoft.AspNetCore.Mvc;
using NLog.Config;

namespace EcommerceAPI.Tests
{
    [TestClass]
    public class CustomerTests
    {

        private readonly ICustomerRepository customerRepository;
        private readonly CustomerDBContext dBContext;
        //private readonly ILogger<CustomerController> _logger;
        //private readonly LogFactory logFactory;
        //private readonly Logger logger;
        public CustomerTests()
        {
            dBContext= new CustomerDBContext();
            this.customerRepository = new SQLCustomerRepository(dBContext);
            //logFactory = new LogFactory();
            //logFactory.Configuration = new XmlLoggingConfiguration(@"D:\\Udemy\\Interview\\EcommerceAPI\\EcommerceAPI\nlog.config", logFactory);
            //logger = logFactory.GetCurrentClassLogger();
            //_logger = new Mock<ILogger<CustomerController>>();

        }

        [TestMethod]
        public async Task GetAllTest()
        {
            var customersDomain = await customerRepository.GetAllAsync();

            var customersDto = new List<CustomerDto>();

            foreach (var customerDomain in customersDomain)
            {
                customersDto.Add(new CustomerDto()
                {
                    Id = customerDomain.Id,
                    FirstName = customerDomain.FirstName,
                    LastName = customerDomain.LastName,
                    City = customerDomain.City,
                    Country = customerDomain.Country,
                    Phone = customerDomain.Phone
                });
            }
            Assert.IsTrue(customersDto.Count>0);
        }

        //[TestMethod]
        //public async Task CreateCustomer()
        //{
        //    var customersDomain = await customerRepository.GetAllAsync();

        //    var customersDto = new List<CustomerDto>();

        //    foreach (var customerDomain in customersDomain)
        //    {
        //        customersDto.Add(new CustomerDto()
        //        {
        //            Id = customerDomain.Id,
        //            FirstName = customerDomain.FirstName,
        //            LastName = customerDomain.LastName,
        //            City = customerDomain.City,
        //            Country = customerDomain.Country,
        //            Phone = customerDomain.Phone
        //        });
        //    }
        //    Assert.IsTrue(customersDto.Count > 0);
        //}

        [TestMethod]
        public async Task GetAllCustomerTest()
        {
            var customerController = new CustomerController(this.customerRepository);
            var actionResult = await customerController.GetAll();
                        
            Assert.IsInstanceOfType(actionResult,Type.GetType("OkObjectResult"));
        }
    }
}