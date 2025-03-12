using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Domain.shared
{
    public class Job 
    {
        public Guid Id { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedOn { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new HashSet<SavedJob>();
    }
}
