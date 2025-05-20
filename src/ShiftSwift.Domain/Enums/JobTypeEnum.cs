namespace ShiftSwift.Domain.Enums
{
    public enum JobTypeEnum
    {
        FullTime = 1,
        PartTime = 2,
        Freelance = 3
    }

    public enum GenderType
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum WorkModeEnum
    {
        OnSite = 1,
        Remotely = 2,
        Hybrid = 3
    }

    public enum SalaryTypeEnum
    {
        PerMonth = 1,
        PerHour = 2,
        Contract = 3
    }

    public enum ApplicationStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
        Shortlisted = 4,
        RemovedFromShortlist = 5

    }

    public enum QuestionType
    {
        Text = 1,
        TrueFalse = 2
    }


}
