using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class PanelService : IPanelService
    {
        private readonly IDataContext _context;

        public PanelService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }
        
        public IEnumerable<Panel> GetAllPanelDetails()
        {
            return _context.Query<Panel>();
        }

        public IEnumerable<Panel> GetPanelDetails()
        {
            return _context.Query<Panel>().Where(x => x.IsDeleted == false);
        }

        Panel IPanelService.GetPositionById(int PanelId)
        {
            return _context.Query<Panel>().Where(x => x.PanelId == PanelId&&x.IsDeleted==false).FirstOrDefault();
        }

        Panel IPanelService.GetPositionByName(string Panel)
        {
            return _context.Query<Panel>().Where(x => x.PanelName.ToLower() == Panel.ToLower() && x.IsDeleted == false).FirstOrDefault();
        }

        public int Add(Panel Panel)
        {
            _context.Add(Panel);
            return Panel.PanelId;
        }

        public void Update(Panel Panel)
        {
            if (Panel.PanelName != null)
            {
                _context.Update(Panel);
            }
        }

        public void Delete(Panel Panel)
        {
            if (Panel.PanelId > 0)
            {
                Panel.IsDeleted = true;
                _context.Update(Panel);
            }
        }
    }
}
