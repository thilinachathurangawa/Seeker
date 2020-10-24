using Seeker.Constants;
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
		public string AssigndUserId { get; set; }
		public virtual ApplicationUser AssigndUser { get; set; }
		public Decimal Budget { get; set; }
		public DateTime FromDateTime { get; set; }
		public DateTime ToDateTime { get; set; }
		public bool IsDeleted { get; set; }
		public string ServiceType { get; set; }
		public virtual ICollection<Attachment> Attachments { get; set; }
		public JobworkflowStatus workflowStatus { get; set; }
		public string JobLatitude { get; set; }
		public string JobLongitude { get; set; }
		public bool IsProviderFeedbackRecived { get; set; }
		public bool IsClientFeedbackRecived { get; set; }
		public bool IsPaymentSend { get; set; }
		public JobAvailabiltyStatus availabilty { get; set; }
		public virtual ICollection<JobComment> JobComments { get; set; }
		public virtual ICollection<Bid> Bids { get; set; }
		public virtual ICollection<JobFeedback> JobFeedbacks { get; set; }

	}
}
