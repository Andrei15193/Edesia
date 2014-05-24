using System;
using System.IO;
namespace Andrei15193.Edesia.DataAccess.Local
{
	public class ImageUploader
		: IImageUploader
	{
		public void Upload(Stream imageStream, string imageName, out Uri imageUri)
		{
			if (imageStream == null)
				throw new ArgumentNullException("imageStream");

			if (imageName == null)
				throw new ArgumentNullException("imageName");
			if (string.IsNullOrWhiteSpace(imageName))
				throw new ArgumentException("Cannot be empty or whitespace!", "imageName");

			DirectoryInfo applicationDirectoryInfo = new DirectoryInfo(@"C:\Users\Andrei\Documents\");
			DirectoryInfo imagesDirectoryInfo = applicationDirectoryInfo.CreateSubdirectory(".Images");

			using (Stream stream = File.Open(Path.Combine(imagesDirectoryInfo.FullName, imageName), FileMode.Create, FileAccess.Write, FileShare.None))
				imageStream.CopyTo(stream);

			imageUri = new Uri(Path.Combine(imagesDirectoryInfo.FullName, imageName), UriKind.Absolute);
		}
	}
}