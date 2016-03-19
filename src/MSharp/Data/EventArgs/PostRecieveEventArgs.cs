using MSharp.Data.Entity;

namespace MSharp.Data.EventArgs
{
	public class PostRecieveEventArgs : System.EventArgs
	{
		public PostRecieveEventArgs(PostEntity post)
		{
			Data = post;
		}

		public PostEntity Data { get; private set; }
	}
}
