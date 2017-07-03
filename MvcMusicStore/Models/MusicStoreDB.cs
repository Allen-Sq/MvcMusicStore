using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    //上下文类将反映 Entity Framework 数据库的上下文，用来处理创建，读取，更新和删除的操作
    /*对于 Code First 来说，我们首先定义模型，然后通过模型来创建数据库，
     * 甚至也不需要写 Insert 语句，我们可以通过标准的 C# 代码来创建表中的记录。*/
    public class MusicStoreDB : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MusicStoreDB() : base("name=MusicStoreDB")
        {
        }

        public System.Data.Entity.DbSet<MvcMusicStore.Models.Album> Albums { get; set; }

        public System.Data.Entity.DbSet<MvcMusicStore.Models.Artist> Artists { get; set; }

        public System.Data.Entity.DbSet<MvcMusicStore.Models.Genre> Genres { get; set; }
    
    }
}
