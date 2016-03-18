using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class InfrastructureDetailsService : IInfrastructureDetailsService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public InfrastructureDetailsService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }
                
        public IList<InfrastructureDetails> GetInfrastructureDetails(int projectId, int WeekId)
        {
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);
            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            var infrastructureDetails = this.context.Query<InfrastructureDetails>()
                .Where(p => p.ProjectId == projectId && p.IsActive == true).ToList();

            if (weekId != 0)
            {
                infrastructureDetails = infrastructureDetails.Where(p => p.WeekId == weekId).ToList();
            }

            if (infrastructureDetails.Count() == 0)
            {
                if (weekId >= currentWeekId)
                {
                    InfrastructureDetails prevWeekData = this.context.Query<InfrastructureDetails>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                    if (prevWeekData != null)
                    {
                        infrastructureDetails = this.context.Query<InfrastructureDetails>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                        foreach (var item in infrastructureDetails)
                        {
                            item.InfrastructureDetailId = 0;
                        }
                    }
                }

            }

            return infrastructureDetails;
        }

        public int SaveInfrastructureDetails(IList<InfrastructureDetails> InfrastructureDetails, ProjectStatus projectStatus, int weekId, string userName)
        {
            try
            {
                if (InfrastructureDetails != null)
                {
                    foreach (var lst in InfrastructureDetails)
                    {
                        if (lst.InfrastructureDetailId == 0)
                        {
                            lst.CreatedBy = userName;
                            lst.CreatedDate = DateTime.Now;
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;
                            lst.WeekId = weekId;
                            this.context.Add(lst);
                        }
                        else if (lst.InfrastructureDetailId != 0)
                        {
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;

                            this.context.Update(lst);
                        }

                    }

                    var dbInfrastructureDetails = context.Query<InfrastructureDetails>()
                                                 .Where(ps => ps.ProjectId == projectStatus.ProjectId
                                                  && ps.WeekId == weekId).ToList();

                    foreach (var dbInfrastructureDetail in dbInfrastructureDetails)
                    {
                        var isPresent = false;
                        foreach (var lst in InfrastructureDetails)
                        {
                            if (dbInfrastructureDetail.InfrastructureDetailId == lst.InfrastructureDetailId)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            dbInfrastructureDetail.ModifiedBy = userName;
                            dbInfrastructureDetail.ModifiedDate = DateTime.Now;
                            dbInfrastructureDetail.IsActive = false;
                            this.context.Update(dbInfrastructureDetail);
                           
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

