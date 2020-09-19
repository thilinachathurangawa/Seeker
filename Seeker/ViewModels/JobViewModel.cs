using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class JobViewModel
	{
		public Guid Id { get; set; }
		public string JobLatitude { get; set; }
		public string JobLongitude { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string JobNumber { get; set; }
		public string Address { get; set; }
		public string CreatedUserId { get; set; }
		public Decimal Budget { get; set; }
		public DateTime FromDateTime { get; set; }
		public DateTime ToDateTime { get; set; }
		public DateTime PostedOn { get; set; }
		public bool IsDeleted { get; set; }
		public string ServiceType { get; set; }
		public JobworkflowStatus workflowStatus { get; set; }
		public List<AttachmentViewModel> Attachments { get; set; }
	}
}
