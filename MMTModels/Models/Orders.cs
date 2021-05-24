using System;

public class Order
{
    public int OrderID { get; set; }
    public string CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryExpected { get; set; }
    public bool ContainsGift { get; set; }
    public string ShippingMode { get; set; }
    public string OrderSource { get; set; }


}