using Nacha.Constants;
using Nacha.Enums;
using Nacha.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace Nacha.Tests
{
    public class BatchControlTests
    {
        BatchControl _sut;
        public BatchControlTests()
        {
            _sut = new BatchControl();
        }

        [Fact]
        public void BatchControl_DefaultValuesAreCorrect()
        {            
            Assert.Equal((short)RecordTypeEnum.BatchControl, _sut.RecordType);
            Assert.Equal(BatchServiceClassCodeConstant.Mixed, _sut.ServiceClassCode);
        }

        [Fact]
        public void BatchControl_InvalidServiceClassCode()
        {
            var _exception = Record.Exception(() => _sut.ServiceClassCode = 9999);
            Assert.IsType(typeof(ArgumentOutOfRangeException), _exception);
        }

        [Fact]
        public void BatchControl_ValidServiceClassCode()
        {
            var _exception = Record.Exception(() => _sut.ServiceClassCode = 
                BatchServiceClassCodeConstant.Credits);
            Assert.Equal(null, _exception);
        }



        [Fact]
        public void BatchControl_CompanyIdentificationRaisesChangedEvent()
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
        public void BatchControl_TestRowCreation()
        {
            var _suts = new List<BatchControl>();
            var _suti = new BatchControl
            {
                CompanyIdentification = "9876543210",
                OriginatingDfi = "99889988",
                TotalCreditAmount = 9223372036854775807,
                TotalDebitAmount = 9223372036854775807
            };
            _suts.Add(_suti);

            var _engine = new FileHelpers.FileHelperEngine<BatchControl>();
            var _test = _engine.WriteString(_suts);
            var _expected = "820000000000000000009223372036859223372036859876543210                         998899880000000\r\n";
            Assert.Equal(_expected, _test);
        }
    }
}
