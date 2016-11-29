using FileHelpers;
using Nacha.Constants;
using Nacha.Enums;
using Nacha.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nacha.Models
{
    [FixedLengthRecord]
    public class FileHeader : BaseModel
    {
        #region Methods
        public FileHeader()
        {
            // inherited property that can't be set with initializers due to encapsulation
            RecordType = (short)RecordTypeEnum.FileHeader;

            // for readability we have opted for constructor initialization vs field initialization. 
            PriorityCode = FileHeaderConstants.PriorityCode;

            // Date and Time seem to be a part of the same whole but we exposed them as
            // separate fields and properties since FileHelpers uses fields and 
            // we want the api to match the field count from the NACHA spec.
            // Setting CreationDate changes CreationTime and vice versa.
            CreationDate = DateTime.Now;
            CreationTime = CreationDate;

            RecordSize = FileHeaderConstants.RecordSize;
            BlockingFactor = FileHeaderConstants.BlockingFactor;
            FormatCode = FileHeaderConstants.FormatCode;
        }

        public void Validate()
        {
            var _message = new List<String>();

            FieldValidator.Validate(nameof(RecordType), RecordType, ref _message);
            FieldValidator.Validate(nameof(ImmediateDestination), ImmediateDestination, ref _message);
            FieldValidator.Validate(nameof(ImmediateOrigin), ImmediateOrigin, ref _message);
            FieldValidator.Validate(nameof(Id), Id, ref _message);
            FieldValidator.Validate(nameof(ImmediateDestinationName), ImmediateDestinationName, ref _message);
            FieldValidator.Validate(nameof(ImmediateOriginName), ImmediateOriginName, ref _message);

            if (_message.Count > 0)
            {
                throw new ArgumentException(ExceptionConstants.Shared_RequiredProperties +
                    string.Join(", ", _message));
            }

            // the exposed api keeps these values in sync.
            if (!DateTime.Equals(CreationDate, CreationTime))
            {
                throw new InvalidOperationException(
                    ExceptionConstants.FileHeader_CreationDateTimeInvalid);
            }
        }
        #endregion

        #region Fields
        [FieldOrder(20), FieldFixedLength(2)]
        private string _priorityCode;

        [FieldOrder(30), FieldFixedLength(10)]
        private string _immediateDestination;

        [FieldOrder(40), FieldFixedLength(10)]
        private string _immediateOrigin;

        [FieldOrder(50), FieldFixedLength(6), FieldConverter(ConverterKind.Date, "yyMMdd")]
        private DateTime _creationDate;

        [FieldOrder(60), FieldFixedLength(4), FieldConverter(ConverterKind.Date, "HHmm")]
        private DateTime _creationTime;

        [FieldOrder(70), FieldFixedLength(1)]
        private string _id;

        [FieldOrder(80), FieldFixedLength(3)]
        private string _recordSize;

        [FieldOrder(90), FieldFixedLength(2)]
        private short _blockingFactor;

        [FieldOrder(100), FieldFixedLength(1)]
        private short _formatCode;

        [FieldOrder(110), FieldFixedLength(23)]
        private string _immediateDestinationName;

        [FieldOrder(120), FieldFixedLength(23)]
        private string _immediateOriginName;

        [FieldOrder(130), FieldFixedLength(8)]
        private string _referenceCode;
        #endregion

        #region Properties
        public string PriorityCode
        {
            get { return _priorityCode; }
            private set { _priorityCode = value; }
        }

        public string ImmediateDestination
        {
            get { return _immediateDestination; }
            set
            {
                // value must be 10 chars, start with a space, and otherwise be numeric
                if (value.Length != 10 ||
                    value.Substring(0, 1) != " " ||
                    !value.Substring(1, 9).All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentException(
                        ExceptionConstants.FileHeader_ImmediateDestinationInvalid);
                }

                _immediateDestination = value;
            }
        }

        public string ImmediateOrigin
        {
            get { return _immediateOrigin; }
            set
            {
                // value must be 10 chars and all numeric
                if (value.Length != 10 ||
                    !value.All(c => c >= '0' && c <= '9'))
                {
                    throw new ArgumentException(
                        ExceptionConstants.FileHeader_ImmediateOriginInvalid);
                }
                _immediateOrigin = value.Trim();
            }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (!DateTime.Equals(value, _creationDate))
                {
                    _creationDate = value;
                    CreationTime = value;
                }
            }
        }

        public DateTime CreationTime
        {
            get { return _creationTime; }
            set
            {
                if (!DateTime.Equals(value, _creationTime))
                {
                    _creationTime = value;
                    CreationDate = value;
                }
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                // value must be 1 chars and upper case A-Z
                if (value.Length != 1 ||
                    !value.All(c => c >= 'A' && c <= 'Z'))
                {
                    throw new ArgumentException(
                        ExceptionConstants.FileHeader_IdInvalid);
                }
                _id = value.Trim();
            }
        }

        public string RecordSize
        {
            get { return _recordSize; }
            private set { _recordSize = value; }
        }

        public short BlockingFactor
        {
            get { return _blockingFactor; }
            private set { _blockingFactor = value; }
        }

        public short FormatCode
        {
            get { return _formatCode; }
            private set { _formatCode = value; }
        }

        public string ImmediateDestinationName
        {
            get { return _immediateDestinationName; }
            set { _immediateDestinationName = value.Trim(); }
        }

        public string ImmediateOriginName
        {
            get { return _immediateOriginName; }
            set { _immediateOriginName = value.Trim(); }
        }

        public string ReferenceCode
        {
            get { return _referenceCode; }
            set { _referenceCode = value.Trim(); }
        }
        #endregion
    }
}
