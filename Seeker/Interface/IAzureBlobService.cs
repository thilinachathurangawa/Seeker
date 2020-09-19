using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Interface
{
	public interface IAzureBlobService
	{
		//Task<string> ReadFileFromBlobAsync(string storageConnectionString, string containerName, string blobName);
		Task<byte[]> DownLoadFileToByteArray(string connectionString, string containerName, string fileName);
		CloudBlockBlob GetBlob(string connectionString, string containerName, string fileName);
		Task<System.Uri> UploadFileToBlob(string connectionString, string containerName, IFormFile file);
		//AttachmentFileTypes GetFileType(string extension);
		//Task<string> SaveAzureFile(GeneralFileViewModel file, Stream stream, string connectionString, string containerName);
		//Task<string> CopyBlob(GeneralFileViewModel file, string newBlobName, string connectionString, string containerName);
		//Task<CloudBlockBlob> GetBlob(string fileName, string connectionString, string containerName);
	}
}
