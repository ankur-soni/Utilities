﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UtilityDataSyncLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EnableDevEntities : DbContext
    {
        public EnableDevEntities()
            : base("name=EnableDevEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<vwExt_Client> vwExt_Client { get; set; }
        public virtual DbSet<vwExt_Company> vwExt_Company { get; set; }
        public virtual DbSet<vwExt_Department> vwExt_Department { get; set; }
        public virtual DbSet<vwExt_Engagement> vwExt_Engagement { get; set; }
        public virtual DbSet<vwExt_EngagementPermissions> vwExt_EngagementPermissions { get; set; }
        public virtual DbSet<vwExt_EngagementRoles> vwExt_EngagementRoles { get; set; }
        public virtual DbSet<vwExt_EngagementRolesSchedule> vwExt_EngagementRolesSchedule { get; set; }
        public virtual DbSet<vwExt_EngagementTaskTypes> vwExt_EngagementTaskTypes { get; set; }
        public virtual DbSet<vwExt_EngagementType> vwExt_EngagementType { get; set; }
        public virtual DbSet<vwExt_Location> vwExt_Location { get; set; }
        public virtual DbSet<vwExt_LocationHoliday> vwExt_LocationHoliday { get; set; }
        public virtual DbSet<vwExt_LocationWorkingSchedule> vwExt_LocationWorkingSchedule { get; set; }
        public virtual DbSet<vwExt_Resource> vwExt_Resource { get; set; }
        public virtual DbSet<vwExt_ResourceHistory> vwExt_ResourceHistory { get; set; }
        public virtual DbSet<vwExt_ResourceSkillLevel> vwExt_ResourceSkillLevel { get; set; }
        public virtual DbSet<vwExt_ResourceWorkingScheduleHistory> vwExt_ResourceWorkingScheduleHistory { get; set; }
        public virtual DbSet<vwExt_Skill> vwExt_Skill { get; set; }
        public virtual DbSet<vwExt_TimeCard> vwExt_TimeCard { get; set; }
        public virtual DbSet<vwExt_TimeCardResourceEngagements> vwExt_TimeCardResourceEngagements { get; set; }
        public virtual DbSet<vwExt_TimeCardResourceTaskTypes> vwExt_TimeCardResourceTaskTypes { get; set; }
        public virtual DbSet<vwExt_TimeZoneMaster> vwExt_TimeZoneMaster { get; set; }
        public virtual DbSet<vwExt_Title> vwExt_Title { get; set; }
        public virtual DbSet<vwExt_TKT_Queue> vwExt_TKT_Queue { get; set; }
        public virtual DbSet<vwExt_TKT_QueueTypeMaster> vwExt_TKT_QueueTypeMaster { get; set; }
        public virtual DbSet<vwExt_TKT_RosterCalenderInfo> vwExt_TKT_RosterCalenderInfo { get; set; }
        public virtual DbSet<vwExt_TKT_RosterCalenderUsers> vwExt_TKT_RosterCalenderUsers { get; set; }
        public virtual DbSet<vwExt_TKT_RosterResourceTypeMaster> vwExt_TKT_RosterResourceTypeMaster { get; set; }
        public virtual DbSet<vwExt_TKT_RosterShift> vwExt_TKT_RosterShift { get; set; }
        public virtual DbSet<vwExt_TKT_RosterTypeMaster> vwExt_TKT_RosterTypeMaster { get; set; }
        public virtual DbSet<vwExt_User> vwExt_User { get; set; }
        public virtual DbSet<vwExt_WeekDayMaster> vwExt_WeekDayMaster { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<vwExt_ResourceType> vwExt_ResourceType { get; set; }
    }
}