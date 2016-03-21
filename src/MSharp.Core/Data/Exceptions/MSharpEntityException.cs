using System;

namespace MSharp.Core.Data.Exceptions
{
	public class MSharpEntityException : MSharpGeneralException
	{
		public MSharpEntityException() { }

		public MSharpEntityException(string message) : base(message) { }

		public MSharpEntityException(string message, Exception innerException) : base(message, innerException) { }
	}
}
