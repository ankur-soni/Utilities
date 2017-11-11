using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
//using Ninject.Web.Common;
using Ninject.Syntax;
using Repository;
using Service;


using Ninject.Modules;
using System.Reflection;
using Repository.Interface;
using Repository.Concrete;
using Service.Interface;
using Service.Concrete;

namespace HR_Web.DI
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;
        public IKernel Kernel { get { return this._kernel; } }

        public NinjectDependencyResolver()
            : base()
        {
            this._kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());
        }

        public object GetService(Type serviceType)
        {
            return this._kernel.TryGet(
                serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._kernel.GetAll(
                serviceType);
        }
        public IBindingToSyntax<T> Bind<T>()
        {
            return this._kernel.Bind<T>();
        }
        public T ResolveType<T>()
        {
            return this._kernel.Get<T>();
        }
    }
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserRepository>().To<UserRepo>();
            Bind<IUserService>().To<UserService>();

            Bind<IPersonalRepository>().To<PersonalDetailsRepo>();
            Bind<IPersonalService>().To<PersonalService>();

            Bind<ILanguageRepository>().To<LanguageRepo>();
            Bind<ILanguageservice>().To<LanguageService>();

            Bind<ICityRepository>().To<CityRepository>();
            Bind<ICityService>().To<CityService>();

            Bind<IStateRepository>().To<StateRepository>();
            Bind<IStateService>().To<StateService>();

            Bind<ICountryRepository>().To<CountryRepository>();
            Bind<ICountryService>().To<CountryService>();

            Bind<IContactRepository>().To<ContactRepository>();
            Bind<IContactService>().To<ContactService>();

            Bind<IEducationRepository>().To<EducationRepository>();
            Bind<IEducationService>().To<EducationService>();

            Bind<IClassRepository>().To<ClassRepository>();
            Bind<IClassService>().To<ClassService>();

            Bind<IDisciplineRepository>().To<DisciplineRepository>();
            Bind<IDisciplineService>().To<DisciplineService>();

            Bind<ICollegeRepository>().To<CollegeRepository>();
            Bind<ICollegeService>().To<CollegeService>();

            Bind<IEducationCategoryRepository>().To<EducationCategoryRepository>();
            Bind<IEducationCategoryService>().To<EducationCategoryService>();

            Bind<ISpecializationRepository>().To<SpecializationRepository>();
            Bind<ISpecializationService>().To<SpecializationService>();

            Bind<IUniversityRepository>().To<UniversityRepository>();
            Bind<IUniversityService>().To<UniversityService>();

            Bind<IMaritalStatusRepository>().To<MaritalStatusRepository>();
            Bind<IMaritalStatusService>().To<MaritalStatusService>();

            Bind<IEmployementRepository>().To<EmployementRepository>();
            Bind<IEmployementService>().To<EmployementService>();

            Bind<IDocumentDetailsRepository>().To<DocumentDetailReposotory>();
            Bind<IDocumentDetailsService>().To<DocumentDetailsService>();

            Bind<IDocumentCategoryRepository>().To<DocumentCategoryRepository>();
            Bind<IDocumentCategoryService>().To<DocumentCategoryService>();

            Bind<IDocumentCatNewRepository>().To<DocumentCatNewRepository>();
            Bind<IDocumentCatNewService>().To<DocumentCatNewService>();


            Bind<IEducationCategoryUniversityBoardMappingRepository>().To<EducationCategoryUniversityBoardMappingRepository>();
            Bind<IEducationCategoryUniversityBoardMappingService>().To<EducationCategoryUniversityBoardMappingService>();

            Bind<IEducationDocumentCategoryMappingRepository>().To<EducationDocumentCategoryMappingRepository>();
            Bind<IEducationDocumentCategoryMappingService>().To<EducationDocumentCategoryMappingService>();

            Bind<ICandidateProgressDetailRepository>().To<CandidateProgressDetailRepository>();
            Bind<ICandidateProgressDetailService>().To<CandidateProgressDetailService>();

            Bind<IRoleRepository>().To<RoleRepository>();
            Bind<IRoleService>().To<RoleService>();

            Bind<IEmployeeRepository>().To<EmployeeRepository>();
            Bind<IEmployeeService>().To<EmployeeService>();

            Bind<ISkillSetRepository>().To<SkillSetRepository>();
            Bind<ISkillsetService>().To<SkillSetService>();

            Bind<ICertificateRepository>().To<CertificationRepository>();
            Bind<ICertificateService>().To<CertificateService>();

            Bind<IProfessionalDetailsRepository>().To<ProfessionalDetailsRepository>();
            Bind<IProfessionalDetailsService>().To<ProfessionalDetailsService>();

            Bind<IEmpSkillsRepository>().To<EmpSillsRepository>();
            Bind<IEmpSkillsService>().To<EmpSkillsService>();

            Bind<IRelationRepository>().To<RelationRepository>();
            Bind<IRelationService>().To<RelationService>();

            Bind<IFamilyDetailsRepository>().To<FamilyDetailsRepository>();
            Bind<IFamilyDetailsService>().To<FamilyDetailsService>();

            Bind<IDepartmentRepository>().To<DepartmentRepository>();
            Bind<IDepartmentService>().To<DepartmentService>();

            Bind<IDesignationRepository>().To<DesignationRepository>();
            Bind<IDesignationService>().To<DesignationService>();

            Bind<IDocumentRepository>().To<DocumentRepository>();
            Bind<IDocumentService>().To<DocumentService>();

            Bind<ICurrencyRepository>().To<CurrencyRepository>();
            Bind<ICurrencyService>().To<CurrencyService>();

            Bind<IBloodGroupRepository>().To<BloodGroupRepository>();
            Bind<IBloodGroupService>().To<BloodGroupService>();

            Bind<IEmploymentCountRepository>().To<EmploymentCountRepository>();
            Bind<IEmploymentCountService>().To<EmploymentCountService>();
        }
    }
}