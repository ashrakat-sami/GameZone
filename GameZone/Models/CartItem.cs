namespace GameZone.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int GameId { get; set; }
        public int Quantity { get; set; }

        public virtual Game Game { get; set; }
    }
}
