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

namespace MMTApi.Services
{
    public class CustomerService
    {
        private readonly DataContext dataContext;
        private readonly ILogger<CustomerService> logger;
        private readonly IConfiguration configuration;

        public CustomerService(DataContext dataContext, ILogger<CustomerService> logger, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            this.logger = logger;
            this.configuration = configuration;
        }
        public async Task<Customer> GetCustomerAsync(CustomerOrderRequestDTO customerDTO)
        {
            return await Task.Run(async () =>
            {

                try
                {
                    //read token from configuration file 
                    var token = configuration.GetSection("ApiToken").Value;
                    // form url for getting customer information
                    string customerUrl = $"{token}&email={customerDTO.Email}";
                    //HttpClient can be injected in as a service via  the services.AddHttpClient() in startup configureServices method
                    HttpClient httpClient = new HttpClient();
                    Customer customer = default(Customer);
                    var response = await httpClient.GetAsync(customerUrl);
                    //Where the email address supplied is not a valid customer in the system, the API will return a 404 status
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;

                    customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                    return customer;
                }
                catch (Exception ex)
                {
                    logger.LogError($"{ex.Message} {ex.InnerException?.Message}");
                    return null;
                }
            });
        }
    }
}
