using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Seeker.Constants;
using Seeker.Interface;
using Seeker.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Seeker.ViewModels;
using Seeker.Services;

namespace Seeker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
		private readonly IAzureBlobService _azureBlobService;
		private readonly AuthenticationContext _dbContext;
		private readonly ApplicationSettings _appSetting;
		private readonly IJobService _jobService;

		
		public JobController(IAzureBlobService azureBlobService, IOptions<ApplicationSettings> appSetting, AuthenticationContext context, IJobService jobService )
		{
			_azureBlobService = azureBlobService;
			_appSetting = appSetting.Value;
			_dbContext = context;
			_jobService = jobService;
		}

		[HttpGet]
		[Route("downloadAttachments")]
		public async Task<(string, string, byte[])> DownloadBlobAsync(Guid attachmentId)
		{
			var attachment = await _dbContext.Attachments.Where(e => e.Id == attachmentId).FirstOrDefaultAsync();

			if (attachment != null)
			{
				var attachmentFileType = GetContainerFileType(attachment.Extension);
				var containerName = GetContainerName(AttachmentFileTypes.Images);
				var fileName = attachment.Id.ToString() + attachment.Extension;
				var contentType = GetMimeType(fileName);

				byte[] file = await _azureBlobService.DownLoadFileToByteArray(_appSetting.AzureBlobStorageConnectionString, containerName, fileName);

				return (fileName, contentType, file);
			}

			return (string.Empty, string.Empty, null);
		}
	
		[HttpGet]
		[Route("getAttachment")]
		public async Task<System.Uri> GetAzureFileUrl(Guid attachmentId)
		{
			var attachment = await _dbContext.Attachments.Where(e => e.Id == attachmentId).FirstOrDefaultAsync();
			if (attachment != null)
			{
				try
				{
					//var fileName = attachment.Id.ToString() + attachment.Extension;
					var fileName = "1.jpg";
					var containerName = GetContainerName(AttachmentFileTypes.Images);
					var blockBlob = _azureBlobService.GetBlob(_appSetting.AzureBlobStorageConnectionString, containerName, fileName);

					await blockBlob.ExistsAsync();
					return blockBlob.Uri;
				}
				catch (Exception)
				{

					return null;
				}
				
			}
			else
				return null;
		}

		[Route("uploadAttachment")]
		[HttpPost]
		public async Task<JsonResult> ProcessImportFile(IEnumerable<IFormFile> files, string importedById)
		{
			if (files != null && files.Count() > 0)
			{
				AttachmentListViewModel AttachmentList = new AttachmentListViewModel();
				AttachmentList.Attachments = new List<AttachmentViewModel>();
				foreach (var file in files)
				{
					
					AttachmentViewModel uploadFile = new AttachmentViewModel();
					if (file == null)
						return new JsonResult(new { Status = ResponseCodes.FileUploadFailed});
					if (file.Length == 0)
						return new JsonResult(new { Status = ResponseCodes.FileUploadFailed});
					var extension = Path.GetExtension(file.FileName);
					var fileType = GetContainerFileType(extension);
					var containerName = GetContainerName(fileType);
					var attachmentId = Guid.NewGuid();
					var Url = await _azureBlobService.UploadFileToBlob(_appSetting.AzureBlobStorageConnectionString, containerName, file);
					if (Url != null)
					{
						var attachment = new Attachment
						{
							Id = attachmentId,
							FileName = file.FileName,
							Extension = extension,
							IsDeleted = 0,
							AttachmentType = (int)fileType,
							UserId = importedById,
							CreatedDateTime = DateTime.UtcNow,
							LastUpdatedDateTime = DateTime.UtcNow,
							CreatedBy = "System",
							LastUpdatedBy = "System",
							FileUrl = Url.ToString()
					};


						await _dbContext.Attachments.AddAsync(attachment);
						await _dbContext.SaveChangesAsync();

						uploadFile.Id = attachmentId;
						uploadFile.Extension = extension;
						uploadFile.Name = file.FileName;
						//uploadFile.title = file.FileName;
						uploadFile.Image = Url.ToString();
						uploadFile.ThumbImage = Url.ToString();




					}

					AttachmentList.Attachments.Add(uploadFile);
					AttachmentList.Status = ResponseCodes.FileUploadSuccess;
				}
				return new JsonResult(AttachmentList);
			}
			return new JsonResult(new { Status = ResponseCodes.FileUploadFailed });
		}

		public AttachmentFileTypes GetContainerFileType(string extension)
		{
			if (extension is null)
				return AttachmentFileTypes.Other;
			switch (extension.ToLower())
			{
				case ".png":
				case ".jpg":
				case ".jpeg":
				case ".gif":
					return AttachmentFileTypes.Images;
				case ".mp4":
				case ".wmv":
				case ".avi":
				case ".mov":
				case ".3gp":
				case ".flv":
				case ".m3u8":
				case ".ts":
					return AttachmentFileTypes.Videos;
				default:
					return AttachmentFileTypes.Other;
			}
		}
		public static string GetMimeType(string fileName)
		{
			var provider = new FileExtensionContentTypeProvider();
			string contentType;
			if (!provider.TryGetContentType(fileName, out contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;
		}

		private string GetContainerName(AttachmentFileTypes attachmentFileType)
		{
			return attachmentFileType == AttachmentFileTypes.Images
						? _appSetting.ImageContainer
						: attachmentFileType == AttachmentFileTypes.Videos
							? _appSetting.VideoContainer
							: _appSetting.GeneralContainer;
		}

		[Route("createJob")]
		[HttpPost]
		public async Task<JsonResult> CreateJob([FromBody]JobViewModel jobViewModel)
		{

		bool status = await _jobService.CreateJobAsync(jobViewModel);

			if (status) {
				return new JsonResult(new { Status = ResponseCodes.AllSuccess });
			}
			return new JsonResult(new { Status = ResponseCodes.AllFail });
		}

		[Route("GetJobList")]
		[HttpGet]
		public async Task<JsonResult> GetJobList(string userId, JobworkflowStatus workFlowStatus)
		{

			var status = await _jobService.GetJobListJobAsync(userId, workFlowStatus);

			
			return new JsonResult(status);
		}

		[Route("GetTimelineJobList")]
		[HttpGet]
		public async Task<JsonResult> GetTimelineJobList(string userId)
		{

			var status = await _jobService.GetTimeLineJobListJobAsync(userId);


			return new JsonResult(status);
		}
	}
}