using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
    public class Bid : AuditableBaseModel
    {
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }       
        public Decimal Budget { get; set; }
        public string Description { get; set; }
        public bool IsBidAccepted { get; set; }
        public bool IsBidRejected { get; set; }
        public string AcceptedUserId { get; set; }
        public virtual ApplicationUser AcceptedUser { get; set; }
        public string CreatedUserId { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
    }
}
