
namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class SelectableItemModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
    }
}
