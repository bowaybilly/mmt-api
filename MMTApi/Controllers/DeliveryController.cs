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
            return await GetDeliveryServicesAsync(customerDTO);
        }

        private async Task<ActionResult<OrderDeliveryDTO>> GetDeliveryServicesAsync(CustomerRequestDTO customerDTO)
        {
            try
            {
                using (var ctx = new DataContext())
                {
                    var token = configuration.GetSection("ApiToken");
                    // base url and code can be stored in configuration file for development and ad production
                    string customerUrl = $"{token}&email={customerDTO.Email}";
                    //HttpClient can be injected in as a service via  the services.AddHttpClient() in startup configureServices method
                    HttpClient httpClient = new HttpClient();
                    Customer customer = default(Customer);
                    var response = await httpClient.GetAsync(customerUrl);
                    //Where the email address supplied is not a valid customer in the system, the API will return a 404 status
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) return NotFound(customerDTO);

                    customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                    //order should display the latest order item
                    var order = ctx.Orders.Where(x => x.CustomerID == customer.customerId).OrderByDescending(x => x.OrderID).FirstOrDefault();
                    //invalid request
                    if (order == null) return new OrderDeliveryDTO()
                    {
                        //Customer property can use AutoMapper to mapp properties
                        Customer = new CustomerResponseDTO() { FirstName = customer.FirstName, LastName = customer.LastName },
                        Order = null
                    };
                    var oi = ctx.OrderItems.AsEnumerable().Where(x => x.OrderID == order.OrderID).Select(x => new OrderitemDTO()
                    {
                        PriceEach = x.Price,
                        Quantity = x.Quantity,
                        Product = ctx.Products
                        .Where(y => y.ProductID == x.ProductID)
                        .Select(y => y.ProductName.Contains("contains a gift") ? "Gift" : y.ProductName)
                        .FirstOrDefault()
                    }).ToArray();


                    //get customer info

                    //The order data should be the most recent order placed by the customer(for clarity, in this case “most recent” means the order with the latest order date, even if that order date is in the future).
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
                            OrderItems = oi,
                            OrderNumber = order.OrderID
                        }

                    });
                }
            }
            catch (Exception)
            {

                return  StatusCode(500);
            }
        }
    }
}
