using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    /// <summary>
    /// 负责网站根目录下的home page、about page和contact page
    /// 每一个操作方法，都有一个同名的视图文件与其相对应，提供了视图与操作方法关联的基础。所以，可以直接利用隐式约定
    /// 使用return view();渲染视图，并返回html页面
    /// </summary>
    public class HomeController : Controller
    {
        MusicStoreDB storeDB = new MusicStoreDB();

        /// <summary>
        /// 负责决定当浏览网站首页时触发的事件
        /// 该方法渲染了Home Index视图，视图则使用渲染过的视图模板生成html，然后该方法返回html给浏览器
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //return View("about");//重写隐式约定中的视图选择逻辑，使首页显示其他页面

           var albums= GetTopSellingAlbums(5);//显示专辑畅销排行前5
            return View(albums);
        }

        //使用ViewBag向视图传递少量数据
        //控制器提供数据渲染视图
        public ActionResult About()
        {
            ViewBag.Message = "我的第一个MVC程序！";
            //利用隐式约定，定义了视图的选择逻辑，可以在/Viws/ControllerName目录下查找与操作方法名相同的视图文件
            //不需要指定视图的文件名，直接return view()渲染视图
            return View();
        }

        //控制器的 Action 方法通过返回的 ActionResule 传送模型对象给视图
        //这就允许控制器可以将所有需要的数据打包，然后传送给视图模板，以便生成适当的 HTML 
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*首页畅销专辑列表，这是私有方法，因为我们不希望直接可以访问到，这里为了简单将它写在了 HomeController 中，
         * 实际开发的时候，可能需要移到后台的逻辑服务中*/
        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
            .OrderByDescending(a => a.OrderDetails.Count())
            .Take(count)
            .ToList();
        }
    }
}