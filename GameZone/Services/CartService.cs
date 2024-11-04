using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
       
     
        public CartService(ApplicationDbContext context)
        {
            _context = context;
          
        }

        public  IEnumerable<CartItem> GetAll()
        {
            var cartItems = _context.CartItems.Include(c => c.Game).ToList();


            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
            }

            return cartItems;
        }

        public IEnumerable<CartItem> AddToCart(int id)
        {

            var game = _context.Games.Find(id);
            //if (game != null)
            //{
                CartItem item = new CartItem
                {
                    GameId = game.Id,
                    Quantity = 1
                };

                List<CartItem> items = new List<CartItem>(); ;
                items.Add(item);


                _context.CartItems.Add(item);
                _context.SaveChanges();
                return items;

           

        }
        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = _context.CartItems.Find(id);

            if (game is null)
                return isDeleted;
            _context.Remove(game);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                
            }

            return isDeleted;
        }

        public IEnumerable<CartItem> DisplayCart()
        {
            var cart = _context.CartItems.ToList();
            return cart;
        }
    }
}
