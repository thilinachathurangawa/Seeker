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
		private UserManager<ApplicationUser> _userManager;
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
				var jobs = await _dbContext.Jobs.Where(j => !j.IsDeleted && j.CreatedUserId == userId && j.workflowStatus == workFlowStatus).ToListAsync();

				if (jobs.Any()) {
					
					jobListViewModel.MiniJobViewModel = new List<MiniJobView>();
					foreach (var job in jobs)
					{
						var miniJobView = new MiniJobView();
						miniJobView.JobId = job.Id;
						miniJobView.Title = job.Title;
						miniJobView.ServiceType = job.ServiceType;
						miniJobView.PostedOn = job.CreatedDateTime;
						miniJobView.Address = job.Address;
						jobListViewModel.MiniJobViewModel.Add(miniJobView);
					}
					jobListViewModel.WorkFlowStatus = workFlowStatus;
				}

			}

			return jobListViewModel;

		}

		public async Task<List<JobViewModel>> GetTimeLineJobListJobAsync(string userId) {
			var jobListViewModel = new List<JobViewModel>();
			var pageSize = 10;
			var pageNumber = 1;
			var queryableJobs =  _dbContext.Jobs.Include(a=>a.Attachments).
				Where(j => !j.IsDeleted && (j.workflowStatus == JobworkflowStatus.BidRecivedOrApproveWating || j.workflowStatus == JobworkflowStatus.PostedJob)).AsQueryable();

			if (queryableJobs.Any())
			{
				foreach (var job in queryableJobs)
				{
					var jobViewModel = new JobViewModel();
					jobViewModel.Id = job.Id;
					jobViewModel.Title = job.Title;
					jobViewModel.ServiceType = job.ServiceType;
					jobViewModel.PostedOn = job.CreatedDateTime;
					jobViewModel.Address = job.Address;
					jobViewModel.JobLatitude = job.JobLatitude;
					jobViewModel.JobLongitude = job.JobLongitude;
					jobListViewModel.Add(jobViewModel);
				}			
			}
			//jobListViewModel = jobListViewModel.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			//jobListViewModel.PageNumber = pageNumber;
			//assetNoteList.PageTotal = notes.Count;
			//assetNoteList.PageSize = pageSize;
			return jobListViewModel;
		}

	}
}
