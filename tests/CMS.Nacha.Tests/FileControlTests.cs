using CMS.Nacha.Enums;
using CMS.Nacha.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMS.Nacha.Tests
{
    public class FileControlTests
    {
        [Fact]
        public void FileControl_DefaultValuesAreCorrect()
        {
            var _sut = new FileControl();
            Assert.Equal((short)RecordTypeEnum.FileControl, _sut.RecordType);
        }

        [Fact]
        public void FileControl_Validate()
        {
            var _sut = new FileControl();
            var _exception = Record.Exception(() => _sut.Validate());
            Assert.IsType(typeof(ArgumentException), _exception);
        }

        [Fact]
        public void FileControl_TestRowCreation()
        {
            var _suts = new List<FileControl>();
            var _sut = new FileControl();
            _suts.Add(_sut);

            var _engine = new FileHelpers.FileHelperEngine<FileControl>();
            var _test = _engine.WriteString(_suts);
            var _expected = "9000000000000000000000000000000000000000000000000000000                                       \r\n";
            Assert.Equal(_expected, _test);
        }
    }
}
