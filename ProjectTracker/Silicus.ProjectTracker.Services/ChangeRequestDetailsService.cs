using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class ChangeRequestDetailsService : IChangeRequestDetailsService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public ChangeRequestDetailsService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }
                
        public IList<ChangeRequestDetails> GetChangeRequestDetails(int projectId, int WeekId)
        {
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            var changeRequestDetails = this.context.Query<ChangeRequestDetails>()
                .Where(p => p.ProjectId == projectId && p.IsActive == true).ToList();

            if (weekId != 0)
            {
                changeRequestDetails = changeRequestDetails.Where(p => p.WeekId == weekId).ToList();
            }

            if (changeRequestDetails.Count() == 0)
            {
                if (weekId >= currentWeekId)
                {
                     ChangeRequestDetails prevWeekData = this.context.Query<ChangeRequestDetails>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                     if (prevWeekData != null)
                     {
                         changeRequestDetails = this.context.Query<ChangeRequestDetails>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                         foreach (var item in changeRequestDetails)
                         {
                             item.ChangeRequestId = 0;
                         }
                     }
                }

            }


            return changeRequestDetails;
        }

        public int SaveChangeRequestDetails(IList<ChangeRequestDetails> ChangeRequestDetails, ProjectStatus projectStatus, int weekId, string userName)
        {
            try
            {
                if (ChangeRequestDetails != null)
                {
                    foreach (var lst in ChangeRequestDetails)
                    {
                        if (lst.ChangeRequestId == 0)
                        {
                            lst.CreatedBy = userName;
                            lst.CreatedDate = DateTime.Now;
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            lst.WeekId = weekId;
                            this.context.Add(lst);
                        }
                        else if (lst.ChangeRequestId != 0)
                        {
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            this.context.Update(lst);
                        }

                    }

                    var dbChangeRequestDetails = context.Query<ChangeRequestDetails>()
                                                 .Where(ps => ps.ProjectId == projectStatus.ProjectId
                                                  && ps.WeekId == weekId).ToList();

                    foreach (var dbChangeRequestDetail in dbChangeRequestDetails)
                    {
                        var isPresent = false;
                        foreach (var lst in ChangeRequestDetails)
                        {
                            if (dbChangeRequestDetail.ChangeRequestId == lst.ChangeRequestId)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            dbChangeRequestDetail.ModifiedBy = userName;
                            dbChangeRequestDetail.ModifiedDate = DateTime.Now;
                            dbChangeRequestDetail.IsActive = false;
                            this.context.Update(dbChangeRequestDetail);
                           
                        }
                    }
                }

                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}