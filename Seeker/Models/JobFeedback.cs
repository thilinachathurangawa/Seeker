using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
    public class JobFeedback : AuditableBaseModel
    {
        public string Feedback { get; set; }
        public int FeedbackRatings { get; set; }
        public bool IsProviderFeedback { get; set; }
        public bool IsClientFeedback { get; set; }
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
