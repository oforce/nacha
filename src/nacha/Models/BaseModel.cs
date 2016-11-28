using FileHelpers;
using Nacha.Constants;
using Nacha.Enums;
using System;

namespace Nacha.Models
{
    public class BaseModel
    {
        [FieldOrder(10), FieldFixedLength(1)]
        private short _recordType;

        public short RecordType
        {
            get { return _recordType; }
            protected set
            {
                if (Enum.IsDefined(typeof(RecordTypeEnum), value))
                {
                    _recordType = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(ExceptionConstants.BaseModel_RecordTypeInvalid);
                }
            }
        }
    }
}
