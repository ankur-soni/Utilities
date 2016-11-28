using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;


namespace Silicus.Ensure.Services.Interfaces
{
    public interface ITagsService
    {
        IEnumerable<Tags> GetTagsDetails();

        int Add(Tags Tag);

        void Update(Tags Tag);

        void Delete(Tags Tag);
    }
}
