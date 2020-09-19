using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Seeker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Seeker.Constants;
using Microsoft.AspNetCore.Http;

namespace Seeker.Services
{
	public class AzureBlobService: IAzureBlobService
	{
		private readonly BlobServiceClient _blobServiceClient;

		public AzureBlobService(BlobServiceClient blobServiceClient)
		{
			_blobServiceClient = blobServiceClient;
		}

		public async Task<byte[]> DownLoadFileToByteArray(string connectionString, string containerName, string fileName)
		{
			try
			{
				var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
				var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
				var cloudBlob = cloudBlobContainer.GetBlobReference(fileName);
				await cloudBlob.FetchAttributesAsync();
				var byteData = new byte[cloudBlob.Properties.Length];
				await cloudBlob.DownloadToByteArrayAsync(byteData, 0);
				return byteData;
			}
			catch (Exception e)
			{

				throw e;
			}
		
		}

		public CloudBlockBlob GetBlob(string connectionString, string containerName, string fileName)
		{
			var storageAccount = CloudStorageAccount.Parse(connectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containerName);
			container.CreateIfNotExistsAsync();
			container.SetPermissionsAsync(
				new BlobContainerPermissions
				{
					PublicAccess = BlobContainerPublicAccessType.Blob
				});
			return container.GetBlockBlobReference(fileName);
		}

		public async Task<System.Uri> UploadFileToBlob(string connectionString, string containerName, IFormFile file)
		{
			CloudStorageAccount storageAccount = null;
			if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
			{
				var blobClient = storageAccount.CreateCloudBlobClient();
				var container = blobClient.GetContainerReference(containerName);
				await container.CreateIfNotExistsAsync();
				await container.SetPermissionsAsync(new BlobContainerPermissions
				{
					PublicAccess = BlobContainerPublicAccessType.Blob
				});
				var blob = container.GetBlockBlobReference(file.FileName);

				await blob.UploadFromStreamAsync(file.OpenReadStream());
				return blob.Uri;
			}
			return null;
		}


		public static AttachmentFileTypes GetFileType(string extension)
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
	}
}
