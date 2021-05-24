using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MMTApi.Infrastructure;
using MMTApi.Services;
using MMTModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMTApi.Controllers
{

    public class DeliveryController : BaseController
    {
        private readonly ILogger<DeliveryController> _logger;
        private readonly IConfiguration configuration;
        private readonly CustomerService customerService;
        private readonly OrderService orderService;
        private readonly OrderItemsService orderItemsService;

        public DeliveryController(ILogger<DeliveryController> logger,
            IConfiguration configuration, CustomerService customerService, OrderService orderService,
            OrderItemsService orderItemsService)
        {
            //Request can be logged for production failures debugging
            _logger = logger;
            this.configuration = configuration;
            this.customerService = customerService;
            this.orderService = orderService;
            this.orderItemsService = orderItemsService;
        }

        [HttpPost("Delivery")]
        public async Task<ActionResult<OrderDeliveryDTO>> PostAsync([FromBody] CustomerOrderRequestDTO customerDTO)
        {
            try
            {
                using (var ctx = new DataContext())
                {
                    _logger.LogInformation("Customer order delivery requested");
                        //get customer
                    Customer customer = await customerService.GetCustomerAsync(customerDTO);
                    //Where the email address supplied is not a valid customer in the system, the API will return a 404 status
                    if (customer == null) return NotFound(customerDTO);
                    //order should display the latest order item
                    var order = await orderService.GetOrdersAsync(customer.customerId);
                    //customer has no order return their name and a succeful report
                    var customerFullName = new CustomerOrderResponseDTO() { FirstName = customer.FirstName, LastName = customer.LastName };
                    if (order == null) return Ok(new OrderDeliveryDTO()
                    {
                        Customer = customerFullName,
                        Order = null
                    });
                    //get order items can  be fetched from an OrderItemsService class
                    var orderItems = await orderItemsService.GetOrderItemsAsync(order.OrderID);
                    
                    var oderDeliverDto = new OrderDeliveryDTO()
                    {
                        //Customer property can use AutoMapper to mapp properties
                        Customer = customerFullName,
                        Order = new OrdersDTO()
                        {
                            //customer address can be an override of the ToString() methodin in the customer class
                            DeliveryAddress = $"{customer.FirstName} {customer.LastName} {customer.HouseNumber} {customer.Street} {customer.Postcode} {customer.Town}",
                            DeliveryExpected = order.DeliveryExpected,
                            OrderDate = order.OrderDate,
                            OrderItems = orderItems,
                            OrderNumber = order.OrderID
                        }

                    };
                    return Ok(oderDeliverDto);
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("Failed loading customer order delivery");
                return StatusCode(500);
            }
        }


    }
}
