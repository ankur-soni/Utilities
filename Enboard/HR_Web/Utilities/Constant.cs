using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Web.Utilities
{
    public class Constant
    {
        public struct DocumentCategory
        {
            public const int IdProof = 1;
            public const int AddressProof = 2;
            public const int Employment = 3;
            public const int Education = 4;
        }

        public struct SubCategory
        {
            public const int Graduation = 1;
            public const int PostGraduation = 2;
            public const int IdProof = 3;
            public const int AddressProof = 4;
            public const int Employment = 5;
            public const int Doctorate = 6;
        }

        public struct IDProof
        {
             public const int PANCard = 2;
             public const int DrivingLicence = 3; 
             public const int VoterID = 14;
             public const int MarriageCertificate = 15;
             public const int ValidWorkVISA = 16;
        }

        public struct AddressProof
        {
            public const int AadharCard = 5;
            public const int Passport = 4;
            public const int ElectricityBill = 17;
            public const int BSNLLandlineBill = 18;
            public const int NationalizedBanksAccountStatement = 19;
            public const int ValidRentAgreement = 20;
            public const int Rationcard = 31;
            public const int VoterId = 30;
        }

        public struct EmploymentProof
        {
            public const int ExperienceLetter = 11;
            public const int RelievingLetter = 12;
            public const int Latest3PaySlips = 13;
            public const int LatestSalaryRevisionLetter = 28;
            public const int AppointmentLetter = 29;
        }      

    }

    public static class Constants
    {
        public static int Married { get { return 1; } }
        public static int Single { get { return 2; } }
        public static int Father { get { return 1; } }
        public static int Mother { get { return 2; } }
        public static int Spouse { get { return 3; } }
        public static int Child { get { return 4; } }
        public static int Sibling { get { return 5; } }
        public static int Relative { get { return 6; } }
    }
}