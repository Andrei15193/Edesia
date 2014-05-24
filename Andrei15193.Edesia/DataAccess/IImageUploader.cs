using System;
using System.IO;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IImageUploader
	{
		void Upload(Stream imageStream, string imageName, out Uri imageUri);
	}
}