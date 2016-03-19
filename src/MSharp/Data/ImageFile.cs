using System.Drawing;
using System.IO;

namespace MSharp.Data
{
	/// <summary>
	/// 画像ファイルを表します。
	/// </summary>
	public class ImageFile : IFile
	{
		public ImageFile(string filePath)
		{
			FileName = Path.GetFileName(filePath);
			Data = Image.FromFile(filePath);
		}

		public ImageFile(string fileName, Image data)
		{
			FileName = fileName;
			Data = data;
		}

		/// <summary>
		/// 
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Image Data { get; set; }
	}
}
