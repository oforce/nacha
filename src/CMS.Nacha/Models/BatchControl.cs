using FileHelpers;
using CMS.Nacha.Constants;
using CMS.Nacha.Enums;
using CMS.Nacha.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CMS.Nacha.Models
{
    [FixedLengthRecord]
    public class BatchControl : BaseModel
    {
        public BatchControl()
        {
            RecordType = (short)RecordTypeEnum.BatchControl;
            ServiceClassCode = BatchServiceClassCodeConstant.Mixed;
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Validate()
        {
            var _message = new List<String>();

            FieldValidator.Validate(nameof(RecordType), RecordType, ref _message);
            FieldValidator.Validate(nameof(EntryAndAddendaCount), EntryAndAddendaCount, ref _message);
            FieldValidator.Validate(nameof(EntryHash), EntryHash, ref _message);
            FieldValidator.Validate(nameof(CompanyIdentification), CompanyIdentification, ref _message);
            FieldValidator.Validate(nameof(OriginatingDfi), OriginatingDfi, ref _message);
            FieldValidator.Validate(nameof(BatchNumber), BatchNumber, ref _message);

            if (_message.Count > 0)
            {
                throw new ArgumentException(ExceptionConstants.Shared_RequiredProperties +
                    string.Join(", ", _message));
            }
        }

        [FieldOrder(20), FieldFixedLength(3)]
        private short _serviceClassCode;

        [FieldOrder(30), FieldFixedLength(6), FieldAlign(AlignMode.Right, '0')]
        private int _entryAndAddendaCount;

        [FieldOrder(40), FieldFixedLength(10), FieldAlign(AlignMode.Right, '0')]
        private long _entryHash;

        [FieldOrder(50), FieldFixedLength(12), FieldAlign(AlignMode.Right, '0')]
        private long _totalDebitAmount;

        [FieldOrder(60), FieldFixedLength(12), FieldAlign(AlignMode.Right, '0')]
        private long _totalCreditAmount;

        [FieldOrder(70), FieldFixedLength(10)]
        private string _companyIdentification = "";

        [FieldOrder(80), FieldFixedLength(19)]
        private string _messageAuthenticationCode = "";

        [FieldOrder(90), FieldFixedLength(6)]
        private string _reserved = "";

        [FieldOrder(100), FieldFixedLength(8)]
        private string _originatingDfi;

        [FieldOrder(110), FieldFixedLength(7), FieldAlign(AlignMode.Right, '0')]
        private int _batchNumber;

        public short ServiceClassCode
        {
            get { return _serviceClassCode; }
            set
            {
                if (!ConstantsHelper.IsValid(typeof(BatchServiceClassCodeConstant), value))
                {
                    throw new ArgumentOutOfRangeException("You must use a value from the BatchServiceClassCodeConstant Class");
                }
                if (!_serviceClassCode.Equals(value))
                {
                    _serviceClassCode = value;
                    NotifyPropertyChanged();
                }
            }
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
            set { _totalDebitAmount = value; }
        }

        public long TotalCreditAmount
        {
            get { return _totalCreditAmount; }
            set { _totalCreditAmount = value; }
        }

        public string CompanyIdentification
        {
            get { return _companyIdentification; }
            set
            {
                // value must be 10 chars and all numeric
                if (value.Length != 10 ||
                    !value.All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentException(
                        ExceptionConstants.BatchHeader_CompanyIdentificationInvalid);
                }

                if (!_companyIdentification.Equals(value))
                {
                    _companyIdentification = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string MessageAuthenticationCode
        {
            get { return _messageAuthenticationCode; }
        }

        public string Reserved
        {
            get { return _reserved; }
        }

        public string OriginatingDfi
        {
            get { return _originatingDfi; }
            set { _originatingDfi = value; }
        }

        public int BatchNumber
        {
            get { return _batchNumber; }
            internal set { _batchNumber = value; }
        }
    }
}
