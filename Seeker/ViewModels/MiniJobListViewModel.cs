using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
	public class MiniJobListViewModel
	{		
		public List<MiniJobView> MiniJobViewModel { get; set; }
		public JobworkflowStatus WorkFlowStatus { get; set; }

	}
}
