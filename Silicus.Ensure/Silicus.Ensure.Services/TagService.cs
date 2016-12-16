using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class TagService:ITagsService
    {
        private readonly IDataContext _context;

        public TagService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IEnumerable<Tags> GetTagsDetails()
        {
            return _context.Query<Tags>();
        }

        public int Add(Tags Tags)
        {
            _context.Add(Tags);
            return Tags.TagId;
        }

        public void Update(Tags Tags)
        {
            if (Tags.TagName != null)
            {
                _context.Update(Tags);
            }
        }

        public void Delete(Tags Tags)
        {
            if (Tags.TagName != null)
            {
                _context.Delete(Tags);
            }
        }

        public Tags GetTagDetailsByName(string tagName)
        {
            if (!string.IsNullOrWhiteSpace(tagName))
            {
               return _context.Query<Tags>().FirstOrDefault(y=>y.TagName==tagName);                
            }
            return null;
        }
    }
}
