using Seeker.Constants;
using Seeker.Models;
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
		Task<JobListCountViewModel> GetJobListCountAsync(string userId, UserType userType);
		Task<JobListViewModel> GetTimeLineJobListJobAsync(string userId, int pageNumber);
		Task<bool> AddJobComment(CommentViewModel jobComment);
		Task<JobViewModel> GetDashboardJobMainDetails(Guid jobId, string userId);
		Task<bool> PlaceBid(BidViewModel bid);
		Task<bool> BidApprove(BidViewModel bid);
		Task<bool> CancelJobByClient(Guid jobId);
		Task<bool> JobMoveToInprogress(Guid jobId);
		Task<bool> JobCompleteByProvider(Guid jobId);
		Task<JobViewModel> AddJobFeedbackAssync(JobFeedbackViewmodal feedback);
	}
}
