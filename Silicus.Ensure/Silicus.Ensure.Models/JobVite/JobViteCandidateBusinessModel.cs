﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.JobVite
{
    public class Resume
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
    }
    public class Coverletter
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
    }

    public class Attachment
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
    }

    public class CustomField
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Application
    {
        public string EId { get; set; }
        public string WorkflowState { get; set; }
        public string WorkStatus { get; set; }
        public string Disposition { get; set; }
        public string Source { get; set; }
        public string SourceType { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string VeteranStatus { get; set; }
        public string JobviteChannel { get; set; }
        public string Comments { get; set; }
        //public Resume Resume { get; set; }
        //public Coverletter Coverletter { get; set; }
        //public List<Attachment> Attachments { get; set; }
        public List<CustomField> CustomField { get; set; }
    }

    public class JobViteCandidateBusinessModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string HomePhone { get; set; }
        //public string WorkPhone { get; set; }
        //public string Mobile { get; set; }
        //public string Address { get; set; }
        //public string Address2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Country { get; set; }
        //public string PostalCode { get; set; }
        public string Title { get; set; }
        public Application Application { get; set; }
    }
}