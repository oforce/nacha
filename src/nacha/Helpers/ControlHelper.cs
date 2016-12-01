using CMS.Nacha.Enums;
using CMS.Nacha.Models;
using System;

namespace CMS.Nacha.Helpers
{
    public static class ControlHelper
    {
        public static long GetCreditAmount(Entry entry)
        {
            switch (entry.TransactionCode)
            {
                case (short)TransactionCodeEnum.AutomatedDepositChecking:
                case (short)TransactionCodeEnum.AutomatedDepositSavings:
                case (short)TransactionCodeEnum.PrenoteOfCheckingCredit:
                case (short)TransactionCodeEnum.PrenoteOfSavingsCredit:
                    return entry.Amount;
                
                default:
                    return 0;
            }
        }

        public static long GetDebitAmount(Entry entry)
        {
            switch (entry.TransactionCode)
            {
                case (short)TransactionCodeEnum.AutomatedPaymentChecking:
                case (short)TransactionCodeEnum.AutomatedPaymentSavings:
                case (short)TransactionCodeEnum.PrenoteOfCheckingDebit:
                case (short)TransactionCodeEnum.PrenoteOfSavingsDebit:
                    return entry.Amount;

                default:
                    return 0;
            }
        }

        public static long GetEntryHash(string rdfiRtn, long currentHash)
        {
            /* The 10-character entry hash is the sum of the eight- digit receiving DFI 
             * routing/transit numbers in entry detail records in the batch. 
             * Add leading zeros as needed and ignore the overflow out of the 
             * high order (leftmost) position. 
             * Note Addenda records are not included in the entry hash. */

            var _newHash = currentHash + Convert.ToInt64(rdfiRtn);

            var _length = _newHash.ToString().Length;

            if (_length > 10)
            {
                _newHash = Convert.ToInt64(_newHash.ToString().Substring(_length - 10, 10));
            }

            return _newHash;
        }
    }
}
