using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Silicus.FrameworxProject.Services
{
    public class ProductBacklogService : IProductBacklogService
    {
        VssBasicCredential _credentials = new VssBasicCredential("", ConfigurationManager.AppSettings["TfsApipat"]);

        Uri _uri = new Uri(ConfigurationManager.AppSettings["TfsApiuri"]);

        public IEnumerable<ProductBacklog> GetAllProductBacklog()
        {
            Wiql wiql = new Wiql()
            {
                Query = "Select * " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Task' " +
                        "Order By [State] Asc, [Changed Date] Desc"
            };




            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItemQueryResult queryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;
                List<WorkItemField> result = workItemTrackingHttpClient.GetFieldsAsync(null).Result;

                if (queryResult != null && queryResult.WorkItems.Count() > 0)
                {
                    // return queryResult;
                    var workItems = GetWorkItemsWithSpecificFields(queryResult.WorkItems.Select(t => t.Id));
                    List<ProductBacklog> productBacklogs = new List<ProductBacklog>();
                    foreach (var item in workItems)
                    {
                        productBacklogs.Add(new ProductBacklog()
                        {
                            Id = item.Fields["System.Id"].ToString(),
                            Title = item.Fields["System.Title"].ToString(),
                            State = item.Fields["System.State"].ToString(),
                            Description = item.Fields.ContainsKey("System.Description") ? item.Fields["System.Description"].ToString() : Constants.InformationNotAvailableText,
                            Assignee = item.Fields.ContainsKey("System.AssignedTo") ? item.Fields["System.AssignedTo"].ToString() : Constants.InformationNotAvailableText,
                            TimeAllocated = item.Fields.ContainsKey("Microsoft.VSTS.Scheduling.OriginalEstimate") ? (double)item.Fields["Microsoft.VSTS.Scheduling.OriginalEstimate"] : 0.00,
                            TimeSpent = item.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork") ? (double)item.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] : 0.0
                        });
                    }

                    return productBacklogs;
                }
                else
                {
                    throw new NullReferenceException("Wiql '" + wiql.Query + "' did not find any results");
                }
            }
        }

        private List<WorkItem> GetWorkItemsWithSpecificFields(IEnumerable<int> ids)
        {
            var fields = new string[] {
                "System.Id",
                "System.Title",
                "System.State",
                "System.Description",
                "System.AssignedTo",
                "Microsoft.VSTS.Scheduling.OriginalEstimate",
                "Microsoft.VSTS.Scheduling.CompletedWork"
            };

            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                List<WorkItem> results = workItemTrackingHttpClient.GetWorkItemsAsync(ids, fields).Result;
                return results;
            }
        }
    }
}