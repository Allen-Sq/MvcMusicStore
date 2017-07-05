using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    //增加授权的标注 [Authorize]，来确定用户必须在登录之后才能访问。
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoreDB storeDB = new MusicStoreDB();
        //出于简化的考虑，在这个教程中没有处理付款的信息，作为替代，我们允许用户输入一个促销代码，这里促销代码定义在常量 PromoCode。
        const string PromoCode = "FREE";

        //用来显示一个用户输入信息的表单
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //验证用户的输入，处理订单
        /*如果成功了就完成订单，如果失败了就重新显示表单。
         *在验证了表单之后，我们将会直接检查促销代码，假设所有的信息都是正确的，
         *我们将会在订单中保存信息，告诉购物车对象完成订单处理，最后，重定向到完成的 Complete Action 方法*/
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                    return RedirectToAction("Complete",new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        /*一旦完成了结账处理，用户将被重定向到 Complete 方法， 这个 Action 方法将会进行简单的检查，
         * 在显示订单号之前，检查订单是否属于当前登录的用户*/
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
            o => o.OrderId == id &&
            o.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                //错误视图创建项目的时候，保存在  /Views/Shared 文件夹中的 error.cshtml 生成
                return View("Error");
            }
        }
	}
}