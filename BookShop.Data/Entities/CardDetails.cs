using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities
{
    public class CardDetails:IIdentifiable
    {
        public long Id { get; set; }
        public long CardNumber { get; set; }
        public int CVV { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => FullName; private set => FullName = FirstName + " " + LastName; }
    }
}
