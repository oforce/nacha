using CMS.Nacha.Constants;
using CMS.Nacha.Enums;
using CMS.Nacha.Models;
using System;
using Xunit;

namespace CMS.Nacha.Tests
{
    public class BatchTests
    {
        Batch _sut;

        public BatchTests()
        {
            _sut = new Batch();
        }

        [Fact]
        public void Batch_EntryMissingValuesThrows()
        {
            _sut.Header.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD;

            var _exception = Record.Exception(() =>
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD))
            );

            Assert.NotNull(_exception);
        }

        [Fact]
        public void Batch_StandardEntryClassCodeSetByFirstAddEntry()
        {
            var _entry = new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                RdfiRtn = "33333333",
                RdfiAccountNumber = "FakeAccount"
            };
            _sut.AddEntry(_entry);
            Assert.Equal(_entry.StandardEntryClassCode, _sut.Header.StandardEntryClassCode);
        }

        [Fact]
        public void Batch_StandardEntryClassCodeMismatchesThrows()
        {
            _sut.Header.StandardEntryClassCode = StandardEntryClassCodeConstant.PPD;

            var _exception = Record.Exception(() =>
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD))
            );

            Assert.NotNull(_exception);
            var _expected = String.Format(ExceptionConstants.Batch_StandardEntryClassCodeMismatched,
                    StandardEntryClassCodeConstant.PPD,
                    StandardEntryClassCodeConstant.CCD);
            Assert.Equal(_expected, _exception.Message);
        }

        [Fact]
        public void Batch_StandardEntryClassCodeMatchDoesNotThrow()
        {
            _sut.Header.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD;

            var _exception = Record.Exception(() =>
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                    RdfiRtn = "33333333",
                    RdfiAccountNumber = "FakeAccount"
                }
                )
            );

            Assert.Null(_exception);
        }

        [Fact]
        public void Batch_EntryListIsLimitedTo9999999()
        {
            _sut.Header.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD;

            var _exception = Record.Exception(() =>
            {
                for (int i = 1; i <= 10000000; i++)
                {
                    _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD) {
                        TransactionCode = (short)TransactionCodeEnum.AutomatedPaymentChecking,
                        RdfiRtn = "33333333",
                        RdfiAccountNumber = "FakeAccount"
                    });
                }
            }
            );

            Assert.NotNull(_exception);
            Assert.Equal(ExceptionConstants.Batch_EntryMaximumExceeded, _exception.Message);
        }

        [Fact]
        public void Batch_EntryNumberIncremented()
        {
            for (int i = 1; i <= 99; i++)
            {
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedPaymentChecking,
                    RdfiRtn = "33333333",
                    RdfiAccountNumber = "FakeAccount"
                });
                Assert.Equal<int>(i, _sut.Control.EntryAndAddendaCount);
            }
        }

        [Fact]
        public void Batch_CreditDebitTotalsCorrect()
        {
            // first add some credits
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                Amount = 999,
                RdfiRtn = "77665544",
                RdfiAccountNumber = "FakeAccount"
            });
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositSavings,
                Amount = 1233,
                RdfiRtn = "77665544",
                RdfiAccountNumber = "FakeAccount"
            });
            Assert.Equal<long>(2232, _sut.Control.TotalCreditAmount);
            Assert.Equal<long>(0, _sut.Control.TotalDebitAmount);

            // now add some debits
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedPaymentChecking,
                Amount = 454,
                RdfiRtn = "77665544",
                RdfiAccountNumber = "FakeAccount"
            });
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedPaymentSavings,
                Amount = 711,
                RdfiRtn = "77665544",
                RdfiAccountNumber = "FakeAccount"
            });
            Assert.Equal<long>(2232, _sut.Control.TotalCreditAmount);
            Assert.Equal<long>(1165, _sut.Control.TotalDebitAmount);

        }

        [Fact]
        public void Batch_EntryHashIsCorrect_SingleEntry()
        {
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                RdfiRtn = "33333333",
                RdfiAccountNumber = "FakeAccount"
            });
            Assert.Equal<long>(33333333, _sut.Control.EntryHash);
        }

        [Fact]
        public void Batch_EntryHashIsCorrect_08Digit()
        {
            // test 2 entries which will create a 8 digit hash
            for (int i = 0; i < 2; i++)
            {
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                    RdfiRtn = "44444444", 
                    RdfiAccountNumber = "FakeAccount"
                });
            }
            Assert.Equal<long>(88888888, _sut.Control.EntryHash);
        }

        [Fact]
        public void Batch_EntryHashIsCorrect_09Digit()
        {
            // test 3 entries which will create a 9 digit hash
            for (int i = 0; i < 3; i++)
            {
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                    RdfiRtn = "44444444",
                    RdfiAccountNumber = "FakeAccount"
                });
            }
            Assert.Equal<long>(133333332, _sut.Control.EntryHash);
        }

        [Fact]
        public void Batch_EntryHashIsCorrect_10Digit()
        {
            // test 20 entries which will create a 10 digit hash
            for (int i = 0; i < 20; i++)
            {
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                    RdfiRtn = "55555555",
                    RdfiAccountNumber = "FakeAccount"
                });
            }
            Assert.Equal<long>(1111111100, _sut.Control.EntryHash);        
        }

        [Fact]
        public void Batch_EntryHashIsCorrect_11Digit()
        {
            // test 100 entries which will create a 11 digit hash to test truncation
            for (int i = 0; i < 511; i++)
            {
                if (i == 510)
                {

                }
                _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
                {
                    TransactionCode = (short)TransactionCodeEnum.AutomatedDepositChecking,
                    RdfiRtn = "66666666",
                    RdfiAccountNumber = "FakeAccount"
                });
            }
            Assert.Equal<long>(4066666326, _sut.Control.EntryHash);
        }

        [Fact]
        public void Batch_HeaderCompanyIdentificationSyncsToControl()
        {
            Assert.Equal(_sut.Header.CompanyIdentification, _sut.Control.CompanyIdentification);
            _sut.Header.CompanyIdentification = "1111222233";
            Assert.Equal("1111222233", _sut.Control.CompanyIdentification);
        }

        [Fact]
        public void Batch_ControlCompanyIdentificationSyncsToHeader()
        {
            Assert.Equal(_sut.Header.CompanyIdentification, _sut.Control.CompanyIdentification);
            _sut.Control.CompanyIdentification = "1111222233";
            Assert.Equal("1111222233", _sut.Header.CompanyIdentification);
        }

        [Fact]
        public void Batch_HeaderServiceClassCodeSyncsToControl()
        {
            Assert.Equal(_sut.Header.ServiceClassCode, _sut.Control.ServiceClassCode);
            _sut.Header.ServiceClassCode = BatchServiceClassCodeConstant.Credits;
            Assert.Equal(BatchServiceClassCodeConstant.Credits, _sut.Control.ServiceClassCode);
        }

        [Fact]
        public void Batch_ControlServiceClassCodeSyncsToHeader()
        {
            Assert.Equal(_sut.Header.ServiceClassCode, _sut.Control.ServiceClassCode);
            _sut.Control.ServiceClassCode = BatchServiceClassCodeConstant.Credits;
            Assert.Equal(BatchServiceClassCodeConstant.Credits, _sut.Header.ServiceClassCode);
        }

        [Fact]
        public void Batch_EntryTraceNumberIsCorrect()
        {
            _sut.Header.OriginatingDfi = "43214321";
            _sut.Header.StandardEntryClassCode = StandardEntryClassCodeConstant.CCD;
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD) {
                 Amount = 999,
                 RdfiAccountNumber = "FakeAccountNumber",
                 RdfiRtn = "22233344",
                 TransactionCode = (short)TransactionCodeEnum.AutomatedDepositSavings
            });

            Assert.Equal("432143210000001", _sut.Entries[0].TraceNumber);

            // add a second one
            _sut.AddEntry(new Entry(StandardEntryClassCodeConstant.CCD)
            {
                Amount = 999,
                RdfiAccountNumber = "FakeAccountNumber",
                RdfiRtn = "22233344",
                TransactionCode = (short)TransactionCodeEnum.AutomatedDepositSavings
            });

            Assert.Equal("432143210000001", _sut.Entries[0].TraceNumber);
            Assert.Equal("432143210000002", _sut.Entries[1].TraceNumber);
        }
    }
}
