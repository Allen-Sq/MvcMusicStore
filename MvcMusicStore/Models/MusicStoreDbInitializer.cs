using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    /// <summary>
    /// 数据库初始化器，将项目设置为每次应用程序重启时重新创建数据库，并且在开发初期具有初始数据
    /// </summary>
    public class MusicStoreDbInitializer
        :System.Data.Entity.DropCreateDatabaseAlways<MusicStoreDB>
    {
        /// <summary>
        /// 重写Seed方法为应用程序创建一些初始的数据
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MusicStoreDB context)
        {
            context.Artists.Add(new Artist { Name = "Al Di Meola" });
            context.Genres.Add(new Genre { Name = "Jazz" });
            context.Albums.Add(new Album
            {
                Artist = new Artist { Name = "Rush" },
                Genre = new Genre { Name = "Rock" },
                Price = 9.99m,
                Title = "Caravan"
            });
            base.Seed(context);
        }
    }
}