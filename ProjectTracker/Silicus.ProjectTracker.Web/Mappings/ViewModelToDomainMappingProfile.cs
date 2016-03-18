
using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Web.ViewModel;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.Mappings
{
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ProjectSummaryViewModel, ProjectSummary>();

            Mapper.CreateMap<ProjectViewModel, Project>();

            Mapper.CreateMap<ProjectResourceUtilizationViewModel, ProjectResourceUtilization>();

            Mapper.CreateMap<ProjectComplaintViewModel, ProjectComplaint>();

            Mapper.CreateMap<PaymentDetailsViewModel, PaymentDetails>();

            Mapper.CreateMap<ProjectStatusPieChartViewModel, ProjectStatusPieChartModel>();

            Mapper.CreateMap<ProjectTopDefaultersViewModel, ProjectTopDefaultersModel>();

            Mapper.CreateMap<ProjectTopSubmittedViewModel, ProjectTopSubmittedModel>();

        }
    }
}