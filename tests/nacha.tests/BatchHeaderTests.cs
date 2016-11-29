using Nacha.Constants;
using Nacha.Enums;
using Nacha.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace Nacha.Tests
{
    public class BatchHeaderTests
    {
        BatchHeader _sut;

        public BatchHeaderTests()
        {
            _sut = new BatchHeader();
        }

        [Fact]
        public void BatchHeader_DefaultValuesAreCorrect()
        {
            Assert.Equal((short)RecordTypeEnum.BatchHeader, _sut.RecordType);
            Assert.Equal(BatchHeaderOriginatorStatusCodeConstant.NonFederalGovernment, _sut.OriginatorStatusCode);
            Assert.Equal(BatchServiceClassCodeConstant.Mixed, _sut.ServiceClassCode);
        }

        [Fact]
        public void BatchHeader_InvalidStandardEntryClassCode()
        {
            var _exception = Record.Exception(() => _sut.StandardEntryClassCode = "bad value");
            Assert.NotNull(_exception);
            var _expected = ExceptionConstants.DotNet_ArgumentOutOfRangeException +
                ExceptionConstants.BatchHeader_StandardEntryClassCodeInvalid;
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void BatchHeader_ValidStandardEntryClassCode()
        {
            var _exception = Record.Exception(() => _sut.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD);
            Assert.Equal(null, _exception);
        }

        [Fact]
        public void BatchHeader_InvalidServiceClassCode()
        {
            var _exception = Record.Exception(() => _sut.ServiceClassCode = 9999);
            Assert.IsType(typeof(ArgumentOutOfRangeException), _exception);
        }

        [Fact]
        public void BatchHeader_ValidServiceClassCode()
        {
            var _exception = Record.Exception(() => _sut.ServiceClassCode = BatchServiceClassCodeConstant.Credits);
            Assert.Equal(null, _exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("BB")]
        [InlineData("CCCCCCCCCC")]
        public void BatchHeader_InvalidOriginatingDfi(string value)
        {
            var _exception = Record.Exception(() => _sut.OriginatingDfi = value);
            Assert.Equal(ExceptionConstants.BatchHeader_OriginatingDfiInvalid, _exception.Message);
        }

        [Fact]
        public void BatchHeader_ValidOriginatingDfi()
        {
            var _exception = Record.Exception(() => _sut.OriginatingDfi = "01234567");
            Assert.Equal(null, _exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("BB")]
        [InlineData("CCCCCCCCCC")]
        public void BatchHeader_InvalidCompanyIdentification(string value)
        {
            var _exception = Record.Exception(() => _sut.CompanyIdentification = value);
            Assert.Equal(ExceptionConstants.BatchHeader_CompanyIdentificationInvalid, _exception.Message);
        }

        [Fact]
        public void BatchHeader_ValidCompanyIdentification()
        {
            var _exception = Record.Exception(() => _sut.CompanyIdentification = "0123456789");
            Assert.Equal(null, _exception);
        }

        [Fact]
        public void BatchHeader_CompanyIdentificationRaisesChangedEvent()
        {
            List<string> _events = new List<string>();

            _sut.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                _events.Add(e.PropertyName);
            };

            _sut.CompanyIdentification = "1111222233";
            Assert.Equal(1, _events.Count);
            Assert.Equal("CompanyIdentification", _events[0]);
        }

        [Fact]
        public void BatchHeader_InvalidOriginatorStatusCode()
        {
            var _exception = Record.Exception(() => _sut.OriginatorStatusCode = 9999);
            Assert.IsType(typeof(ArgumentOutOfRangeException), _exception);
        }

        [Fact]
        public void BatchHeader_ValidOriginatorStatusCode()
        {
            var _exception = Record.Exception(() => _sut.OriginatorStatusCode = BatchHeaderOriginatorStatusCodeConstant.NonFederalGovernment);
            Assert.Equal(null, _exception);
        }

        [Fact]
        public void BatchHeader_TestRowCreation()
        {
            var _suts = new List<BatchHeader>();
            var _suti = new BatchHeader
            {
                ServiceClassCode = BatchServiceClassCodeConstant.Credits,
                CompanyName = "CONTRACTOR MGMT",
                CompanyDiscretionaryData = "",
                CompanyIdentification = "9876543210",
                StandardEntryClassCode = StandardEntryClassCodeConstant.PPD,
                CompanyEntryDescription = "SETTLEMENT",
                CompanyDescriptiveDate = "",
                EffectiveEntryDate = new DateTime(2016, 8, 11),
                OriginatorStatusCode = BatchHeaderOriginatorStatusCodeConstant.NonFederalGovernment,
                OriginatingDfi = "99889988"
            };
            _suts.Add(_suti);

            var _engine = new FileHelpers.FileHelperEngine<BatchHeader>();
            var _test = _engine.WriteString(_suts);
            var _expected = "5220CONTRACTOR MGMT                     9876543210PPDSETTLEMENT      160811   1998899880000000\r\n";
            Assert.Equal(_expected, _test);
        }
    }
}
