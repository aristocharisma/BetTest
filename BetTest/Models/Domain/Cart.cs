namespace BetTest.Models.Domain
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string userId { get; set; }
        public DateTime Adddate { get; set; }
        public decimal? CartTotal { get; set; }
        
    }
}
