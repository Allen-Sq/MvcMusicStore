namespace MvcMusicStore.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        //移除购物车信息，弹出提示
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}