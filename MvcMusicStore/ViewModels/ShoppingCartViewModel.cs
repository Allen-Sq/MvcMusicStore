using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.ViewModels
{
    //视图模型，专门用于控制器向视图传递复杂数据，与数据库无关
    //使用这个模式，我们需要创建强类型的用于视图场景的类来表示信息，
    //这个类拥有视图所需要的值或者内容。
    //我们的控制器填充信息，然后传递这种类的对象供视图使用，
    //样就可以得到强类型的、编译时检查支持，并且在视图模板中带有智能提示
    public class ShoppingCartViewModel
    {
        //购物项目集合
        public List<Cart> CartItems { get; set; }
        //总价
        public decimal CartTotal { get; set; }
    }
}