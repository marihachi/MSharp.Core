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
		/// ファイル名を取得します。
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// イメージデータを取得します。
		/// </summary>
		public Image Data { get; private set; }
	}
}
