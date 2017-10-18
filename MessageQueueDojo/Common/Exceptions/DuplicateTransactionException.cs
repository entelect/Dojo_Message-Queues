using System;

namespace Common
{
	public class DuplicateTransactionException : Exception
	{
		public DuplicateTransactionException(): base("Duplicate Transaction")
		{
		}
	}
}
