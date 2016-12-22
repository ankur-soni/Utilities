using System;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.FrameworxProject.DAL.Interfaces;

namespace Silicus.FrameworxProject.Services
{
    public class FrameworxFeedbackService : IFrameworxFeedbackService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;

        public FrameworxFeedbackService(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();
        }
        public void SaveFeedbackDetails(FrameworxFeedback frameworxFeedback)
        {
            frameworxFeedback.LastChange = DateTime.UtcNow;
            _FrameworxProjectDatabaseContext.Add(frameworxFeedback);
        }
    }
}
