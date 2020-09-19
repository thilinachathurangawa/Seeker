using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class AttachmentViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Extension { get; set; }
		public string Type { get; set; }
		public string title { get; set; }
		public string Image { get; set; }
		public string ThumbImage { get; set; }		
	}
}
