﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IPDEntities : DbContext
    {
        public IPDEntities()
            : base("name=IPDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EmployeeFamilyDetail> EmployeeFamilyDetails { get; set; }
        public virtual DbSet<Master_City> Master_City { get; set; }
        public virtual DbSet<Master_Class> Master_Class { get; set; }
        public virtual DbSet<Master_College> Master_College { get; set; }
        public virtual DbSet<Master_Country> Master_Country { get; set; }
        public virtual DbSet<Master_Department> Master_Department { get; set; }
        public virtual DbSet<Master_Designation> Master_Designation { get; set; }
        public virtual DbSet<Master_Discipline> Master_Discipline { get; set; }
        public virtual DbSet<Master_DocumentCategory> Master_DocumentCategory { get; set; }
        public virtual DbSet<Master_EducationCategory> Master_EducationCategory { get; set; }
        public virtual DbSet<Master_Form> Master_Form { get; set; }
        public virtual DbSet<Master_Language> Master_Language { get; set; }
        public virtual DbSet<Master_LeavingReason> Master_LeavingReason { get; set; }
        public virtual DbSet<Master_Location> Master_Location { get; set; }
        public virtual DbSet<Master_MaritalStatus> Master_MaritalStatus { get; set; }
        public virtual DbSet<Master_Relation> Master_Relation { get; set; }
        public virtual DbSet<Master_Role> Master_Role { get; set; }
        public virtual DbSet<Master_Specialization> Master_Specialization { get; set; }
        public virtual DbSet<Master_State> Master_State { get; set; }
        public virtual DbSet<Master_University> Master_University { get; set; }
        public virtual DbSet<WelcomeNoteDetail> WelcomeNoteDetails { get; set; }
        public virtual DbSet<UserReminder> UserReminders { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual DbSet<EmployeeSkillDetail> EmployeeSkillDetails { get; set; }
        public virtual DbSet<Master_Certification> Master_Certification { get; set; }
        public virtual DbSet<Master_SkillSet> Master_SkillSet { get; set; }
        public virtual DbSet<EmployeeContactDetail> EmployeeContactDetails { get; set; }
        public virtual DbSet<EmployeeProfessionalDetail> EmployeeProfessionalDetails { get; set; }
        public virtual DbSet<EmploymentDetail> EmploymentDetails { get; set; }
        public virtual DbSet<Master_Currency> Master_Currency { get; set; }
        public virtual DbSet<EmployeePersonalDetail> EmployeePersonalDetails { get; set; }
        public virtual DbSet<Master_Bloodgroup> Master_Bloodgroup { get; set; }
        public virtual DbSet<AdminEducationCategoryForUser> AdminEducationCategoryForUsers { get; set; }
        public virtual DbSet<EducationDocumentCategoryMapping> EducationDocumentCategoryMappings { get; set; }
        public virtual DbSet<DocumentDetail> DocumentDetails { get; set; }
        public virtual DbSet<Master_Document> Master_Document { get; set; }
        public virtual DbSet<LoginDetail> LoginDetails { get; set; }
        public virtual DbSet<CandidateChangeRequestsDetail> CandidateChangeRequestsDetails { get; set; }
        public virtual DbSet<CandidateGraphProgressDetail> CandidateGraphProgressDetails { get; set; }
        public virtual DbSet<EmployeeEducationDetail> EmployeeEducationDetails { get; set; }
        public virtual DbSet<EmploymentCount> EmploymentCounts { get; set; }
        public virtual DbSet<EducationCategoryUniversityBoardMapping> EducationCategoryUniversityBoardMappings { get; set; }
        public virtual DbSet<Master_SubDocumentsCategory> Master_SubDocumentsCategory { get; set; }
        public virtual DbSet<PendingCandidateDetail> PendingCandidateDetails { get; set; }
        public virtual DbSet<temp_city> temp_city { get; set; }
        public virtual DbSet<temp_uni> temp_uni { get; set; }
    
        public virtual ObjectResult<GetDocumentDetails_Result> GetDocumentDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDocumentDetails_Result>("GetDocumentDetails");
        }
    
        public virtual ObjectResult<GetEducationList_Result> GetEducationList(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetEducationList_Result>("GetEducationList", userIDParameter);
        }
    
        public virtual ObjectResult<GetProffesionalDetails_Result> GetProffesionalDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetProffesionalDetails_Result>("GetProffesionalDetails");
        }
    
        public virtual ObjectResult<DocumentStatus_Result> DocumentStatus(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DocumentStatus_Result>("DocumentStatus", userIdParameter);
        }
    }
}
