using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    //表示订单的明细
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }//主键
        public int OrderId { get; set; }//标识属于哪个订单
        public int AlbumId { get; set; }
        public int Quantity { get; set; }//数量
        public decimal UnitPrice { get; set; }//单价
        public virtual Album Album { get; set; }//专辑对象
        public virtual Order Order { get; set; }//订单对象
    }
}