using System;

namespace MSharp.Core.Data.Exceptions
{
	public class MSharpApiException : MSharpGeneralException
	{
		public MSharpApiException() { }

		public MSharpApiException(string message) : base(message) { }

		public MSharpApiException(string message, Exception innerException) : base(message, innerException) { }
	}
}
