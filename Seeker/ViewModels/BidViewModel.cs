using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
    public class BidViewModel
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string FromDateTimeDisplay { get; set; }
        public string ToDateTimeDisplay { get; set; }
        public string CreatedUserId { get; set; }
        public Decimal Budget { get; set; }
        public string Description { get; set; }
        public string BidPlacedUserName { get; set; }
        public string BidPlacedUserId { get; set; }
        public string ApprovedUserId { get; set; }


    }
}
