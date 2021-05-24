using Microsoft.Extensions.Logging;
using MMTApi.Infrastructure;
using MMTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMTApi.Services
{
    public class OrderItemsService
    {
        private readonly ILogger<OrderItemsService> logger;

        public OrderItemsService(ILogger<OrderItemsService> logger)
        {
            this.logger = logger;
        }
        public async Task<OrderitemDTO[]> GetOrderItemsAsync(Order order)
        {
            return await Task.Run(() =>
            {
                using (var ctx = new DataContext())
                {
                    this.logger.LogInformation("Get order items called");
                    var orderItems = ctx.OrderItems.Where(x => x.OrderID == order.OrderID).Select(x => new OrderitemDTO()
                    {
                        PriceEach = x.Price,
                        Quantity = x.Quantity,
                        //Products can be fetched from a ProductService class
                        Product = ctx.Products
                         .Where(y => y.ProductID == x.ProductID)
                         .Select(y => order.ContainsGift ? "Gift" : y.ProductName)
                         .FirstOrDefault()
                    }).ToArray();
                    return orderItems;
                }

            });
        }
    }
}
