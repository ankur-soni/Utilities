namespace Silicus.Ensure.Models.Constants
{
 

    public enum CandidateStatus
    {
        New = 1,
        TestAssigned = 2,
        TestSubmitted = 3,
        UnderEvaluation = 4,
        Selected = 5,
        Rejected = 6
    }

    public enum RoleName
    {
        Candidate = 1,
        Panel = 2,
        Admin = 3,
        Recruiter = 4
    }

    public enum Year
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Tweleve = 12,
        Thirteen = 13,
        Fourteen = 14,
        Fifteen = 15,
        Sixteen = 16,
        Seventeen = 17,
        Eighteen = 18,
        Nighteen = 19,
        Twenty = 20,
        TwentyOne = 21,
        TwentyTwo = 22,
        TwentyThree = 23,
        TwentyFour = 24,
        TwentyFive = 25,
        TwentySix = 26,
        TwentySeven = 27,
        TwentyEight = 28,
        TwentyNine = 29,
        Thirghty = 30,
        ThirghtyOne = 31,
        ThirghtyTwo = 32,
        ThirghtThree = 33,
        ThirghtyFour = 34,
        ThirghtyFive = 35,
    }

    public enum Month
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11
    }

    public static class EnumDisplayNames
    {
        public static string CandidateStatusString(this CandidateStatus status)
        {
            switch (status)
            {
                case CandidateStatus.New:
                    return "New";
                case CandidateStatus.Rejected:
                    return "Rejected";
                case CandidateStatus.Selected:
                    return "Selected";
                case CandidateStatus.TestAssigned:
                    return "Test assigned";
                case CandidateStatus.TestSubmitted:
                    return "Test submitted";
                case CandidateStatus.UnderEvaluation:
                    return "Under evaluation";
                default: return "";
            }
        }
    }

}