using System;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace Andrei15193.Edesia.DataAccess.Azure
{
	public class ImageUploader
		: IImageUploader
	{
		public ImageUploader(string connectionStringCloudSettingName)
		{
			if (connectionStringCloudSettingName == null)
				throw new ArgumentNullException("connectionStringCloudSettingName");
			if (string.IsNullOrWhiteSpace(connectionStringCloudSettingName))
				throw new ArgumentException("Cannot be empty or whitespace!", "connectionStringCloudSettingName");

			_connectionStringCloudSettingName = connectionStringCloudSettingName;
		}

		public void Upload(Stream imageStream, string imageName, out Uri imageUri)
		{
			if (imageStream == null)
				throw new ArgumentNullException("imageStream");

			if (imageName == null)
				throw new ArgumentNullException("imageName");
			if (string.IsNullOrWhiteSpace(imageName))
				throw new ArgumentException("Cannot be empty or whitespace!", "imageName");

			CloudBlockBlob imageBlockBlob = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
															   .CreateCloudBlobClient()
															   .GetContainerReference("public")
															   .GetBlockBlobReference("Edesia/images/" + imageName);
			imageBlockBlob.UploadFromStream(imageStream);

			imageUri = imageBlockBlob.Uri;
		}

		private readonly string _connectionStringCloudSettingName;
	}
}