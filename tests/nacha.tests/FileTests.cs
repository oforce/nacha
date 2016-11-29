using Nacha.Constants;
using Nacha.Enums;
using Nacha.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nacha.Tests
{
    public class FileTests
    {
        File _sut;

        public FileTests()
        {
            _sut = new File();
        }

        [Fact]
        public void File_AddBatchCreatesOne()
        {            
            _sut.AddBatch(new Batch());
            Assert.Equal(1, _sut.Batches.Count);
        }

        [Fact]
        public void File_BatchesListIsLimitedTo999999()
        {
            var _exception = Record.Exception(() =>
            {
                for (int i = 1; i <= 1000000; i++)
                {
                    _sut.AddBatch(new Batch());
                }
            }
            );

            Assert.NotNull(_exception);
            Assert.Equal(ExceptionConstants.File_BatchMaximumExceeded, _exception.Message);
        }

        [Fact]
        public void File_BatchNumberIncremented()
        {
            for (int i = 1; i <= 99; i++)
            {
                _sut.AddBatch(new Batch());
                Assert.Equal(i, _sut.Batches[i - 1].Header.BatchNumber);
            }
        }

        [Fact]
        public void File_ValidationRaisesForMissingRequiredProperties()
        {
            var _exception = Record.Exception(() => _sut.Validate());
            Assert.NotNull(_exception);
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(95, 10)]
        [InlineData(332, 34)]
        public void File_GetBlockCount(int records, int expectedBlockCount)
        {
            Assert.Equal(expectedBlockCount, _sut.GetBlockCount(records));
        }

        [Fact]
        public void File_TestHarness()
        {
            var _output = new StringBuilder();

            var _file = new File();
            _file.Header.Id = "A";
            _file.Header.ImmediateDestination = " 091000019";
            _file.Header.ImmediateDestinationName = "WELLS FARGO";
            _file.Header.ImmediateOrigin = "9876543210";
            _file.Header.ImmediateOriginName = "Contractor Mgmt Srvcs";
            _file.Header.ReferenceCode = "";

            _file.AddBatch(new Batch());

            _file.Batches[0].Header.CompanyName = "Contractor Mgmt";
            _file.Batches[0].Header.CompanyIdentification = "9876543210";
            _file.Batches[0].Header.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD;
            _file.Batches[0].Header.CompanyEntryDescription = "SETTLEMENT";
            _file.Batches[0].Header.EffectiveEntryDate = DateTime.Now;
            _file.Batches[0].Header.OriginatingDfi = "09100001";

            _file.Batches[0].AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                Amount = 99988,
                AddendaRecordIndicator = (short)AddendaRecordIndicatorEnum.NotIncluded,
                CheckDigit = "8",
                RdfiAccountNumber = "44556677",
                DiscretionaryData = "",
                ReceiverIdentificationNumber = "ID#Something1",
                RdfiRtn = "99887766",
                ReceiverName = "Receiving Company Name1",
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking
            });

            _file.Batches[0].AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                Amount = 1250233,
                AddendaRecordIndicator = (short)AddendaRecordIndicatorEnum.NotIncluded,
                CheckDigit = "8",
                RdfiAccountNumber = "11223344",
                DiscretionaryData = "",
                ReceiverIdentificationNumber = "ID#Something2",
                RdfiRtn = "99887766",
                ReceiverName = "Receiving Company Name2",
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositSavings
            });

            var _fileHeaders = new List<FileHeader>();
            _fileHeaders.Add(_file.Header);
            var _engine = new FileHelpers.FileHelperEngine<FileHeader>();
            _output.Append(_engine.WriteString(_fileHeaders));

            foreach (var batch in _file.Batches)
            {
                var _batchHeaders = new List<BatchHeader>();
                _batchHeaders.Add(batch.Header);
                var _batchEngine = new FileHelpers.FileHelperEngine<BatchHeader>();
                _output.Append(_batchEngine.WriteString(_batchHeaders));

                var _entries = new List<Entry>();
                foreach (var entry in batch.Entries)
                {                    
                    _entries.Add(entry);
                }

                var _entryEngine = new FileHelpers.FileHelperEngine<Entry>();
                _output.Append(_entryEngine.WriteString(_entries));

                var _batchControls = new List<BatchControl>();
                _batchControls.Add(batch.Control);
                var _batchControlEngine = new FileHelpers.FileHelperEngine<BatchControl>();
                _output.Append(_batchControlEngine.WriteString(_batchControls));

            }

            var _fileControls = new List<FileControl>();
            _fileControls.Add(_file.Control);
            var _fileControlEngine = new FileHelpers.FileHelperEngine<FileControl>();
            _output.Append(_fileControlEngine.WriteString(_fileControls));
            // do we want our own "checksum"

            System.IO.File.WriteAllText(@"C:\NachaTest.txt", _output.ToString());
        }
    }
}
