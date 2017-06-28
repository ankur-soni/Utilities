using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.JobVite
{
     public class APIResponse
    {
        public List<JobViteCandidateBusinessModel> candidates { get; set; }
        public int total { get; set; }
    }

    //public class HiringManager
    //{
    //    public string employeeId { get; set; }
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public string userId { get; set; }
    //    public string userName { get; set; }
    //}

    public class PrimaryHiringManager
    {
        public string employeeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
    }

    public class Recruiter
    {
        public string employeeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
    }

    public class Job
    {
        public string company { get; set; }
        public List<object> customField { get; set; }
        public string department { get; set; }
        public string eId { get; set; }
        public List<HiringManager> hiringManagers { get; set; }
        public string jobType { get; set; }
        public string location { get; set; }
        public PrimaryHiringManager primaryHiringManager { get; set; }
        public List<Recruiter> recruiters { get; set; }
        public string requisitionId { get; set; }
        public string subsidiaryId { get; set; }
        public string title { get; set; }
    }

    public class Resume
    {
        public string content { get; set; }
        public string format { get; set; }
        public string name { get; set; }
    }

    public class Application
    {
        public List<object> customField { get; set; }
        public string eId { get; set; }
        public string gender { get; set; }
        public bool hasArtifacts { get; set; }
        public Job job { get; set; }
        public string jobviteChannel { get; set; }
        public long lastUpdatedDate { get; set; }
        public string race { get; set; }
        public Resume resume { get; set; }
        public long sentDate { get; set; }
        public string source { get; set; }
        public string sourceType { get; set; }
        public string veteranStatus { get; set; }
        public string workflowState { get; set; }
    }

    public class JobViteCandidateBusinessModel
    {
        public string address { get; set; }
        public string address2 { get; set; }
        public Application application { get; set; }
        public string city { get; set; }
        public string companyName { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string eId { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string homePhone { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string mobile { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string title { get; set; }
        public string workPhone { get; set; }
        public string workStatus { get; set; }

        
    }
}