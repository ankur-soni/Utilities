using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly ILogger _logger;

        public EmailTemplateService(IDataContextFactory dataContextFactory, ILogger logger)
        {
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _logger = logger;
        }

        public List<EmailTemplate> GetEmailTemplates()
        {
            return _encourageDatabaseContext.Query<EmailTemplate>().ToList();
        }
    }
}
