using FileHelpers;
using CMS.Nacha.Constants;
using CMS.Nacha.Enums;
using CMS.Nacha.Helpers;
using System;
using System.Collections.Generic;

namespace CMS.Nacha.Models
{
    [FixedLengthRecord]
    public class FileControl : BaseModel
    {
        public FileControl()
        {
            RecordType = (short)RecordTypeEnum.FileControl;
        }

        public void Validate()
        {
            var _message = new List<string>();

            FieldValidator.Validate(nameof(RecordType), RecordType, ref _message);
            FieldValidator.Validate(nameof(BatchCount), BatchCount, ref _message);
            FieldValidator.Validate(nameof(BlockCount), BlockCount, ref _message);
            FieldValidator.Validate(nameof(EntryAndAddendaCount), EntryAndAddendaCount, ref _message);
            FieldValidator.Validate(nameof(EntryHash), EntryHash, ref _message);
            FieldValidator.Validate(nameof(TotalDebitAmount), TotalDebitAmount, ref _message);
            FieldValidator.Validate(nameof(TotalCreditAmount), TotalCreditAmount, ref _message);

            if (_message.Count > 0)
            {
                throw new ArgumentException(ExceptionConstants.Shared_RequiredProperties +
                    string.Join(", ", _message));
            }
        }



        [FieldOrder(20), FieldFixedLength(6), FieldAlign(AlignMode.Right, '0')]
        private int _batchCount;

        [FieldOrder(30), FieldFixedLength(6), FieldAlign(AlignMode.Right, '0')]
        private int _blockCount;

        [FieldOrder(40), FieldFixedLength(8), FieldAlign(AlignMode.Right, '0')]
        private int _entryAndAddendaCount;

        [FieldOrder(50), FieldFixedLength(10), FieldAlign(AlignMode.Right, '0')]
        private long _entryHash;

        [FieldOrder(60), FieldFixedLength(12), FieldAlign(AlignMode.Right, '0')]
        private long _totalDebitAmount;

        [FieldOrder(70), FieldFixedLength(12), FieldAlign(AlignMode.Right, '0')]
        private long _totalCreditAmount;

        [FieldOrder(80), FieldFixedLength(39)]
        private string _reserved = "";

        public int BatchCount
        {
            get { return _batchCount; }
            internal set { _batchCount = value; }
        }

        public int BlockCount
        {
            get { return _blockCount; }
            internal set { _blockCount = value; }
        }

        public int EntryAndAddendaCount
        {
            get { return _entryAndAddendaCount; }
            internal set { _entryAndAddendaCount = value; }
        }

        public long EntryHash
        {
            get { return _entryHash; }
            internal set { _entryHash = value; }
        }

        public long TotalDebitAmount
        {
            get { return _totalDebitAmount; }
            internal set { _totalDebitAmount = value; }
        }

        public long TotalCreditAmount
        {
            get { return _totalCreditAmount; }
            internal set { _totalCreditAmount = value; }
        }

        public string Reserved
        {
            get { return _reserved; }
        }
    }
}
