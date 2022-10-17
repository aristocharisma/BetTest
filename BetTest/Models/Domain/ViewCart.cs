namespace BetTest.Models.Domain
{
    public class ViewCart
    {
        public Guid Id { get; set; }
        public DateTime Adddate { get; set; }
        public Guid CartId { get; set; }
        public string UserId { get; set; }
        public int Qunatity { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Photo { get; set; }
        public decimal LineTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalInclVat { get; set; }
        public decimal? CartTotal { get; set; }

    }
}
