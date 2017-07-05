using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {

        MusicStoreDB storeDB = new MusicStoreDB();// 访问MusicStoreEneities 类的对象实例
        //以下方法称为控制器操作，响应URL请求，执行正确的操作，并向浏览器做出响应
        //该Index方法实现在页面上列出音乐商店里所有音乐流派的功能
        // GET: /Store/
        public ActionResult Index()
        {
            //return "Hello from Store.Index()";

            //使用列表存储专辑流派信息
            //var genres = new List<Genre>
            //{
            //    new Genre {Name="Disco"},
            //    new Genre{Name="Jazz"},
            //    new Genre{Name= "Rock"}
            //};
            //return View(genres);

            var genres = storeDB.Genres.ToList();
            return this.View(genres);
        }

        //控制器操作可以将查询字符串作为其操作方法的参数来接收
        // GET:/Store/Browse?genre=?Disco
        public ActionResult Browse(string genre)
        {
            //利用方法httputility.htmlencode来预处理用户输入，这样能阻止用户用链接向视图中注入js代码或html标记（组织爱脚本注入攻击）
            //string message = HttpUtility.HtmlEncode("Store.Browse, Genre  = " + genre);
            //return message;
            //var genreModel = new Genre { Name = genre };
            //return View(genreModel);

            /*Single 方法使用一个 Lambda 表达式作为参数，表示我们希望获取匹配指定值的单个流派对象。
             * 通过 Include 方法可以指定我们希望获取的相关信息，这种方式非常有效。
             * 这样，我们就可以在一次数据访问中，既可以获取流派对象，也可以同时获取相关的专辑对象*/
            var genreModel = storeDB.Genres.Include("Albums").Single(g => g.Name == genre);
            return this.View(genreModel);
        }

        //读取和显示直接嵌入到URL中的输入参数
        // GET:/Store/Details/5
        public ActionResult Details(int id)
        {
            //string message = "Store.Details, ID= " + id;
            //return message;
            //var album = new Album { Title = "Album" + id };

            //使用强类型视图，允许设置视图的模型类型，可以代替ViewBag，这里表示从控制器向视图传递一个两边都是强类型的模型对象
            //在controller中向重载的View()方法中传递模型实例来指定模型，这里选择的是album模型（模板）
            //return View(album);

            var album = storeDB.Albums.Find(id);
            return View(album);
        }

        /*在 Action 方法上我们增加了 [ChildActionOnly] 标注，这意味着我们仅仅可以通过分部视图来访问这个 Action，
         * 这可以防止通过浏览 /Store/GenreMenu 来访问，对于分部视图来说，这不是必须的，但是一个很好的实践，
         * 因为我们希望我们的控制器方法被我们希望的方式使用，这里我们还返回了一个分部视图而不是一个普通的视图，
         * 这用来告诉视图引擎，不需要对这个视图使用布局，它将会被包含在其他的视图中*/
        // GET: /Store/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();
            return PartialView(genres);
        }
    }
}