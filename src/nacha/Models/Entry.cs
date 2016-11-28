﻿using FileHelpers;
using Nacha.Constants;
using Nacha.Enums;
using Nacha.Helpers;
using System;
using System.Linq;

namespace Nacha.Models
{
    [FixedLengthRecord]
    public class Entry : BaseModel
    {
        private Entry()
        {
            // parameterless constructor for FileHelpers
        }

        public Entry(string standardEntryClassCode)
        {
            RecordType = (short)RecordTypeEnum.EntryDetail;

            // default to no Addenda Record
            _addendaRecordIndicator = (short)AddendaRecordIndicatorEnum.NotIncluded;

            StandardEntryClassCode = standardEntryClassCode;
        }

        public string GetCheckDigit(string routingTransitNumber)
        {
            if (routingTransitNumber.Length != 8)
            {
                throw new ArgumentOutOfRangeException(
                    ExceptionConstants.Shared_RoutingNumberInvalid);
            }

            int _sum = 0;
            _sum += 3 * Convert.ToInt16(routingTransitNumber.Substring(0, 1));
            _sum += 7 * Convert.ToInt16(routingTransitNumber.Substring(1, 1));
            _sum += 1 * Convert.ToInt16(routingTransitNumber.Substring(2, 1));
            _sum += 3 * Convert.ToInt16(routingTransitNumber.Substring(3, 1));
            _sum += 7 * Convert.ToInt16(routingTransitNumber.Substring(4, 1));
            _sum += 1 * Convert.ToInt16(routingTransitNumber.Substring(5, 1));
            _sum += 3 * Convert.ToInt16(routingTransitNumber.Substring(6, 1));
            _sum += 7 * Convert.ToInt16(routingTransitNumber.Substring(7, 1));

            int _checkDigit = (10 - (_sum % 10));

            if (_checkDigit == 10) { _checkDigit = 0; }

            return Convert.ToString(_checkDigit);
        }

        public void Validate()
        {
            var _message = "";
            FieldValidator.ValidateString(nameof(StandardEntryClassCode), StandardEntryClassCode, ref _message);

            FieldValidator.ValidateShort(nameof(RecordType), RecordType, ref _message);                        
            FieldValidator.ValidateShort(nameof(TransactionCode), TransactionCode, ref _message);
            FieldValidator.ValidateString(nameof(RdfiRtn), RdfiRtn, ref _message);
            FieldValidator.ValidateString(nameof(CheckDigit), CheckDigit, ref _message);
            FieldValidator.ValidateString(nameof(RdfiAccountNumber), RdfiAccountNumber, ref _message);
            FieldValidator.ValidatePositiveLong(nameof(Amount), Amount, ref _message);

            // we cant validate the TraceNumber as its generated by Batch.AddEntry
            //FieldValidator.ValidateString(nameof(TraceNumber), TraceNumber, ref _message);

            if (_message != "")
            {
                throw new ArgumentException(ExceptionConstants.Shared_RequiredProperties +
                    _message.Substring(0, _message.Length - 2));
            }
        }

        // this is not exported. This is used for validating homogenous batch entries
        public string StandardEntryClassCode
        {
            get { return _standardEntryClassCode; }
            set
            {
                if (!ConstantsHelper.IsValid(typeof(StandardEntryClassCodeConstant), value))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_StandardEntryClassCodeInvalid);
                }

                _standardEntryClassCode = value;
            }
        }

        // this is not exported. This is used for validating homogenous batch entries
        [FieldHidden]
        private string _standardEntryClassCode = "";

        [FieldOrder(20), FieldFixedLength(2)]
        private short _transactionCode;

        [FieldOrder(30), FieldFixedLength(8)]
        private string _rdfiRtn;

        [FieldOrder(40), FieldFixedLength(1)]
        private string _checkDigit;

        [FieldOrder(50), FieldFixedLength(17)]
        private string _rdfiAccountNumber;

        [FieldOrder(60), FieldFixedLength(10)]
        private long _amount;

        [FieldOrder(70), FieldFixedLength(15)]
        private string _receiverIdentificationNumber;

        [FieldOrder(80), FieldFixedLength(22)]
        private string _receiverName;

        [FieldOrder(90), FieldFixedLength(2)]
        private string _discretionaryData;

        [FieldOrder(100), FieldFixedLength(1)]
        private short _addendaRecordIndicator;

        [FieldOrder(110), FieldFixedLength(15)]
        private string _traceNumber;

        public short TransactionCode
        {
            get { return _transactionCode; }
            set
            {
                if (!Enum.IsDefined(typeof(TransactionCodeEnum), value))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_TransactionCodeInvalid);
                }
                _transactionCode = value;
            }
        }

        public string RdfiRtn
        {
            get { return _rdfiRtn; }
            set
            {
                // value must be 8 chars and all numeric
                if (value.Length != 8 ||
                    !value.All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_RdfiRtnInvalid);
                }
                _rdfiRtn = value;
                CheckDigit = GetCheckDigit(_rdfiRtn);
            }
        }

        public string CheckDigit
        {
            get { return _checkDigit; }
            set
            {
                // value must be 1 char and all numeric
                if (value.Length != 1 ||
                    !value.All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_CheckDigitInvalid);
                }
                _checkDigit = value;
            }
        }

        public string RdfiAccountNumber
        {
            get { return _rdfiAccountNumber; }
            set { _rdfiAccountNumber = value; }
        }

        public long Amount
        {
            get { return _amount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_AmountInvalid);
                }
                _amount = value;
            }
        }

        public string ReceiverIdentificationNumber
        {
            get { return _receiverIdentificationNumber; }
            set { _receiverIdentificationNumber = value; }
        }

        public string ReceiverName
        {
            get { return _receiverName; }
            set { _receiverName = value; }
        }

        public string DiscretionaryData
        {
            get { return _discretionaryData; }
            set { _discretionaryData = value; }
        }

        public short AddendaRecordIndicator
        {
            get { return _addendaRecordIndicator; }
            set
            {
                if (!Enum.IsDefined(typeof(AddendaRecordIndicatorEnum), value))
                {
                    throw new ArgumentOutOfRangeException(
                        ExceptionConstants.Entry_AddendaRecordIndicatorInvalid);
                }
                _addendaRecordIndicator = value;
            }
        }

        public string TraceNumber
        {
            get { return _traceNumber; }
            internal set { _traceNumber = value; }
        }
    }
}
