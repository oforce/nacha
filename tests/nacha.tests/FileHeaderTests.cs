using Nacha.Constants;
using Nacha.Enums;
using Nacha.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nacha.Tests
{
    public class FileHeaderTests
    {
        FileHeader _sut;

        public FileHeaderTests()
        {
            _sut = new FileHeader();
        }

        [Fact]
        public void FileHeader_DefaultValuesAreCorrect()
        {
            Assert.Equal((short)RecordTypeEnum.FileHeader, _sut.RecordType);
            
            Assert.Equal(FileHeaderConstants.BlockingFactor, _sut.BlockingFactor);
            Assert.Equal(FileHeaderConstants.FormatCode, _sut.FormatCode);
            Assert.Equal(FileHeaderConstants.PriorityCode, _sut.PriorityCode);
            Assert.Equal(FileHeaderConstants.RecordSize, _sut.RecordSize);
            Assert.Equal(_sut.CreationDate, _sut.CreationTime);
        }

        [Fact]
        public void FileHeader_ValidateRaisesForMissingValues()
        {
            var _exception = Record.Exception(() => _sut.Validate());
            Assert.IsType(typeof(ArgumentException), _exception);
        }

        [Fact]
        public void FileHeader_ValidateSucceeds()
        {
            _sut.ImmediateDestination = " 123456789";
            _sut.ImmediateOrigin = "1234567890";
            _sut.Id = "A";
            _sut.ImmediateDestinationName = "Bank Name";
            _sut.ImmediateOriginName = "Our Company Name";
            _sut.Validate();
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData("1234567890")]
        [InlineData(" ABCDEFGHI")]
        public void FileHeader_InvalidImmediateDestinationRaises(string testValue)
        {
            var _exception = Record.Exception(() => _sut.ImmediateDestination = testValue);

            Assert.NotNull(_exception);
            Assert.Equal(ExceptionConstants.FileHeader_ImmediateDestinationInvalid, _exception.Message);
        }

        [Fact]
        public void FileHeader_ValidImmediateDestinationSucceeds()
        {
            _sut.ImmediateDestination = " 123456789";
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData(" 123456789")]
        [InlineData(" ABCDEFGHI")]
        public void FileHeader_InvalidImmediateOriginRaises(string testValue)
        {
            var _exception = Record.Exception(() => _sut.ImmediateOrigin = testValue);

            Assert.NotNull(_exception);
            Assert.Equal(ExceptionConstants.FileHeader_ImmediateOriginInvalid, _exception.Message);
        }

        [Fact]
        public void FileHeader_ValidImmediateOriginSucceeds()
        {
            _sut.ImmediateOrigin = "1234567890";
        }

        [Fact]
        public void FileHeader_CreationDateCascadesToCreationTime()
        {
            var _newDate = new DateTime(2016, 10, 31);
            Assert.NotEqual(_newDate, _sut.CreationTime);
            _sut.CreationDate = _newDate;
            Assert.Equal(_newDate, _sut.CreationTime);
        }

        [Fact]
        public void FileHeader_CreationTimeCascadesToCreationDate()
        {
            var _newDate = new DateTime(2016, 10, 31);
            Assert.NotEqual(_newDate, _sut.CreationDate);
            _sut.CreationTime = _newDate;
            Assert.Equal(_newDate, _sut.CreationDate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("BB")]
        public void FileHeader_InvalidIdRaises(string testValue)
        {
            var _exception = Record.Exception(() => _sut.Id = testValue);

            Assert.NotNull(_exception);
            Assert.Equal(ExceptionConstants.FileHeader_IdInvalid, _exception.Message);
        }

        [Fact]
        public void FileHeader_ValidIdSucceeds()
        {
             _sut.Id = "A";
        }

        [Fact]
        public void FileHeader_TestRowCreation()
        {
            var _suts = new List<FileHeader>();
            var _suti = new FileHeader
            {
                ImmediateOriginName = "Test Company Name",
                ImmediateDestinationName = "Bank of America DAL",
                CreationDate = DateTime.Now,
                CreationTime = DateTime.Now,
                Id = "A",  
                ImmediateDestination = " 123456789",
                ImmediateOrigin = "9996663331",
                ReferenceCode = "ref#"
            };
            _suts.Add(_suti);

            var _engine = new FileHelpers.FileHelperEngine<FileHeader>();
            var _test = _engine.WriteString(_suts);
            var _timestamp = _suti.CreationDate.ToString("yyMMdd") + _suti.CreationTime.ToString("HHmm");
            var _expected = "101 1234567899996663331" + _timestamp + "A094101Bank of America DAL    Test Company Name      ref#    \r\n";
            Assert.Equal(_expected, _test); 
        }
    }
}
