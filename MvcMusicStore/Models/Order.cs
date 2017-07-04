using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    //订单的汇总和发货信息
    public class Order
    {
        public int OrderId { get; set; }//订单唯一标识，主键
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}