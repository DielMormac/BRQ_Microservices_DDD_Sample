using System;
using System.Collections.Generic;
using System.Text;

namespace Transaction.Domain.Enum
{
    /// <summary>
    /// Transaction Status Enumerator
    /// </summary>
    public enum TransactionType
    {
        Transfer,
        Withdraw,
        Deposit
    }
}
