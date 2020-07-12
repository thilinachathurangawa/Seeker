using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class Job : AuditableBaseModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string JobNumber { get; set; }
		public string Address { get; set; }
		public string CreatedUserId { get; set; }
		public virtual ApplicationUser CreatedUser { get; set; }
		public Decimal Budget { get; set; }
		public DateTime FromDateTime { get; set; }
		public DateTime ToDateTime { get; set; }
		public bool IsDeleted { get; set; }
		public virtual ICollection<Attachment> Attachments { get; set; }
	}
}
