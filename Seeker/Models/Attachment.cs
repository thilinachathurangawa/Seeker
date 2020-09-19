using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class Attachment : AuditableBaseModel
	{
		public Guid? JobId { get; set; }
		public virtual Job Job { get; set; }
		public string FileName { get; set; }
		public string Extension { get; set; }
		public int IsDeleted { get; set; }
		public int AttachmentType { get; set; }
		public string UserId { get; set; }
		public string FileUrl { get; set; }
	}
}
