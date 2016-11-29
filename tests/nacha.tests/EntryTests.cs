using Nacha.Constants;
using Nacha.Enums;
using Nacha.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nacha.Tests
{
    public class EntryTests
    {
        Entry _sut;

        public EntryTests()
        {
            _sut = new Entry(StandardEntryClassCodeConstant.CCD);
        }

        [Fact]
        public void Entry_DefaultValuesAreCorrect()
        { 
            Assert.Equal((short)RecordTypeEnum.EntryDetail, _sut.RecordType);
            Assert.Equal((short)AddendaRecordIndicatorEnum.NotIncluded, _sut.AddendaRecordIndicator);
            Assert.Equal(StandardEntryClassCodeConstant.CCD, _sut.StandardEntryClassCode);
        }

        [Fact]
        public void Entry_InvalidStandardEntryClassCode()
        {
            var _exception = Record.Exception(() => _sut.StandardEntryClassCode = "bad value");
            Assert.NotNull(_exception);
            var _expected = ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.Entry_StandardEntryClassCodeInvalid;
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void Entry_ValidStandardEntryClassCode()
        {
            var _exception = Record.Exception(() => _sut.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD);
            Assert.Equal(null, _exception);
        }

        [Fact]
        public void Entry_InvalidTransactionCode()
        {
            var _exception = Record.Exception(() => _sut.TransactionCode = -999);
            Assert.NotNull(_exception);
            var _expected = ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.Entry_TransactionCodeInvalid;
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void Entry_ValidTransactionCode()
        {
            var _exception = Record.Exception(() => _sut.TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking);
            Assert.Equal(null, _exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData("AAAABBBB")]
        [InlineData("0123456789")]
        public void Entry_InvalidRdfiRtn(string value)
        {
            var _exception = Record.Exception(() => _sut.RdfiRtn = value);
            Assert.NotNull(_exception);
            var _expected = ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.Entry_RdfiRtnInvalid;
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void Entry_ValidRdfiRtn()
        {
            var _exception = Record.Exception(() => _sut.RdfiRtn = "01234567");
            Assert.Equal(null, _exception);
        }
        
        [Fact]
        public void Entry_InvalidAmount()
        {
            var _exception = Record.Exception(() => _sut.Amount = -9);
            Assert.Equal(ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.Entry_AmountInvalid,
                _exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9223372036854775807)] 
        public void Entry_ValidAmount(long value)
        {
            _sut.Amount = value;
        }

        [Fact]
        public void Entry_InvalidAddendaRecordIndicator()
        {
            var _exception = Record.Exception(() => _sut.AddendaRecordIndicator = -999);
            Assert.NotNull(_exception);
            var _expected = ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.Entry_AddendaRecordIndicatorInvalid;
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void Entry_ValidAddendaRecordIndicator()
        {
             _sut.AddendaRecordIndicator =(short)AddendaRecordIndicatorEnum.Included;
        }

        [Fact]
        public void Entry_GetCheckDigitThrowsForInvalidLength()
        { 
            var _exception = Record.Exception(() => _sut.GetCheckDigit("123456789"));
            Assert.Equal(ExceptionConstants.DotNet_ArgumentOutOfRangeException + 
                ExceptionConstants.Shared_RoutingNumberInvalid, 
                _exception.Message);
        }

        [Theory]
        [InlineData("07640125", "1")]
        [InlineData("32517075", "4")]
        [InlineData("24407732", "3")]
        [InlineData("12100040", "0")]
        public void Entry_GetCheckDigitSucceeds(string rtn, string expected)
        { 
            var _result = _sut.GetCheckDigit(rtn);
            Assert.Equal(expected, _result);
        }

        [Fact]
        public void Entry_TestRowCreation()
        {
            var _suts = new List<Entry>();
            var _suti = new Entry(StandardEntryClassCodeConstant.CCD)
            {
                Amount = 99988,
                AddendaRecordIndicator = (short)AddendaRecordIndicatorEnum.NotIncluded,
                CheckDigit = "8",
                RdfiAccountNumber = "44556677",
                DiscretionaryData = "",
                ReceiverIdentificationNumber = "ID#Something",
                RdfiRtn = "99887766",
                ReceiverName = "Receiving Company Name",
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking

            };
            _suts.Add(_suti);

            var _engine = new FileHelpers.FileHelperEngine<Entry>();
            var _test = _engine.WriteString(_suts);
            var _expected = "62299887766244556677              99988ID#Something   Receiving Company Name  0               \r\n";
            Assert.Equal(_expected, _test);
        }
    }
}
