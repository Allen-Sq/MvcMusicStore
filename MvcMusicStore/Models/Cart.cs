using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcMusicStore.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }//Cart表的主键，加上[Key]的标识，表示主键，否则EF CodeFirst 将会认为表的主键名为 CartId 或者 Id
        public string  CartId { get; set; }//允许匿名用户使用购物车，不是表的主键，标识购物车，可以用来区分不同用户的购物车
        public int AlbumId { get; set; }//购物车的商品即专辑，每一个专辑都有一个专辑的id
        public int Count { get; set; }//同一个专辑多次购买，可以不用重复显示，只需计算数量
        public System.DateTime DateCreated { get; set; }
        public virtual Album Album { get; set; }
    }
}