using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class JobListViewModel
	{
		public int TotalJobs { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public List<JobViewModel> Jobs { get; set; }
	}
}
