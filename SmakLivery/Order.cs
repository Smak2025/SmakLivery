using System;
using System.Collections.Generic;
using System.Text;

namespace SmakLivery
{
    public enum OrderStatus
    {
        Accepted, Prepearing, OnTheWay, Delivered, Cancelled
    }

    public class Order
    {
        public long Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Accepted;
        public long? CourierId { get; set; } = null;
    }
}
