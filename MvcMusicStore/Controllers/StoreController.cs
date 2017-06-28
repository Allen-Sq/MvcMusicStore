using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        //以下方法称为控制器操作，响应URL请求，执行正确的操作，并向浏览器做出响应
        //该Index方法实现在页面上列出音乐商店里所有音乐流派的功能
        // GET: /Store/
        public string Index()
        {
            return "Hello from Store.Index()";
        }

        //控制器操作可以将查询字符串作为其操作方法的参数来接收
        // GET:/Store/Browse?genre=?Disco
        public string Browse(string genre)
        {
            //利用方法httputility.htmlencode来预处理用户输入，这样能阻止用户用链接向视图中注入js代码或html标记
            string message = HttpUtility.HtmlEncode("Store.Browse, Genre  = " + genre);
            return message;
        }

        //读取和显示直接嵌入到URL中的输入参数
        // GET:/Store/Details/5
        public string Details(int id)
        {
            string message = "Store.Details, ID= " + id;
            return message;
        }
    }
}