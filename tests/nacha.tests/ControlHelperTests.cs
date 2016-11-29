using Nacha.Enums;
using Nacha.Helpers;
using Nacha.Models;
using Xunit;

namespace Nacha.Tests
{
    public class ControlHelperTests
    {
        [Theory]
        [InlineData((short)TransactionCodeEnum.AutomatedDepositChecking)]
        [InlineData((short)TransactionCodeEnum.AutomatedDepositSavings)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfCheckingCredit)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfSavingsCredit)]
        public void ControlHelper_GetCreditAmount(short transactionCode)
        {
            var _entry = new Entry(Constants.StandardEntryClassCodeConstant.CCD)
            {
                Amount = 6543,
                TransactionCode = transactionCode
            };
            Assert.Equal(_entry.Amount, ControlHelper.GetCreditAmount(_entry));
        }

        [Theory]
        [InlineData((short)TransactionCodeEnum.AutomatedPaymentChecking)]
        [InlineData((short)TransactionCodeEnum.AutomatedPaymentSavings)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfCheckingDebit)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfSavingsDebit)]
        public void ControlHelper_GetCreditAmountReturns0(short transactionCode)
        {
            var _entry = new Entry(Constants.StandardEntryClassCodeConstant.CCD)
            {
                Amount = 6543,
                TransactionCode = transactionCode
            };
            Assert.Equal(0, ControlHelper.GetCreditAmount(_entry));
        }
        

        [Theory]
        [InlineData((short)TransactionCodeEnum.AutomatedPaymentChecking)]
        [InlineData((short)TransactionCodeEnum.AutomatedPaymentSavings)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfCheckingDebit)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfSavingsDebit)]
        public void ControlHelper_GetDebitAmount(short transactionCode)
        {
            var _entry = new Entry(Constants.StandardEntryClassCodeConstant.CCD)
            {
                Amount = 6543,
                TransactionCode = transactionCode
            };
            Assert.Equal(_entry.Amount, ControlHelper.GetDebitAmount(_entry));
        }

        [Theory]
        [InlineData((short)TransactionCodeEnum.AutomatedDepositChecking)]
        [InlineData((short)TransactionCodeEnum.AutomatedDepositSavings)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfCheckingCredit)]
        [InlineData((short)TransactionCodeEnum.PrenoteOfSavingsCredit)]
        public void ControlHelper_GetDebitAmountReturns0(short transactionCode)
        {
            var _entry = new Entry(Constants.StandardEntryClassCodeConstant.CCD)
            {
                Amount = 6543,
                TransactionCode = transactionCode
            };
            Assert.Equal(0, ControlHelper.GetDebitAmount(_entry));
        }

        [Theory]
        [InlineData("00022266", 0, 22266)]
        [InlineData("00022266", 33333333, 33355599)]
        [InlineData("99999999", 9999999999, 99999998)]
        public void ControlHelper_GetEntryHash(string rdfiRtn, long currentHash, long expected)
        {
            var _hash = ControlHelper.GetEntryHash(rdfiRtn, currentHash);
            Assert.Equal(expected, _hash);
        }
    }
}
