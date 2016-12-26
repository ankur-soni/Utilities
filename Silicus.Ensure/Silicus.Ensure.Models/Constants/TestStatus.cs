namespace Silicus.Ensure.Models.Constants
{
    public enum TestStatus
    {
        NotAssigned = 1,
        Assigned = 2,
        Submitted = 3,
        Evaluated = 4,
        Completed = 5
    }

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
        Admin = 3
    }
}