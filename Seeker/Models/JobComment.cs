using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class JobComment : BaseModel
	{
		public Guid JobId { get; set; }
		public string CommentedUserId { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public virtual ApplicationUser CommentedUser { get; set; }
	}
}
