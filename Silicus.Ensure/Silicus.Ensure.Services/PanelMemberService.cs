using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Services
{
    public class PanelMemberService : IPanelMemberService
    {
         private readonly IDataContext _context;

         public PanelMemberService(IDataContextFactory dataContextFactory)
        {
            _context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public PanelMemberDetail GetPanelMeberDetails(int userId)
        {
            return _context.Query<PanelMemberDetail>().FirstOrDefault(x => x.UserId == userId);
        }

        public bool UpesertPanelMeberDetail(PanelMemberDetail panelMemberDetail)
        {
            var panelMenber = _context.Query<PanelMemberDetail>().FirstOrDefault(x => x.UserId == panelMemberDetail.UserId);
            if (panelMenber==null)
            {
                _context.Add(panelMemberDetail);
            }
            else
            {
                panelMemberDetail.Id = panelMenber.Id;
                _context.Update(panelMemberDetail);
            }

            return true;
        }
    }
}
