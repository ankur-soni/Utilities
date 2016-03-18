namespace Silicus.ProjectTracker.Models
{
    public class FilterItemModel
    {
        public string Name { get; set; }
        public string FilterName { get; set; }
        public bool IsModified { get; set; }
        public bool IsInline { get; set; }
        public bool IsInlineCheckbox { get; set; }
        public bool IsInlineToggle{ get; set; }
        public bool IsTabber { get; set; }
        public bool HasValues { get; set; }
    }
}
