using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ConsoleApplication1;
using Eda.RDBI.Entities;
using Eda.RDBI.Logger;
using Eda.RDBI.Models.DataObjects;
using IDataContext = Eda.RDBI.Entities.IDataContext;

namespace Eda.RDBI.Services.NotificationFeed
{
    public class FeedProcessor : IFeedProcessor
    {
        private readonly ILogger _logger;
        private readonly IDataContextFactory _dataContextFactory;

        public FeedProcessor(ILogger logger, IDataContextFactory dataContextFactory)
        {
            _logger = logger;
            _dataContextFactory = dataContextFactory;
        }

        public void ProcessFeed()
        {
            try
            {
                _logger.Log("ProcessFeed() called...", LogCategory.Information);

                using (var rss = new RssReader(ConfigurationManager.AppSettings["FeedUrl"]))
                {
                    var items = rss.Execute().ToList();
                    var newNofication = new List<RssItem>();

                    if(items.Count <= 0)
                        return;

                    try
                    {
                        using (IDataContext dataContext = _dataContextFactory.Create(ConnectionType.Ip))
                        {
                            var notifications = dataContext.Query<NotificationMessage>().ToList();

                            newNofication.AddRange(notifications.Any()
                                ? items.Where(rssItem => notifications.All(n => n.StartDate != rssItem.Date))
                                : items);

                            foreach (var rssNotification in newNofication)
                            {
                                var notification = new NotificationMessage
                                {
                                    Text = rssNotification.Title,
                                    Link = rssNotification.Link,
                                    StartDate = rssNotification.Date,
                                    EndDate = rssNotification.ExpireOn,
                                    NotificationId = Guid.NewGuid()
                                };

                                dataContext.Add(notification);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void LogException(Exception exception)
        {
            _logger.Log(exception);
            System.Diagnostics.Trace.WriteLine(exception.Message);
        }
    }
}