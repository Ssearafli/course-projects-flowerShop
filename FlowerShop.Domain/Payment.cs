namespace FlowerShop.Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public DateOnly PaymentDate {  get; set; }
        public DateOnly SubscriptionExpiryDate { get; set; }
        public string TariffName { get; set; }
        public float TariffAmount { get; set; }
        public string PaymentId { get; set; }
    }
}
