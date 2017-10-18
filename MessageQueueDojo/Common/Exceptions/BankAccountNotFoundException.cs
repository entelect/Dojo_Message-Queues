using System;

namespace Common
{
	class BankAccountNotFoundException : Exception
	{
		public BankAccountNotFoundException() : base("Bank Account Not Found")
		{
		}
	}
}
