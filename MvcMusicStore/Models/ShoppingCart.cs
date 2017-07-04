using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    //ShoppingCart 模型类处理  Cart 表的数据访问，另外，它还需要处理在购物车中增加或者删除项目的业务逻辑
    public partial class ShoppingCart
    {
        //数据库上下文
        MusicStoreDB storeDB = new MusicStoreDB();

        //购物车的标识
        string ShoppingCartId { get; set; }

        //用于在Session中保存当前用户的购物车标识
        public const string CartSessionKey = "CartId";

        /*这是一个静态方法，用来获取当前用户的购物车对象，它使用 GetCartId 方法来读取保存当前 Session 中的购物车标识，
         * GetCartId 方法需要 HttpContextBase 以便获取当前的 Session，找到购物车标识*/
        public static ShoppingCart GetCart(HttpContextBase context)//网站上下文，找到key，获取购物车标识，找到购物车
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        //控制器作为参数，据此找到上下文，调用GetCart()方法
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        /*将专辑对象作为参数传入方法，添加专辑到购物车中，在 Cart 表中跟踪每个专辑的数量，
         *在这个方法中，我们将会检查是在表中增加一个新行，还是仅仅在用户已经选择的专辑上增加数量*/
        public void AddToCart(Album album)
        {
            // Get the matching cart and album instances
            //检查该专辑是否已经在购物车中
            var cartItem = storeDB.Carts.SingleOrDefault(
c => c.CartId == ShoppingCartId
&& c.AlbumId == album.AlbumId);//该购物车id是否为我的购物车，该专辑是否在购物车中有了。（cards表里包含了所有人的购物车）

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                //保存到数据库中
                cartItem.Count++;
            }

            // Save changes
            storeDB.SaveChanges();
        }

        /*通过专辑的标识从用户的购物车中将这个专辑的数量减少 1，如果用户仅仅剩下一个，那么就删除这一行*/
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.Carts.Single(
cart => cart.CartId == ShoppingCartId
&& cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                //数量每次减1
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }

                // Save changes
                storeDB.SaveChanges();
            }

            return itemCount;
        }

        //删除用户购物车中所有的项目（清空购物车）
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }

            // Save changes
            storeDB.SaveChanges();
        }

        //获取购物项目的列表用来显示或者处理
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        //获取用户购物车中专辑的数量
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();//(int?)表示可空

            // Return 0 if all entries are null
            return count ?? 0;
        }

        //获取购物车中商品的总价
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        //将购物车转换为结账处理过程中的订单，即创建订单
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

                storeDB.OrderDetails.Add(orderDetail);

            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            storeDB.SaveChanges();

            // Empty the shopping cart
            //清空购物车
            EmptyCart();

            // Return the OrderId as the confirmation number
            //返回订单对象
            return order.OrderId;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                //如果当前没有购物车
                //购物车需要进行区分
                //如果用户已经登录，用户名就是购物车的标识
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    //为匿名用户创建一个GUID，作为购物车标识
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        //合并购物车，如果用户在匿名状态下加入购物车，登录后，需要将其购物车合并到登录后的购物车
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDB.SaveChanges();
        }
    }
}