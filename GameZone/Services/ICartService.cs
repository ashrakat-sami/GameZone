namespace GameZone.Services
{
    public interface ICartService
    {
        IEnumerable<CartItem> GetAll();
        bool Delete(int id);
        IEnumerable<CartItem> AddToCart(int id);
        IEnumerable<CartItem> DisplayCart();
    }
}
