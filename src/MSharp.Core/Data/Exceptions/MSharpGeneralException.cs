using System;

namespace MSharp.Core.Data.Exceptions
{
	public class MSharpGeneralException : ApplicationException
	{
		public MSharpGeneralException() { }

		public MSharpGeneralException(string message) : base(message) { }

		public MSharpGeneralException(string message, Exception innerException) : base(message, innerException) { }
	}
}
