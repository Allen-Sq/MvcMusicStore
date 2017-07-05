using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    public class Album
    {
        //快捷方式输入prop然后按Tab键两次
        [ScaffoldColumn(false)]//在编辑表单的时候，需要隐藏起来的的字符
        public virtual int AlbumId { get; set; }
        [DisplayName("Genre")]//定义表单字段的提示名称
        public virtual int GenreId { get; set; }
        [DisplayName("Artist")]
        public virtual int ArtistId { get; set; }
        [Required(ErrorMessage = "An Album Title is required")]//表示这个属性是必须提供内容的字段
        [StringLength(160)]//定义字符串类型的属性的最大长度
        public virtual string Title { get; set; }
        [Required(ErrorMessage="Price is required")]
        [Range(0.01, 100.00, ErrorMessage = "Price must be between 0.01 and 100.00")]//为数字类型的属性提供最大值和最小值
        public virtual decimal Price { get; set; }
        [DisplayName("Album Art URL")]
        [StringLength(1024)]
        public virtual string AlbumArtUrl { get; set; }

        //设置为virtual，这将会使 EF-Code First 使用延迟加载
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }

        //增加一个导航属性，以便EF知道关联的信息
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}