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
        /// <summary>
        /// 负责决定当浏览网站首页时触发的事件
        /// 该方法渲染了Home Index视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("about");//重写隐式约定中的视图选择逻辑，使首页显示其他页面
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}