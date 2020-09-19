using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class MiniJobView
	{
		public Guid JobId { get; set; }
		public string Title { get; set; }
		public string ServiceType { get; set; }
		public DateTime PostedOn { get; set; }
		public string Address { get; set; }
	}
}
