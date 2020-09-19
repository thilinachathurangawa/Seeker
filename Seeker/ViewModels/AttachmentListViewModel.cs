using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class AttachmentListViewModel
	{
		public List<AttachmentViewModel> Attachments { get; set; }
		public ResponseCodes Status { get; set; }
	}
}
