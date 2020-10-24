using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seeker.Constants;
using Seeker.Interface;
using Seeker.Models;
using Seeker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Services
{
	public class JobService: IJobService
	{
		private readonly AuthenticationContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		public JobService(AuthenticationContext context, UserManager<ApplicationUser> userManager)
		{
			_dbContext = context;
			_userManager = userManager;
		}

		public async Task<bool> CreateJobAsync(JobViewModel jobViewModel)
		{
			try
			{			

				var jobModal = new Job();
				jobModal.Id = Guid.NewGuid();
				jobModal.IsDeleted = false;
				jobModal.FromDateTime = jobViewModel.FromDateTime;
				jobModal.ToDateTime = jobViewModel.ToDateTime;
				jobModal.Title = jobViewModel.Title;
				jobModal.Description = jobViewModel.Description;
				jobModal.Address = jobViewModel.Address;
				jobModal.CreatedUserId = jobViewModel.CreatedUserId;
				jobModal.Budget = jobViewModel.Budget;
				jobModal.ServiceType = jobViewModel.ServiceType;
				jobModal.workflowStatus = jobViewModel.workflowStatus;
				jobModal.CreatedDateTime = DateTime.UtcNow;
				jobModal.availabilty = jobViewModel.Availabilty;
				jobModal.JobLatitude = jobViewModel.JobLatitude;
				jobModal.JobLongitude = jobViewModel.JobLongitude;

				await _dbContext.Jobs.AddAsync(jobModal);
				await _dbContext.SaveChangesAsync();

				if (jobViewModel.Attachments != null && jobViewModel.Attachments.Any()) {
					List<Attachment> attachmentList = new List<Attachment>();
					foreach (var attachment in jobViewModel.Attachments)
					{
						var file = await _dbContext.Attachments.Where(a => a.Id == attachment.Id).FirstAsync();
						attachmentList.Add(file);
					}

					if (attachmentList.Any()) {
						foreach (var attachment in attachmentList)
						{
							attachment.JobId = jobModal.Id;
						}
						await _dbContext.SaveChangesAsync();
					}

				}
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<MiniJobListViewModel> GetJobListJobAsync(string userId, JobworkflowStatus workFlowStatus) {

			var jobListViewModel = new MiniJobListViewModel();

			var user = await _userManager.FindByIdAsync(userId);
			if (user.UserType == UserType.Client) {
				var jobs = new List<Job>();

				if (workFlowStatus == JobworkflowStatus.Rejected)
				{
					jobs = await _dbContext.Jobs.Include(b => b.Bids).Where(j => j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == workFlowStatus).ToListAsync();
				}
				else
				{
					 jobs = await _dbContext.Jobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == workFlowStatus).ToListAsync();
				}
				if (jobs.Any()) {
					
					jobListViewModel.MiniJobViewModel = new List<MiniJobView>();
					foreach (var job in jobs)
					{
						var miniJobView = new MiniJobView();
						miniJobView.JobId = job.Id;
						miniJobView.Title = job.Title;
						miniJobView.ServiceType = job.ServiceType;
						miniJobView.PostedOn = job.CreatedDateTime.ToString("dd MMMM yyyy hh:mm tt");
						miniJobView.Address = job.Address;
						jobListViewModel.MiniJobViewModel.Add(miniJobView);
					}
					jobListViewModel.WorkFlowStatus = workFlowStatus;
				}

			}

			if (user.UserType == UserType.ServiceProvider)
			{
				var jobs = new List<Job>();

				if (workFlowStatus == JobworkflowStatus.Rejected) {
					jobs = await _dbContext.Jobs.Include(b => b.Bids).Where(j =>j.Bids.Any(b => b.CreatedUserId == userId && b.IsBidRejected)).ToListAsync();
				}
                else
                {
					jobs = await _dbContext.Jobs.Include(b => b.Bids).Where(j => !j.IsDeleted && j.Bids.Any(b => b.CreatedUserId == userId) && j.workflowStatus == workFlowStatus).ToListAsync();

				}


				if (jobs.Any())
				{

					jobListViewModel.MiniJobViewModel = new List<MiniJobView>();
					foreach (var job in jobs)
					{
						var miniJobView = new MiniJobView();
						miniJobView.JobId = job.Id;
						miniJobView.Title = job.Title;
						miniJobView.ServiceType = job.ServiceType;
						miniJobView.PostedOn = job.CreatedDateTime.ToString("dd MMMM yyyy hh:mm tt");
						miniJobView.Address = job.Address;
						jobListViewModel.MiniJobViewModel.Add(miniJobView);
					}
					jobListViewModel.WorkFlowStatus = workFlowStatus;
				}

			}

			return jobListViewModel;

		}


		public async Task<JobListCountViewModel> GetJobListCountAsync(string userId, UserType userType) {

			var jobListCountViewModel = new JobListCountViewModel();

			var queryableJobs = _dbContext.Jobs.Include(b=>b.Bids)
				.Include(u => u.CreatedUser).AsQueryable();

			if (userType == UserType.Client) {

				jobListCountViewModel.PostedJobCount = await queryableJobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.PostedJob).CountAsync();
				jobListCountViewModel.BidRecivedOrApproveWatingJobCount = await queryableJobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.BidRecivedOrApproveWating).CountAsync();
				jobListCountViewModel.BidAcceptedJobCount = await queryableJobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.BidAccepted).CountAsync();
				jobListCountViewModel.InProgressCount = await queryableJobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.InProgress).CountAsync();
				jobListCountViewModel.CompletedCount = await queryableJobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.Completed).CountAsync();
				jobListCountViewModel.RejectedCount = await queryableJobs.Where(j => j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == JobworkflowStatus.Rejected).CountAsync();
			}


			if (userType == UserType.ServiceProvider)
			{

				jobListCountViewModel.BidRecivedOrApproveWatingJobCount = await queryableJobs.Where(j => j.Bids.Any(b => b.CreatedUserId == userId) && j.workflowStatus == JobworkflowStatus.BidRecivedOrApproveWating).CountAsync();
				jobListCountViewModel.BidAcceptedJobCount = await queryableJobs.Where(j => j.Bids.Any(b => b.CreatedUserId == userId) && j.workflowStatus == JobworkflowStatus.BidAccepted).CountAsync();
				jobListCountViewModel.InProgressCount = await queryableJobs.Where(j => j.Bids.Any(b => b.CreatedUserId == userId) && j.workflowStatus == JobworkflowStatus.InProgress).CountAsync();
				jobListCountViewModel.CompletedCount = await queryableJobs.Where(j => j.Bids.Any(b => b.CreatedUserId == userId) && j.workflowStatus == JobworkflowStatus.Completed).CountAsync();
				jobListCountViewModel.RejectedCount = await queryableJobs.Where(j => j.Bids.Any(b => b.CreatedUserId == userId && b.IsBidRejected)).CountAsync();
			}


			return jobListCountViewModel;

		}

        public async Task<JobListViewModel> GetTimeLineJobListJobAsync(string userId, int pageNumber) {
			var jobListViewModel = new JobListViewModel();
			var pageSize = 2;
			var queryableJobs =  _dbContext.Jobs
				.Include(a=>a.Attachments)
				.Include(u=>u.CreatedUser)
				.Include(c=>c.JobComments).ThenInclude(cu=>cu.CommentedUser).
				Where(j => !j.IsDeleted && (j.workflowStatus == JobworkflowStatus.BidRecivedOrApproveWating || j.workflowStatus == JobworkflowStatus.PostedJob)).AsQueryable();

			var ConvertedListJobs = await queryableJobs.ToListAsync();

			var jobList = new List<JobViewModel>();
			jobList = AsJobViewModel(ConvertedListJobs);
			jobListViewModel.Jobs = jobList;

			jobListViewModel.TotalJobs = queryableJobs.Count();
			jobListViewModel.Jobs = jobListViewModel.Jobs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			jobListViewModel.PageNumber = pageNumber;
			jobListViewModel.PageSize = pageSize;
			return jobListViewModel;
		}



		public async Task<bool> AddJobComment(CommentViewModel jobComment)
		{
			try
			{
				var job = await _dbContext.Jobs.Include(c=>c.JobComments).Where(j => j.Id == jobComment.JobId).FirstAsync();

				if (!string.IsNullOrEmpty(jobComment.Comment))
				{
					JobComment comment = new JobComment();
					comment.Comment = jobComment.Comment;
					comment.CommentedUserId = jobComment.UserId;
					comment.JobId = jobComment.JobId;
					comment.CreatedDateTime = DateTime.UtcNow;

					job.JobComments.Add(comment);
					await _dbContext.SaveChangesAsync();
				}
				return true;
			}
			catch (Exception)
			{

				return false;
			}
			
		}


		public async Task<JobViewModel> GetDashboardJobMainDetails(Guid jobId, string userId) {

			var jobViewModel = new JobViewModel();
			var jobDetail = await _dbContext.Jobs
				.Include(a => a.Attachments)
				.Include(a => a.AssigndUser)
				.Include(u => u.CreatedUser)
				.Include(u => u.JobFeedbacks)
				.Include(u => u.Bids).ThenInclude(u=>u.CreatedUser)
				.Where(j => j.Id == jobId).FirstAsync();


			jobViewModel =  AsJobMainDetailViewModel(jobDetail);


			return jobViewModel;
		}




		public async Task<bool> PlaceBid(BidViewModel bid)
		{
			try
			{
				if (bid == null)
				{
					return false;
				}

				var bidModel = new Bid();

				bidModel.JobId = bid.JobId;
				bidModel.FromDateTime = bid.FromDateTime;
				bidModel.ToDateTime = bid.ToDateTime;
				bidModel.CreatedUserId = bid.CreatedUserId;
				bidModel.Budget = bid.Budget;
				bidModel.Description = bid.Description;


				await _dbContext.Bid.AddAsync(bidModel);
				await _dbContext.SaveChangesAsync();

				var job = await _dbContext.Jobs.Where(j => j.Id == bid.JobId).FirstAsync();

				job.workflowStatus = JobworkflowStatus.BidRecivedOrApproveWating;
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{

				return false;
			}

		}




		public async Task<bool> BidApprove(BidViewModel bid)
		{
			try
			{
				if (bid == null)
				{
					return false;
				}

				var AcceptedBid = await _dbContext.Bid.Where(b => b.Id == bid.Id).FirstAsync();

				AcceptedBid.IsBidAccepted = true;
				AcceptedBid.IsBidRejected = false;
				AcceptedBid.AcceptedUserId = bid.ApprovedUserId;

				await _dbContext.SaveChangesAsync();

				var job = await _dbContext.Jobs.Include(b=>b.Bids).Where(j => j.Id == bid.JobId).FirstAsync();

				job.workflowStatus = JobworkflowStatus.BidAccepted;

                foreach (var item in job.Bids)
                {
					if(item.Id != bid.Id) {
						item.IsBidRejected = true;
						item.IsBidAccepted = false;
					}

                }

				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{

				return false;
			}

		}


		public async Task<bool> CancelJobByClient(Guid jobId) {
			try
			{
				if (jobId == Guid.Empty)
				{
					return false;
				}				

				var job = await _dbContext.Jobs.Include(b => b.Bids).Where(j => j.Id == jobId).FirstAsync();
				job.IsDeleted = true;
				job.workflowStatus = JobworkflowStatus.Rejected;
				foreach (var item in job.Bids)
				{					
						item.IsBidRejected = true;
						item.IsBidAccepted = false;

				}
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}


		public async Task<bool> JobMoveToInprogress(Guid jobId)
		{
			try
			{
				if (jobId == Guid.Empty)
				{
					return false;
				}

				var job = await _dbContext.Jobs.Where(j => j.Id == jobId).FirstAsync();

				job.workflowStatus = JobworkflowStatus.InProgress;				
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}


		public async Task<bool> JobCompleteByProvider(Guid jobId)
		{
			try
			{
				if (jobId == Guid.Empty)
				{
					return false;
				}

				var job = await _dbContext.Jobs.Where(j => j.Id == jobId).FirstAsync();

				job.workflowStatus = JobworkflowStatus.Completed;
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}


		public async Task<JobViewModel> AddJobFeedbackAssync(JobFeedbackViewmodal feedback) {

			try
			{
				if (feedback == null)
				{
					return null;
				}

				var feedbackModal = new JobFeedback();

				feedbackModal.Id = Guid.NewGuid();
				feedbackModal.Feedback = feedback.Feedback;
				feedbackModal.FeedbackRatings = feedback.FeedbackRatings;
				feedbackModal.UserId = feedback.UserId;
				feedbackModal.JobId = feedback.JobId;
				feedbackModal.IsProviderFeedback = feedback.IsProviderFeedback;
				feedbackModal.IsClientFeedback = feedback.IsClientFeedback;


				await _dbContext.JobFeedbacks.AddAsync(feedbackModal);
				await _dbContext.SaveChangesAsync();

				var job = await _dbContext.Jobs.Where(j => j.Id == feedback.JobId).FirstAsync();

				job.IsClientFeedbackRecived = feedback.IsClientFeedback;
				job.IsProviderFeedbackRecived = feedback.IsProviderFeedback;
				await _dbContext.SaveChangesAsync();

				var feedbackJob = await GetDashboardJobMainDetails(feedback.JobId, feedback.UserId);
				return feedbackJob;
			}
			catch (Exception)
			{

				return null;
			}

		}











		#region private section


		private static JobViewModel AsJobMainDetailViewModel(Job job)
		{
			var transformJobViewModel = new JobViewModel();

			transformJobViewModel.Id = job.Id;
			transformJobViewModel.Title = job.Title;
			transformJobViewModel.ServiceType = job.ServiceType;
			transformJobViewModel.PostedOn = job.CreatedDateTime.ToString("dd MMMM yyyy hh:mm tt");
			transformJobViewModel.Address = job.Address;
			transformJobViewModel.JobLatitude = job.JobLatitude;
			transformJobViewModel.JobLongitude = job.JobLongitude;
			transformJobViewModel.Budget = job.Budget;
			transformJobViewModel.workflowStatus = job.workflowStatus;
			transformJobViewModel.Availabilty = job.availabilty;
			transformJobViewModel.Description = job.Description;
			transformJobViewModel.CreatedBy = string.Concat(job.CreatedUser.FirstName + " " + job.CreatedUser.LastName);
			transformJobViewModel.FromDateTime = job.FromDateTime;
			transformJobViewModel.ToDateTime = job.ToDateTime;
			transformJobViewModel.IsPaymentSend = job.IsPaymentSend;
			transformJobViewModel.IsClientFeedbackRecived = job.IsClientFeedbackRecived;
			transformJobViewModel.IsProviderFeedbackRecived = job.IsClientFeedbackRecived;
			transformJobViewModel.FromDateTimeDisplay = job.FromDateTime.ToString("dd MMMM yyyy hh:mm tt");
			transformJobViewModel.ToDateTimeDisplay = job.ToDateTime.ToString("dd MMMM yyyy hh:mm tt");

			if (job.Attachments != null)
			{
				transformJobViewModel.Attachments = GetAttachmentList(job.Attachments);
			}
			if(job.workflowStatus == JobworkflowStatus.Completed && job.JobFeedbacks != null)
            {
				transformJobViewModel.JobFeedbacks = GetFedbacktList(job.JobFeedbacks);
			}
			

			if (job.workflowStatus == JobworkflowStatus.BidRecivedOrApproveWating)
			{
				transformJobViewModel.Bids = GetBidList(job.Bids);
			}
			if (job.workflowStatus == JobworkflowStatus.BidAccepted || job.workflowStatus == JobworkflowStatus.InProgress || job.workflowStatus == JobworkflowStatus.Completed)
			{
				var acceptedBid = job.Bids.First(b => b.IsBidAccepted);
				transformJobViewModel.AcceptedBid = GetAcceptedBid(acceptedBid);
			}

			return transformJobViewModel;
		}


		private static List<JobViewModel> AsJobViewModel(List<Job> jobsList)
		{
			var transformJobViewModel = jobsList.Select(job => new JobViewModel
			{
				Id = job.Id,
				Title = job.Title,
				workflowStatus = job.workflowStatus,
				ServiceType = job.ServiceType,
				PostedOn = job.CreatedDateTime.ToString("dd MMMM yyyy hh:mm tt"),
				Address = job.Address,
				JobLatitude = job.JobLatitude,
				JobLongitude =job.JobLongitude,
				Budget = job.Budget,
				Availabilty = job.availabilty,
				Description = job.Description,
				CreatedBy = string.Concat(job.CreatedUser.FirstName +" "+ job.CreatedUser.LastName),
				FromDateTime = job.FromDateTime,
				ToDateTime =job.ToDateTime,
				FromDateTimeDisplay = job.FromDateTime.ToString("dd MMMM yyyy hh:mm tt"),
				ToDateTimeDisplay = job.ToDateTime.ToString("dd MMMM yyyy hh:mm tt"),
				Attachments = GetAttachmentList(job.Attachments),
				JobCommentList = GetJobComments(job.JobComments)
			}).ToList();

			return transformJobViewModel;
		}

		private static List<JobFeedbackViewmodal> GetFedbacktList(ICollection<JobFeedback> feedbackList)
		{
			var feedbacks = feedbackList.Select(file => new JobFeedbackViewmodal
			{
				JobId = file.JobId,
				Feedback = file.Feedback,
				FeedbackRatings = file.FeedbackRatings,
				UserId = file.UserId,
				IsClientFeedback = file.IsClientFeedback,
				IsProviderFeedback = file.IsProviderFeedback,
				
			}).ToList();

			return feedbacks;
		}

		private static List<AttachmentViewModel> GetAttachmentList(ICollection<Attachment> attachmentsList)
		{
			var attachmentList = attachmentsList.Select(file => new AttachmentViewModel
			{
				Name = file.FileName,
				Extension = file.Extension,
				Type = file.AttachmentType,
				Image = file.FileUrl,
				ThumbImage = file.FileUrl
			}).ToList();

			return attachmentList;
		}

		private static BidViewModel GetAcceptedBid(Bid acceptedBid)
		{
			var acceptedBidViewModel = new BidViewModel();
			acceptedBidViewModel.Id = acceptedBid.Id;
			acceptedBidViewModel.JobId = acceptedBid.JobId;
			acceptedBidViewModel.FromDateTimeDisplay = acceptedBid.FromDateTime.ToString("MM/dd/yyyy HH:mm");
			acceptedBidViewModel.ToDateTimeDisplay = acceptedBid.ToDateTime.ToString("MM/dd/yyyy HH:mm");
			acceptedBidViewModel.CreatedUserId = acceptedBid.CreatedUserId;
			acceptedBidViewModel.Budget = acceptedBid.Budget;
			acceptedBidViewModel.Description = acceptedBid.Description;
			acceptedBidViewModel.BidPlacedUserId = acceptedBid.CreatedUser.Id;
			acceptedBidViewModel.BidPlacedUserName = acceptedBid.CreatedUser.FirstName;

			return acceptedBidViewModel;
		}

		private static List<BidViewModel> GetBidList(ICollection<Bid> bid)
		{
			var bidList = bid.Select(file => new BidViewModel
			{
			Id = file.Id,
			JobId = file.JobId,
			FromDateTimeDisplay = file.FromDateTime.ToString("MM/dd/yyyy HH:mm"),
			ToDateTimeDisplay = file.ToDateTime.ToString("MM/dd/yyyy HH:mm"),
			CreatedUserId = file.CreatedUserId,
			Budget = file.Budget,
			Description = file.Description,
			BidPlacedUserId = file.CreatedUser.Id,
			BidPlacedUserName = file.CreatedUser.FirstName
			}).ToList();

			return bidList;
		}

		private static List<CommentViewModel> GetJobComments(ICollection<JobComment> commentList)
		{
			var commentListViewModel = commentList.Select(comment => new CommentViewModel
			{
				JobId= comment.JobId,
				UserId  =comment.CommentedUserId,
				UserName = string.Concat(comment.CommentedUser.FirstName + " " + comment.CommentedUser.LastName),
				CreatedOn = comment.CreatedDateTime.ToString("dd MMMM yyyy hh:mm tt"),
				Comment =comment.Comment

			}).ToList();

			return commentListViewModel;
		}

		#endregion

	}
}
