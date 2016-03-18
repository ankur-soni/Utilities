namespace Silicus.Finder.Models
{
    public class DocumentBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
    }

    public class EntityA : DocumentBase
    {
        public int BankId { get; set; }
        public int AccountNo { get; set; }
    }

    public class Model : DocumentBase
    {
        public int BankId { get; set; }
        public int AccountNo { get; set; }
        public int InvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
