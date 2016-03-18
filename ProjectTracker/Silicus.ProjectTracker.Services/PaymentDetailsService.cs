using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class PaymentDetailsService : IPaymentDetailsService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public PaymentDetailsService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }

        public IList<PaymentDetails> GetPaymentDetails(int projectId, int WeekId)
        {
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);


            var paymentDetails = this.context.Query<PaymentDetails>()
                .Where(p => p.ProjectId == projectId && p.IsActive == true).ToList();

            if (WeekId != 0)
            {
                paymentDetails = paymentDetails.Where(p => p.WeekId == weekId).ToList();
            }

            if (paymentDetails.Count() == 0)
            {
                if (weekId >= currentWeekId)
                {
                    PaymentDetails prevWeekData = this.context.Query<PaymentDetails>().Where(p => p.ProjectId == projectId).OrderByDescending(p => p.WeekId).FirstOrDefault();
                    if (prevWeekData != null)
                    {
                        paymentDetails = this.context.Query<PaymentDetails>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                        foreach (var item in paymentDetails)
                        {
                            item.PaymentDetailId = 0;
                        }
                    }
                }

            }

            return paymentDetails;
        }

        public int SavePaymentDetails(IList<PaymentDetails> PaymentDetails, ProjectStatus projectStatus, int weekId, string userName)
        {
            try
            {
                if (PaymentDetails != null)
                {

                    foreach (var lst in PaymentDetails)
                    {
                        if (lst.PaymentDetailId == 0)
                        {
                            lst.CreatedBy = userName;
                            lst.CreatedDate = DateTime.Now;
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;

                            lst.WeekId = weekId;
                            this.context.Add(lst);
                        }
                        else if (lst.PaymentDetailId != 0)
                        {
                            lst.ModifiedBy = userName;
                            lst.ModifiedDate = DateTime.Now;

                            this.context.Update(lst);
                        }
                                              
                    }

                    var dbPaymentDetails = context.Query<PaymentDetails>()
                                           .Where(ps => ps.ProjectId == projectStatus.ProjectId
                                           && ps.WeekId == weekId).ToList();

                    foreach (var dbPaymentDetail in dbPaymentDetails)
                    {
                        var isPresent = false;
                        foreach (var lst in PaymentDetails)
                        {
                            if (dbPaymentDetail.PaymentDetailId == lst.PaymentDetailId)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            dbPaymentDetail.ModifiedBy = userName;
                            dbPaymentDetail.ModifiedDate = DateTime.Now;
                            dbPaymentDetail.IsActive = false;

                            this.context.Update(dbPaymentDetail);
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
