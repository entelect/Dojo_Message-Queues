using System;

namespace Common
{
	public class InsufficientFundsException: Exception
	{
		public InsufficientFundsException(): base("Insufficient Funds")
		{
		}
	}
}
