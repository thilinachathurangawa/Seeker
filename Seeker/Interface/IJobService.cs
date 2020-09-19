using Seeker.Constants;
using Seeker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Interface
{
	public interface IJobService
	{
		Task<bool> CreateJobAsync(JobViewModel jobViewModel);
		Task<MiniJobListViewModel> GetJobListJobAsync(string userId, JobworkflowStatus workFlowStatus);
		Task<JobListViewModel> GetTimeLineJobListJobAsync(string userId);
	}
}
