using Microsoft.Extensions.Logging;
using MMTApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMTApi.Services
{
    public class OrderService
    {
        private readonly ILogger<OrderService> logger;

        public OrderService(ILogger<OrderService> logger)
        {
            this.logger = logger;
        }
        public async Task<Order> GetOrdersAsync(string customerId)
        {
            return await Task.Run(() => {
                logger.LogInformation("Get orders called");
                using (var ctx=new DataContext())
                {
                    var order= ctx.Orders.Where(x => x.CustomerID == customerId).OrderByDescending(x => x.OrderDate).FirstOrDefault();
                    return order;
                }
            });
        }
    }
}
