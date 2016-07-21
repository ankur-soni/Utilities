namespace Silicus.Reusable.Models
{
    public class Frameworx
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HtmlDescription { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}
