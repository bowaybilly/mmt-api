using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MMTApi.Infrastructure;
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

        public DeliveryController(ILogger<DeliveryController> logger,IConfiguration configuration)
        {
            //Request can be logged for production failures debugging
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpPost("Delivery")]
        public async Task<ActionResult<OrderDeliveryDTO>> PostAsync([FromBody] CustomerRequestDTO customerDTO)
        {
            try
            {
                using (var ctx = new DataContext())
                {
                    //read token from configuration file 
                    var token = configuration.GetSection("ApiToken");
                    // form url for getting customer information
                    string customerUrl = $"{token}&email={customerDTO.Email}";
                    //HttpClient can be injected in as a service via  the services.AddHttpClient() in startup configureServices method
                    HttpClient httpClient = new HttpClient();
                    Customer customer = default(Customer);
                    var response = await httpClient.GetAsync(customerUrl);
                    //Where the email address supplied is not a valid customer in the system, the API will return a 404 status
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) return NotFound(customerDTO);

                    customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                    //order should display the latest order item
                    var order = ctx.Orders.Where(x => x.CustomerID == customer.customerId).OrderByDescending(x => x.OrderDate).FirstOrDefault();
                    //customer has no order return their name and a succeful report
                    if (order == null) return Ok(new OrderDeliveryDTO()
                    {
                        Customer = new CustomerResponseDTO() { FirstName = customer.FirstName, LastName = customer.LastName },
                        Order = null
                    });
                    //get order items can  be fetched from an OrderItemsService class
                    var orderItems = ctx.OrderItems.AsEnumerable().Where(x => x.OrderID == order.OrderID).Select(x => new OrderitemDTO()
                    {
                        PriceEach = x.Price,
                        Quantity = x.Quantity,
                        //Products can be fetched from a ProductService class
                        Product = ctx.Products
                        .Where(y => y.ProductID == x.ProductID)
                        .Select(y => y.ProductName.Contains("contains a gift") ? "Gift" : y.ProductName)
                        .FirstOrDefault()
                    }).ToArray();

                    return Ok(new OrderDeliveryDTO()
                    {
                        //Customer property can use AutoMapper to mapp properties
                        Customer = new CustomerResponseDTO() { FirstName = customer.FirstName, LastName = customer.LastName },
                        Order = new OrdersDTO()
                        {
                            //customer address can be an override of the ToString() methodin in the customer class
                            DeliveryAddress = $"{customer.FirstName} {customer.LastName} {customer.HouseNumber} {customer.Street} {customer.Postcode} {customer.Town}",
                            DeliveryExpected = order.DeliveryExpected,
                            OrderDate = order.OrderDate,
                            OrderItems = orderItems,
                            OrderNumber = order.OrderID
                        }

                    });
                }
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

       
    }
}
 