using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Silicus.FrameworxProject.DAL.Interfaces;
using Silicus.FrameworxProject.Models;
using Silicus.FrameworxProject.Services.Interfaces;
using Silicus.Reusable.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Silicus.FrameworxProject.Services
{
    public class ProductBacklogService : IProductBacklogService
    {
        VssBasicCredential _credentials = new VssBasicCredential("", ConfigurationManager.AppSettings["TfsApipat"]);
        Uri _uri = new Uri(ConfigurationManager.AppSettings["TfsApiuri"]);
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IFrameworxProjectDatabaseContext _FrameworxProjectDatabaseContext;
        public ProductBacklogService(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
            _FrameworxProjectDatabaseContext = _dataContextFactory.CreateFrameworxProjectDbContext();

        }
        public IEnumerable<ProductBacklog> GetAllProductBacklog(string projectName)
        {
            Wiql wiql = new Wiql()
            {
                Query = "Select * " +
                        "From WorkItems " +
                        "Where System.TeamProject='" + projectName + "' " +
                        "Order By [Changed Date] Desc"
            };

            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItemQueryResult queryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;

                if (queryResult != null && queryResult.WorkItems.Count() > 0)
                {
                    // return queryResult;
                    var workItems = GetWorkItemsWithSpecificFields(queryResult.WorkItems.Select(t => t.Id));
                    List<ProductBacklog> productBacklogs = new List<ProductBacklog>();
                    var detailsFromDb = _FrameworxProjectDatabaseContext.Query<ProductBacklog>().ToList();

                    foreach (var item in workItems)
                    {
                        productBacklogs.Add(new ProductBacklog()
                        {
                            Id = int.Parse(item.Fields["System.Id"].ToString()),
                            Title = item.Fields["System.Title"].ToString(),
                            State = item.Fields["System.State"].ToString(),
                            Type = item.Fields.ContainsKey("System.WorkItemType") ? item.Fields["System.WorkItemType"].ToString() : Constants.InformationNotAvailableText,
                            Area = item.Fields.ContainsKey("System.AreaPath") ? item.Fields["System.AreaPath"].ToString() : Constants.InformationNotAvailableText,
                            AssigneeDisplayName = detailsFromDb.Any(t => t.Id == item.Id) ? detailsFromDb.FirstOrDefault(t => t.Id == item.Id).AssigneeDisplayName : "Unassigned",
                            AssigneeEmail = detailsFromDb.Any(t => t.Id == item.Id) ? detailsFromDb.FirstOrDefault(t => t.Id == item.Id).AssigneeEmail : "",
                            AssignedBy = detailsFromDb.Any(t => t.Id == item.Id) ? detailsFromDb.FirstOrDefault(t => t.Id == item.Id).AssignedBy : "",
                            TimeAllocated = item.Fields.ContainsKey("Microsoft.VSTS.Scheduling.OriginalEstimate") ? (double)item.Fields["Microsoft.VSTS.Scheduling.OriginalEstimate"] : 0.00,
                            TimeSpent = item.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork") ? (double)item.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] : 0.0,
                            CreatedDate = (DateTime)item.Fields["System.CreatedDate"],
                            ChangedDate = (DateTime)item.Fields["System.ChangedDate"]
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
                "System.WorkItemType",
                "System.AreaPath",
                "System.AssignedTo",
                "Microsoft.VSTS.Scheduling.OriginalEstimate",
                "Microsoft.VSTS.Scheduling.CompletedWork",
                "System.CreatedDate",
                "System.ChangedDate"
            };

            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                List<WorkItem> results = workItemTrackingHttpClient.GetWorkItemsAsync(ids, fields).Result;
                return results;
            }
        }
        public IEnumerable<TeamProjectReference> GetTeamProjects()
        {
            // create project object
            using (ProjectHttpClient projectHttpClient = new ProjectHttpClient(_uri, _credentials))
            {
                IEnumerable<TeamProjectReference> projects = projectHttpClient.GetProjects().Result;
                return projects;
            }
        }
        public WorkItem UpdateTimeAllocated(ProductBacklog productBacklog)
        {
            return UpdateWorkItem(productBacklog.Id.Value, "Microsoft.VSTS.Scheduling.OriginalEstimate", productBacklog.TimeAllocated);
        }
        public WorkItem UpdateTimeSpent(ProductBacklog productBacklog)
        {
            return UpdateWorkItem(productBacklog.Id.Value, "Microsoft.VSTS.Scheduling.CompletedWork", productBacklog.TimeSpent);
        }
        private WorkItem UpdateWorkItem(int workItemId, string paramName, object paramValue)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/" + paramName,
                    Value = paramValue
                }
            );

            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItem result = workItemTrackingHttpClient.UpdateWorkItemAsync(patchDocument, workItemId).Result;
                return result;
            }
        }
        public ProductBacklog GetWorkItemDetails(int id)
        {
            var fields = new string[] {
                "System.Id",
                "System.Title",
                "System.Description",
                "System.State",
                "System.WorkItemType",
                "System.AreaPath",
                "System.AssignedTo",
                "Microsoft.VSTS.Scheduling.OriginalEstimate",
                "Microsoft.VSTS.Scheduling.CompletedWork",
                "System.CreatedDate",
                "System.ChangedDate"
            };

            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItem result = workItemTrackingHttpClient.GetWorkItemAsync(id, fields).Result;

                var backlogItem = new ProductBacklog()
                {
                    Id = int.Parse(result.Fields["System.Id"].ToString()),
                    Title = result.Fields["System.Title"].ToString(),
                    State = result.Fields["System.State"].ToString(),
                    Description = result.Fields.ContainsKey("System.Description") ? result.Fields["System.Description"].ToString() : "",
                    Type = result.Fields.ContainsKey("System.WorkItemType") ? result.Fields["System.WorkItemType"].ToString() : Constants.InformationNotAvailableText,
                    Area = result.Fields.ContainsKey("System.AreaPath") ? result.Fields["System.AreaPath"].ToString() : Constants.InformationNotAvailableText,
                    TimeAllocated = result.Fields.ContainsKey("Microsoft.VSTS.Scheduling.OriginalEstimate") ? (double)result.Fields["Microsoft.VSTS.Scheduling.OriginalEstimate"] : 0.00,
                    TimeSpent = result.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork") ? (double)result.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] : 0.0,
                    CreatedDate = (DateTime)result.Fields["System.CreatedDate"],
                    ChangedDate = result.Fields.ContainsKey("System.ChangedDate") ? (DateTime)result.Fields["System.ChangedDate"] : DateTime.Now
                };

                var detailsFromDb = _FrameworxProjectDatabaseContext.Query<ProductBacklog>().FirstOrDefault(t => t.Id == backlogItem.Id);
                if (detailsFromDb != null)
                {
                    backlogItem.AssigneeDisplayName = detailsFromDb.AssigneeDisplayName;
                    backlogItem.AssignedBy = detailsFromDb.AssignedBy;
                }

                return backlogItem;
            }
        }
        public void UpdateAssignee(ProductBacklog productBacklog)
        {
            if (_FrameworxProjectDatabaseContext.Query<ProductBacklog>().Any(t => t.Id == productBacklog.Id))
            {
                _FrameworxProjectDatabaseContext.Update<ProductBacklog>(productBacklog);
            }
            else
            {
                _FrameworxProjectDatabaseContext.Add<ProductBacklog>(productBacklog);
            }
        }
        public void AddWorkItem(ProductBacklog productBacklog, string projectName)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(
               new JsonPatchOperation()
               {
                   Operation = Operation.Add,
                   Path = "/fields/System.Title",
                   Value = productBacklog.Title
               }
           );

            patchDocument.Add(
             new JsonPatchOperation()
             {
                 Operation = Operation.Add,
                 Path = "/fields/System.Description",
                 Value = productBacklog.Description
             }
         );

            patchDocument.Add(
              new JsonPatchOperation()
              {
                  Operation = Operation.Add,
                  Path = "/fields/System.State",
                  Value = productBacklog.State
              }
          );

            patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Scheduling.OriginalEstimate",
                Value = productBacklog.TimeAllocated
            }
        );

            patchDocument.Add(
           new JsonPatchOperation()
           {
               Operation = Operation.Add,
               Path = "/fields/System.CreatedDate",
               Value = productBacklog.CreatedDate
           }
       );


            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItem result = workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, projectName, productBacklog.Type).Result;
                if (result != null)
                {
                    productBacklog.Id = result.Id;
                    _FrameworxProjectDatabaseContext.Add<ProductBacklog>(productBacklog);
                }
            }

        }

        public bool IsFrameworxUser(string emailAddress)
        {
            return _FrameworxProjectDatabaseContext.Query<FrameworxUser>().Any(u => u.EmailAddress == emailAddress);
        }

        public IEnumerable<WorkItemClassificationNode> GetAreas(string project, int depth = 100)
        {
            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
            {
                WorkItemClassificationNode result = workItemTrackingHttpClient.GetClassificationNodeAsync(project, TreeStructureGroup.Areas, null, depth).Result;
                IEnumerable<WorkItemClassificationNode> tree = result.Children;
                return result != null ? result.GetAllNodes() : null;
            }
        }
    }
}