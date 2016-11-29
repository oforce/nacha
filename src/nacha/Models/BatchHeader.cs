using FileHelpers;
using Nacha.Constants;
using Nacha.Enums;
using Nacha.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Nacha.Models
{
    // This will not work for IAT (international) we will build that when the time comes
    [FixedLengthRecord]
    public class BatchHeader : BaseModel, INotifyPropertyChanged
    {

        #region Methods
        public BatchHeader()
        {
            RecordType = (short)RecordTypeEnum.BatchHeader;
            ServiceClassCode = BatchServiceClassCodeConstant.Mixed;
            OriginatorStatusCode = BatchHeaderOriginatorStatusCodeConstant.NonFederalGovernment;
            SettlementDate = "";
        }

        public void Validate()
        {
            var _message = new List<String>();

            FieldValidator.ValidateShort(nameof(RecordType), RecordType, ref _message);
            FieldValidator.ValidateShort(nameof(ServiceClassCode), ServiceClassCode, ref _message);
            FieldValidator.ValidateString(nameof(CompanyName), CompanyName, ref _message);
            FieldValidator.ValidateString(nameof(CompanyIdentification), CompanyIdentification, ref _message);
            FieldValidator.ValidateString(nameof(StandardEntryClassCode), StandardEntryClassCode, ref _message);
            FieldValidator.ValidateString(nameof(CompanyEntryDescription), CompanyEntryDescription, ref _message);
            FieldValidator.ValidateShort(nameof(OriginatorStatusCode), OriginatorStatusCode, ref _message);
            FieldValidator.ValidateString(nameof(OriginatingDfi), OriginatingDfi, ref _message);
            FieldValidator.ValidateInt(nameof(BatchNumber), BatchNumber, ref _message);

            if (_message.Count > 0)
            {
                throw new ArgumentException(ExceptionConstants.Shared_RequiredProperties +
                    string.Join(", ", _message));
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields
        [FieldOrder(20), FieldFixedLength(3)]
        private short _serviceClassCode;

        [FieldOrder(30), FieldFixedLength(16)]
        private string _companyName;

        [FieldOrder(40), FieldFixedLength(20)]
        private string _companyDiscretionaryData;

        [FieldOrder(50), FieldFixedLength(10)]
        private string _companyIdentification = "";

        [FieldOrder(60), FieldFixedLength(3)]
        private string _standardEntryClassCode;

        [FieldOrder(70), FieldFixedLength(10)]
        private string _companyEntryDescription;

        [FieldOrder(80), FieldFixedLength(6)]
        private string _companyDescriptiveDate;

        [FieldOrder(90), FieldFixedLength(6), FieldConverter(ConverterKind.Date, "yyMMdd")]
        private DateTime _effectiveEntryDate;

        [FieldOrder(100), FieldFixedLength(3)]
        private string _settlementDate;

        [FieldOrder(110), FieldFixedLength(1)]
        private short _originatorStatusCode;

        [FieldOrder(120), FieldFixedLength(8)]
        private string _originatingDfi;

        [FieldOrder(130), FieldFixedLength(7), FieldAlign(AlignMode.Right, '0')]
        private int _batchNumber;
        #endregion

        #region Properties
        public short ServiceClassCode
        {
            get { return _serviceClassCode; }
            set
            {
                if (!ConstantsHelper.IsValid(typeof(BatchServiceClassCodeConstant), value))
                {
                    throw new ArgumentOutOfRangeException(ExceptionConstants.BatchHeader_ServiceClassCodeInvalid);
                }

                if (!_serviceClassCode.Equals(value))
                {
                    _serviceClassCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public string CompanyDiscretionaryData
        {
            get { return _companyDiscretionaryData; }
            set { _companyDiscretionaryData = value; }
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

        public string StandardEntryClassCode
        {
            get { return _standardEntryClassCode; }
            set
            {
                if (!ConstantsHelper.IsValid(typeof(StandardEntryClassCodeConstant), value))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.BatchHeader_StandardEntryClassCodeInvalid);
                }

                _standardEntryClassCode = value;
            }
        }

        public string CompanyEntryDescription
        {
            get { return _companyEntryDescription; }
            set { _companyEntryDescription = value; }
        }

        public string CompanyDescriptiveDate
        {
            get { return _companyDescriptiveDate; }
            set { _companyDescriptiveDate = value; }
        }

        public DateTime EffectiveEntryDate
        {
            get { return _effectiveEntryDate; }
            set { _effectiveEntryDate = value; }
        }

        public string SettlementDate
        {
            get { return _settlementDate; }
            private set { _settlementDate = value; }
        }

        public short OriginatorStatusCode
        {
            get { return _originatorStatusCode; }
            set
            {
                if (!ConstantsHelper.IsValid(
                    typeof(BatchHeaderOriginatorStatusCodeConstant), value))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.BatchHeader_OriginatorStatusCodeInvalid);
                }

                _originatorStatusCode = value;
            }
        }

        public string OriginatingDfi
        {
            get { return _originatingDfi; }
            set
            {
                // value must be 8 chars and all numeric
                if (value.Length != 8 ||
                    !value.All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentException(
                        ExceptionConstants.BatchHeader_OriginatingDfiInvalid);
                }
                _originatingDfi = value;
            }
        }

        public int BatchNumber
        {
            get { return _batchNumber; }
            internal set { _batchNumber = value; }
        }
        #endregion
    }
}
