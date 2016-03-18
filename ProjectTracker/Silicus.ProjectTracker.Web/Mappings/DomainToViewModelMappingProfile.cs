using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Web.ViewModel;
using Silicus.ProjectTracker.Models.DataObjects;


namespace Silicus.ProjectTracker.Web.Mappings
{
    [ExcludeFromCodeCoverage]
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ProjectSummary, ProjectSummaryViewModel>();

            Mapper.CreateMap<Project, ProjectViewModel>();

            Mapper.CreateMap<ProjectResourceUtilization, ProjectResourceUtilizationViewModel>();

            Mapper.CreateMap<ProjectComplaint, ProjectComplaintViewModel>();

            Mapper.CreateMap<ProjectStatus, ProjectViewModel>();

            Mapper.CreateMap<PaymentDetails, PaymentDetailsViewModel>();

            Mapper.CreateMap<ProjectStatusPieChartModel, ProjectStatusPieChartViewModel>();

            Mapper.CreateMap<ProjectTopDefaultersModel, ProjectTopDefaultersViewModel>();

            Mapper.CreateMap<ProjectTopSubmittedModel, ProjectTopSubmittedViewModel>();
                        
          
        }
    }
}