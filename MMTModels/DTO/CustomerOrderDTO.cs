using System;

namespace MMTModels
{

    public class OrderDeliveryDTO
    {
        public CustomerResponseDTO Customer { get; set; }
        public OrdersDTO Order { get; set; }
    }

     public class OrdersDTO
    {
         public int? OrderNumber { get; set; }
         public DateTime? OrderDate { get; set; }
         public string DeliveryAddress { get; set; }
         public OrderitemDTO[] OrderItems { get; set; }
         public DateTime? DeliveryExpected { get; set; }
     }

     public class OrderitemDTO
    {
         public string Product { get; set; }
         public int Quantity { get; set; }
         public decimal PriceEach { get; set; }
     }

}
