namespace BetTest.Models.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public int Qunatity { get; set; }

    }
}
