using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class CommentViewModel
	{
		public Guid JobId { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string CreatedOn { get; set; }
		public string Comment { get; set; }
	}
}
