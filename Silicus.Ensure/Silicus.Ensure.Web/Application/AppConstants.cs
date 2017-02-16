
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Silicus.Ensure.Web.Application
{
    public static class AppConstants
    {

        public const string TestStatusAssigned = "Assigned";
        public const string TestStatusUnAssigned = "UnAssigned";
        public const string TestStatusSubmitted = "Submitted";
        public const string ResumeNameSeparationCharacter = "_";
        public const string ResumeFolderName = "~/CandidateResume";
        public const string ProfilePhotoFolderName = "~/CandidateProfilePhoto";
    }
}