using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
    public class JobListCountViewModel
    {
        public int PostedJobCount { get; set; }
        public int BidRecivedOrApproveWatingJobCount { get; set; }
        public int BidAcceptedJobCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}
