using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreManagerController : Controller
    {
        //手工在控制器中增加数据访问的实体上下文对象
        MvcMusicStore.Models.MusicStoreDB db
            = new MvcMusicStore.Models.MusicStoreDB();      
        //private MusicStoreDB db = new MusicStoreDB();

        // GET: /StoreManager/
        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            //var albums = db.Albums.Include("Genre").Include("Artist");
            return View(albums.ToList());
        }

        // GET: /StoreManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: /StoreManager/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            return View();
        }

        // POST: /StoreManager/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // GET: /StoreManager/Edit/5
        /*在需要用户编辑数据的时候，我们首先提供一个编辑表单，用户得到这个编辑表单的方式一般是通过某个超级链接，
         * 这样的请求方式将是 GET 请求，当这样的请求到达服务器的时候，我们向客户端返回编辑页面，允许用户编辑数据*/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            //为了构建从数据库中所有可能得到的流派和艺术家的列表，并将这些列表存储在viewbag中以便以后让dropdownlist辅助方法检索
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // POST: /StoreManager/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        /*在编辑数据的窗体中，我们提供一个 form 表单，这个表单的提交方式设置为 Post 方式，
         * 用户在提交表单的时候，将填写的数据提交到服务器。由于此时的提交方式成为  Post 方式，
         * 这就允许我们在服务器上通过请求的提交方式区分出来请求的类型。
         * 这样，我们就可以在 Controlller 中提供同名的 Action 来处理用户的编辑操作，
         * Get 方式的 Action 用来提供编辑表单，而 Post 方式的 Action 用来获取用户提交的数据。*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)//使用模型绑定器构建album参数，检查Album类，并查找能用于绑定的所有Album属性。
        {
            //检查模型对象的有效性，确保用户输入有用的专辑特性值，有效则是happy path，无效则是sad path
            if (ModelState.IsValid)
            {
                //告知数据上下文该对象在数据库中已经存在，所以框架应获取现有的专辑应用数据而不是再创建一个新的专辑记录。
                db.Entry(album).State = EntityState.Modified;
                //在数据上下文中调用SaveChanges，这时上下文将生成一条SQL UPDATE命令来更新对应的字段值以保留新值。
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // GET: /StoreManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: /StoreManager/Delete/5
        //用来处理点击确认删除之后的请求
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
