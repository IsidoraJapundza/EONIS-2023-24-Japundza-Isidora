namespace EONIS_IT34_2020.Models.Stripe
{
    public class PaymentRequest
    {
        public string PaymentMethodId { get; set; }
        public long Price { get; set; }
        public string OrderId { get; set; }
    }
}
